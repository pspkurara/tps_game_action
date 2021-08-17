using UnityEngine;
using System.Collections; 

public class EnemyFollow : MonoBehaviour
{
	private Transform target;
	private Vector3 vec;
	public int EnemyLife = 1;
	public GameObject BlueRockEffectPrefab = null;
	public GameObject BombEffect = null;
	private Pause isPause;
	private GUI_Yurasi GYurashi1;
	private DamageScript DMS;
	public int Damage = 1;
	private EnemyCreat ES;
	public int EnemyKilledScore = 10;
	public int EnemyDestroiAttackBonusScore = 5;
	public float EnemySpeedLocal = 0.07f;
	public Transform PosObject;
	public Transform ViewObject;
	public int NowEnemyStatus;
	public float EnemyDashSpeed = 0.1f;
	public float EnemyDashMaxTime = 1f;
	public float AttackInterval = 1f;
	private float StatusChangeTimes = 0f;
	public float RotationInterval = 0.1f;
	private float AddVector = 0f;
	public float RotationForgiveError = 0.01f;
	public float EnemyNTTime = 1f;
	public float NutralEnemyRotationInterval = 0.01f;
	public int EnemyAffiliationZoneID = 0;
	public int EnemyAffiliationZoneType = 0;
	public int EnemyCharacteristicID = 0;
	
	public float gravity = 20.0f;
	CharacterController controller;
	private Vector3 velocity;
	Vector3 OldtargerRotation;
	public float HeightOffset = 0.5f;
	private bool FindPlayer = false;
	private float RotationVector;
	private Vector3 MyScale;
	private float MySize = 1f;
	bool ChangeScale = false;
	private float MySizeSave;
	public float RotationMax;
	private float TimeMax = 0f;
	private int RandomPull = 0;
	//public AudioClip HitAudio;
	public AudioClip SpeedSound;
	public AudioClip RotationSound;
	private AudioSource Audio;
	public LayerMask OnlyObject;
	
	void Start()
	{
		Audio = GetComponent<AudioSource>();
		MyScale = transform.localScale;
		ES = GameObject.Find("EnemysScript").GetComponent<EnemyCreat>();
		GYurashi1 = GameObject.Find("Life").GetComponent<GUI_Yurasi>();
		isPause = GameObject.Find("PauseScript").GetComponent<Pause>();
		PlayerFind();
		controller = GetComponent<CharacterController>();
		OnEnable();
	}
	
	void OnEnable(){
		int Randomness = Random.Range(0,100);
		if (Randomness >= 50){
			RotationVector = 1f;
		}else{
			RotationVector = -1f;
		}
		ChangeScale = true;
		MySize = 0f;
		NowEnemyStatus = 6;
		velocity = Vector3.zero;
		FindPlayer = false;
	}
	
	void PlayerFind(){
		if (GameObject.Find("Player")){
			target = GameObject.Find("Player").transform;
			DMS = GameObject.Find("Player").GetComponent<DamageScript>();
		}else{
			target = gameObject.transform;
		}
	}
 
