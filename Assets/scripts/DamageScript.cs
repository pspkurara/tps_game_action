using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {
	
	public AnimationClip downAnimation;
	public AnimationClip damageAnimation;
	public AnimationClip deadhitAnimation;
	public GameObject BodyObj;
	public AudioClip BloodAudio = null;
	public GameObject BloodEffect = null;
	private AudioSource AS;
	private HUD_GUIs HG;
	private PlayerStatus PStatus;
	private Transform PLTransform;
	public Transform BloodShots;
	private bool DeathFlag = false, DeathFlag2 = false;
	private CharacterController CC;
	private BoxCollider CCol;
	public float GameOverStartWaitTime = 1f;
	private GameOverEffect GOF;
	
	private ThirdPersonController TPS;
	// Use this for initialization
	void Start () {
		if(GameObject.Find("GameOverScript")){
			GOF = GameObject.Find("GameOverScript").GetComponent<GameOverEffect>();
		}
		CC = GetComponent<CharacterController>();
		CCol = BodyObj.GetComponent<BoxCollider>();
		TPS = GetComponent<ThirdPersonController>();
		AS = GetComponent<AudioSource>();
		PStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
		if (GameObject.Find("GUIs")){
			HG = GameObject.Find("GUIs").GetComponent<HUD_GUIs>();
		}
		animation[deadhitAnimation.name].speed = TPS.downSpeed;
		animation[downAnimation.name].speed = TPS.downSpeed;
		animation[damageAnimation.name].speed = TPS.damageSpeed;
		animation[downAnimation.name].layer = TPS.DamagesLayer;
		animation[damageAnimation.name].layer = TPS.DamagesLayer;
		animation[deadhitAnimation.name].layer = TPS.DamagesLayer;
	}
	
	public void DamageHit(int DamageLevel){
		if (PStatus.PlayerLife - 1 <= 0f){
			if (DeathFlag == false){
				//GameOverStart();
				if(GameObject.Find("GameOverScript")){
					GOF.StartFlag = true;
				}
				CC.enabled = false;
				CCol.enabled = true;
				animation.PlayQueued(downAnimation.name,QueueMode.PlayNow);
				TPS._characterState = ThirdPersonController.CharacterState.Downing;
				DeathFlag = true;
			}
			if(DeathFlag2 == true && DeathFlag == true){
				animation.PlayQueued(deadhitAnimation.name,QueueMode.PlayNow);
			}
			if (PStatus.PlayerLife < 0){
				PStatus.PlayerLife = 0;
			}
		}else{
			StopCoroutine("AnimationCheck");
			TPS._characterState = ThirdPersonController.CharacterState.Damaging;
			animation.PlayQueued(damageAnimation.name,QueueMode.PlayNow);
			StartCoroutine(AnimationCheck());
		}
		Instantiate(BloodEffect,PLTransform.position,PLTransform.rotation * new Quaternion(0f,0f,0f,-90f));
		AS.PlayOneShot(BloodAudio);
		PStatus.NoDamageFlag = false;
		PStatus.PlayerLife -= DamageLevel;
		if (HG != null){
			HG.Refresh(false);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		PLTransform = BloodShots.transform;
	}
	
	IEnumerator AnimationCheck()
	{
		while(animation.isPlaying){
			yield return null;
		}
		TPS.ControlEnabled();
		
	}
	
	void OnDeathFlag(){
		DeathFlag2 = true;
	}
	
	IEnumerator GameOverStart(){
		yield return new WaitForSeconds(GameOverStartWaitTime);
		if(GameObject.Find("GameOverScript")){
			GOF.StartFlag = true;
		}
		
	}
}
