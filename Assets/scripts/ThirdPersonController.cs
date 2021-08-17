using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class ThirdPersonController : MonoBehaviour {
	
	// Require a character controller to be attached to the same game object
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	public AnimationClip squatAnimation;
	
	
	public AnimationClip idleAnimationKoshi;
	public AnimationClip walkAnimationKoshi;
	public AnimationClip runAnimationKoshi;
	public AnimationClip jumpPoseAnimationKoshi;
	public AnimationClip squatAnimationKoshi;
	
	public float walkMaxAnimationSpeed = 0.75f;
	public float trotMaxAnimationSpeed = 1.0f;
	public float runMaxAnimationSpeed = 1.0f;
	public float jumpAnimationSpeed = 1.15f;
	public float landAnimationSpeed = 1.0f;
	
	private Animation _animation;
	
	public enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
		Squating = 5,
		Damaging = 6,
		Downing = 7,
		Deads = 8,
	}
	
	public CharacterState _characterState;
	
	// The speed when walking
	public float walkSpeed = 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 6.0f;
	public float runSpeedDef = 8.0f;
	
	public float squatSpeed = 3.0f;
	
	public float damageSpeed = 1.0f;
	public float downSpeed = 1.0f;
	
	public float inAirControlAcceleration = 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 0.5f;
	
	// The gravity for the character
	public float gravity = 20.0f;
	// The gravity in controlled descent mode
	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	public float trotAfterSeconds = 3.0f;
	
	public int KoshiLayer = 5;
	public int DamagesLayer = 10;
	
	public bool canJump = true;
	public bool canSquat = true;
		
	public int Weapon = 1;
	
	private float jumpRepeatTime = 0.05f;
	private float jumpTimeout = 0.15f;
	private float groundedTimeout = 0.25f;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0f;
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0f;
	// The current x-z move speed
	private float moveSpeed = 0.0f;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags;
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping = false;
	private bool jumpingReachedApex = false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0f;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime = -10.0f;
	// Last time we performed a jump
	private float lastJumpTime = -1.0f;
	
	private bool squating = false;
	
	public bool ControllEnabled = true;
	
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	//private float lastJumpStartHeight = 0.0f;
	
	
	private Vector3 inAirVelocity = Vector3.zero;
	
	private float lastGroundedTime = 0.0f;
	
	
	public bool isControllable = true;

	public ShootControl ShootC;
	
	private Commands Command;
	
	void Awake ()
	{
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		moveDirection = transform.TransformDirection(Vector3.forward);
		ShootC = GetComponent<ShootControl>();
		_animation = GetComponent<Animation>();
		if(!_animation)
			print("The character you would like to control doesn't have animations. Moving her might look weird.");
		
		/*
	public var idleAnimation : AnimationClip;
	public var walkAnimation : AnimationClip;
	public var runAnimation : AnimationClip;
	public var jumpPoseAnimation : AnimationClip;	
		*/
		if(!idleAnimation) {
			_animation = null;
			print("No idle animation found. Turning off animations.");
		}
		if(!walkAnimation) {
			_animation = null;
			print("No walk animation found. Turning off animations.");
		}
		if(!runAnimation) {
			_animation = null;
			print("No run animation found. Turning off animations.");
		}
		if(!jumpPoseAnimation && canJump) {
			_animation = null;
			print("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
		if(!squatAnimation && canSquat){
			_animation = null;
			print("No squat animation found and the character has canSquat enabled. Turning off animations.");
		}
				
	}
	
	
	void UpdateSmoothedMovementDirection ()
	{
		Transform cameraTransform = Camera.main.transform;
		bool grounded = IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
	
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0f, -forward.x);
		float v;
		float h;
		if (ShootC.isDamageFlag == false){
			v = Input.GetAxisRaw(Command.Vertical);
			h = Input.GetAxisRaw(Command.Horizontal);
		}else{
			v = 0f;
			h = 0f;
		}
	
		// Are we moving backwards or looking backwards
		if (v < -0.2f)
			movingBack = true;
		else
			movingBack = false;
		
		bool wasMoving = isMoving;
		isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
			
		// Target direction relative to the camera
		Vector3 targetDirection = h * right + v * forward;
		
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
	
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9f && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			var curSmooth = speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
		
			_characterState = CharacterState.Idle;
			
			// Pick speed modifier
			if (Input.GetButton (Command.RunCommand))
			{
				if (Input.GetButton(Command.GunSetupCommand)){
					targetSpeed *= runSpeed;
				}else{
					targetSpeed *= runSpeedDef;
				}
				_characterState = CharacterState.Running;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
			}
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
			
			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}
		// In air controls
		else
		{
			// Lock camera while in air
			if (jumping || squating)
				lockCameraTimer = 0.0f;
				
	
			if (isMoving)
				inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
		}
		
	
			
	}
	
	
	void ApplyJumping ()
	{
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
	
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing		
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void Squat ()
	{
		if (isControllable && jumping == false && canSquat == true)
		{
			if (ShootC.isDamageFlag == false){
				if(Input.GetButton(Command.SquatCommand))
				{
					_characterState = CharacterState.Squating;
					print("Squat");
				}
			}
		}	
			
	}
	
	
	void ApplyGravity ()
	{
		if (isControllable)	// don't move player at all if not controllable.
		{
			// Apply gravity
			//bool jumpButton = false;
			if (ShootC.isDamageFlag == false){
			//	jumpButton = Input.GetButton("Jump");
			}
			
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
		
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	
	float CalculateJumpVerticalSpeed (float targetJumpHeight)
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	void DidJump ()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		//lastJumpStartHeight = transform.position.y;
		lastJumpButtonTime = -10f;
		
		_characterState = CharacterState.Jumping;
	}
	
	void Update() {
		
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
	
		if (ShootC.isDamageFlag == false){
			if (Input.GetButtonDown (Command.JumpCommand))
			{
				lastJumpButtonTime = Time.time;
			}
		}
	
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
	
		// Apply jumping logic
		ApplyJumping ();
		
		// Calculate actual motion
		Vector3 movement = moveDirection * moveSpeed + new Vector3 (0f, verticalSpeed, 0f) + inAirVelocity;
		movement *= Time.deltaTime;
		
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		
		if (ShootC.isDamageFlag == false){
			//動かしてあたっていたらどこに当たっているかを返す.
			collisionFlags = controller.Move(movement);
		}
			
		Squat();
	
		
		// ANIMATION sector
		
		if(_animation) {
			if	(_characterState == CharacterState.Damaging){
			}else if(_characterState == CharacterState.Downing){
				IsDamage();
			}else if (_characterState == CharacterState.Deads){
				
			}else{
				if(_characterState == CharacterState.Jumping) 
				{
					if(!jumpingReachedApex) {
						_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimation.name);
						_animation[jumpPoseAnimationKoshi.name].speed = jumpAnimationSpeed;
						_animation[jumpPoseAnimationKoshi.name].layer = KoshiLayer;
						_animation[jumpPoseAnimationKoshi.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimationKoshi.name);
					} else {
						_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
						_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimation.name);
						_animation[jumpPoseAnimationKoshi.name].speed = -landAnimationSpeed;
						_animation[jumpPoseAnimationKoshi.name].layer = KoshiLayer;
						_animation[jumpPoseAnimationKoshi.name].wrapMode = WrapMode.ClampForever;
						_animation.CrossFade(jumpPoseAnimationKoshi.name);			
					}
				} 
				else 
				{
					if(controller.velocity.sqrMagnitude < 0.1f) {
						_animation.CrossFade(idleAnimation.name);
						_animation[idleAnimationKoshi.name].layer = KoshiLayer;
						_animation.CrossFade(idleAnimationKoshi.name);
						if(_characterState == CharacterState.Squating) {
							_animation[squatAnimation.name].speed = squatSpeed;
							_animation[squatAnimationKoshi.name].layer = KoshiLayer;
							_animation.CrossFade(squatAnimation.name);
							_animation.CrossFade(squatAnimationKoshi.name);
						}
					}
					else
					{
						if(_characterState == CharacterState.Running) {
							_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
							_animation.CrossFade(runAnimation.name);
							_animation[runAnimationKoshi.name].layer = KoshiLayer;
							_animation[runAnimationKoshi.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
							_animation.CrossFade(runAnimationKoshi.name);
						}
						else if(_characterState == CharacterState.Trotting) {
							_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
							_animation.CrossFade(walkAnimation.name);
							_animation[walkAnimationKoshi.name].layer = KoshiLayer;
							_animation[walkAnimationKoshi.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
							_animation.CrossFade(walkAnimationKoshi.name);
						}
						else if(_characterState == CharacterState.Walking) {
							_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
							_animation.CrossFade(walkAnimation.name);
							_animation[walkAnimationKoshi.name].layer = KoshiLayer;
							_animation[walkAnimationKoshi.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
							_animation.CrossFade(walkAnimationKoshi.name);
						}
					}
				}
			}
		}
		// ANIMATION sector
		
		// Set rotation to the move direction
		if (IsGrounded())
		{
			
			transform.rotation = Quaternion.LookRotation(moveDirection);
				
		}	
		else
		{
			Vector3 xzMove = movement;
			xzMove.y = 0f;
			if (xzMove.sqrMagnitude > 0.001f)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}	
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void OnControllerColliderHit (ControllerColliderHit hit)
	{
	//	Debug.DrawRay(hit.point, hit.normal);
		if (hit.moveDirection.y > 0.01f) 
			return;
	}
	
	public float GetSpeed () {
		return moveSpeed;
	}
	
	public bool IsJumping () {
		return jumping;
	}
	
	public bool IsSquating () {
		return squating;
	}
	
	public bool IsGrounded () {
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	public Vector3 GetDirection () {
		return moveDirection;
	}
	
	public bool IsMovingBackwards () {
		return movingBack;
	}
	
	public float GetLockCameraTimer () 
	{
		return lockCameraTimer;
	}
	
	public bool IsMoving ()
	{
		return Mathf.Abs(Input.GetAxisRaw(Command.Vertical)) + Mathf.Abs(Input.GetAxisRaw(Command.Horizontal)) > 0.5;
	}
	
	public bool HasJumpReachedApex ()
	{
		return jumpingReachedApex;
	}
	
	public bool IsGroundedWithTimeout ()
	{
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	public void Reset ()
	{
		gameObject.tag = "Player";
	}
	
	public void IsDamage(){
		ShootC.isDamageFlag = true;
	}
	
	public void ControlEnabled(){
		ShootC.isDamageFlag = false;
	}
}