	void FixedUpdate()
	{
		if (DMS == null){
			if (GameObject.Find("Player")){
				target = GameObject.Find("Player").transform;
				DMS = GameObject.Find("Player").GetComponent<DamageScript>();
			}
		}
		if (isPause.PauseFlag == false){
			switch(NowEnemyStatus){
			case 0: //FightMode Wait
					if (controller.isGrounded){
						if (StatusChangeTimes >= EnemyNTTime){
						Vector3 MyTransform = velocity;
						switch(RandomPull){
						case 0:
							velocity = new Vector3(Random.Range(0f,0.1f),Random.Range(0f,0.1f),Random.Range(0f,0.1f));
							MyTransform = velocity;
							RandomPull = 1;
							break;
						case 1:
							velocity = MyTransform * -1;
							RandomPull = 0;
							break;
						}
						StatusChangeTimes = 0f;
						if (TimeMax >= EnemyNTTime){
							NowEnemyStatus = 1;
							StatusChangeTimes = 0f;
						}
					}
				}
				if (FindPlayer == false){
					NowEnemyStatus = 4;
					StatusChangeTimes = 0f;
				}
				Gravity();
				break;
			case 1: //FightMode SearchPlayer
				if (controller.isGrounded){
					Transform targetRotation = gameObject.transform;
					Vector3 targetEuler = Vector3.zero, myEuler = Vector3.zero;
					targetRotation.LookAt(target.transform.position);
					targetEuler = targetRotation.eulerAngles;
					OldtargerRotation = target.transform.eulerAngles;
					while(targetEuler.y > 180f && targetEuler.y <= -180f ){
						if (targetEuler.y <= -180){
							targetEuler += new Vector3(0f,180f,0f);
						}else{
							targetEuler -= new Vector3(0f,180f,0f);
						}
					}
					while(myEuler.y > 180f && myEuler.y <= -180f ){
						if (myEuler.y <= -180){
							myEuler += new Vector3(0f,180f,0f);
						}else{
							myEuler -= new Vector3(0f,180f,0f);
						}
					}
					targetEuler = new Vector3(0f,targetEuler.y - myEuler.y,0);
					myEuler = Vector3.zero;
					if (targetEuler.y >= 0f){
						AddVector = targetEuler.y / 100 * 1;
					}else{
						AddVector = targetEuler.y / 100 * -1;
					}
					
					
					RaycastHit hit;
					if (Physics.Raycast(transform.position, targetRotation.eulerAngles, out hit, Mathf.Infinity, OnlyObject)){
						if (hit.collider.gameObject.tag != "Player"){
						}
					}
					
					
					NowEnemyStatus = 2;
					StatusChangeTimes = 0f;
				}
				if (FindPlayer == false){
					NowEnemyStatus = 4;
					StatusChangeTimes = 0f;
				}
				Gravity();
				break;
			case 2: //FightMode LookAtPlayer
				if (controller.isGrounded){
					if (TimeMax >= RotationInterval){
						if (StatusChangeTimes >= 0.01f){
							transform.eulerAngles += new Vector3(0f,AddVector,0f);
							if (transform.eulerAngles.y <= OldtargerRotation.y + RotationForgiveError || transform.eulerAngles.y >= OldtargerRotation.y - RotationForgiveError){
								Transform TargetLook = transform;
								TargetLook.LookAt(target.transform.position);
								transform.eulerAngles = new Vector3(transform.eulerAngles.x,TargetLook.eulerAngles.y,transform.eulerAngles.z);
								Audio.PlayOneShot(SpeedSound);
								NowEnemyStatus = 3;
								TimeMax = 0f;
								StatusChangeTimes = 0f;
							}
							StatusChangeTimes = 0f;
						}
					}
				}
				if (FindPlayer == false){
					NowEnemyStatus = 4;
					StatusChangeTimes = 0f;
				}
				Gravity();
				break;
			case 3: //FightMode DashAtPlayer
				if (controller.isGrounded){
					velocity = transform.forward * EnemyDashSpeed;
					if (TimeMax >= EnemyDashMaxTime){
						velocity = Vector3.zero;
						NowEnemyStatus = 0;
						RandomPull = 0;
						StatusChangeTimes = 0f;
						TimeMax = 0f;
					}
				}
				Gravity();
				break;
			case 4: //NutralMode Wait
				if(StatusChangeTimes >= NutralEnemyRotationInterval){
					velocity = transform.forward * 0.1f;
					transform.eulerAngles += new Vector3(0f,RotationVector,0f);
					StatusChangeTimes = 0f;
				}
				if (FindPlayer == true){
					Audio.PlayOneShot(RotationSound);
					RotationMax = 0f;
					NowEnemyStatus = 5;
					StatusChangeTimes = 0f;
					TimeMax = 0f;
				}
				Gravity();
				break;
			case 5: //NutralMode FindedPlayer
				if(StatusChangeTimes >= 0.01f){
					if (Mathf.Abs(RotationMax) >= 360f * 2f){
						NowEnemyStatus = 2;
						RotationMax = 0f;
						StatusChangeTimes = 0;
					}else{
						transform.eulerAngles += new Vector3(0f,RotationVector * 20f,0f);
						RotationMax += RotationVector * 20f;
					}
					StatusChangeTimes = 0f;
				}
				if (FindPlayer == false){
					NowEnemyStatus = 4;
					StatusChangeTimes = 0f;
				}
				Gravity();
				break;
			case 6: //NutralMode EnemyBirth
				if(StatusChangeTimes >= 0.01f){
					MySize += 0.01f;
					Mathf.Clamp(MySize,0f,1f);
					if (MySize >= 1f){
						ChangeScale = false;
						StatusChangeTimes = 0f;
						NowEnemyStatus = 4;
					}
					StatusChangeTimes = 0f;
				}
				break;
			}
			StatusChangeTimes += (Time.deltaTime);// * ES.EnemySpeed);
			TimeMax += (Time.deltaTime);// * ES.EnemySpeed);
		}
		if (ChangeScale == true){
			transform.localScale = MyScale * MySize;
		}
		transform.eulerAngles = new Vector3(0f,transform.eulerAngles.y,0f);
		controller.Move(velocity);		
		
	}
	
	void Gravity(){
		velocity.y -= gravity;
	}
	
	/* void OnControllerColliderHit(ControllerColliderHit hit) {
		RaycastHit hitx1;
		if (NowEnemyStatus == 3){
			if (Physics.SphereCast(transform.position,controller.radius - 1f, transform.forward,out hitx1)){
			//	velocity = Vector3.zero;
			//	NowEnemyStatus = 0;
			//	RandomPull = 0;
			//	StatusChangeTimes = 0f;
			}
		}
		
		if (hit.gameObject.tag == "Enemy01"){
			Audio.PlayOneShot(HitAudio);
		}
		
	}*/
	
	void OnCollisionEnter(Collision coli){
		RelayOnCollisionEnter(coli);
	}
	
	public void RelayOnCollisionEnter(Collision coli){
		if(coli.gameObject.tag == "Player"){
			isDamageEnemy();
		}
	}
	
	void isDamageEnemy(){
		DMS.DamageHit(Damage);
		GYurashi1.Yure();
	}
	
	public void EnemyDamage(int AttackPower){
		EnemyLife = EnemyLife - AttackPower;
		if (EnemyLife <= 0){
			EnemyDestroi();
		}
	}
	
	void EnemyDestroi(){
		Instantiate(BombEffect,ViewObject.transform.position,ViewObject.transform.rotation);
		//ES.NowEnemys = ES.NowEnemys - 1;
		ES.EnemyZoneTypeDecision (EnemyAffiliationZoneType, EnemyAffiliationZoneID, EnemyCharacteristicID);
		Destroy(gameObject);
	}
	
	public void RelayOnTriggerEnter(Collider coli){
		if (coli.gameObject.tag == "Player"){
			isDamageEnemy();
		}
	}
	
	public void PlayerSearched(bool Searchs){
		FindPlayer = Searchs;
	}
	
	void OnDisable(){
		FindPlayer = false;
	}
	
}