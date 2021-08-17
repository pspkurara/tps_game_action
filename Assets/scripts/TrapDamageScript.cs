using UnityEngine;
using System.Collections;

public class TrapDamageScript : MonoBehaviour {
	
	private GUI_Yurasi GYurashi1;
	private DamageScript DMS;
	public bool AnimEnabled = true;
	public AnimationClip LoopAnim;
	public AudioClip SwingAudio;
	AudioSource Audio;
	AnimationState AnimState;
	Animation Anim;
	public GameObject TrapStage;
	bool OneTime = false;
	TrapStates TStates;
	private GameObject CameraObject;
	// Use this for initialization
	void Start () {
		if (GameObject.Find("Player")){
			DMS = GameObject.Find("Player").GetComponent<DamageScript>();
		}
		GYurashi1 = GameObject.Find("Life").GetComponent<GUI_Yurasi>();
		Anim = TrapStage.GetComponent<Animation>();
		TStates = TrapStage.GetComponent<TrapStates>();
		AnimState = TrapStage.animation[LoopAnim.name];
		Audio = GetComponent<AudioSource>();
		OneTime = true;
		CameraObject = GameObject.Find("Main Camera");
	}
	
	void OnEnable(){
		OneTimeStart(true);
	}
	
	void OnDisable(){
		OneTimeStart(false);
	}
	
	void OnTriggerEnter(Collider coli){
		if(coli.gameObject.tag == "Player"){
			if (GameObject.Find("Player") && DMS == null){
				DMS = GameObject.Find("Player").GetComponent<DamageScript>();
			}
			DMS.DamageHit(TStates.Damage);
			GYurashi1.Yure();
		}
	}
	
	void Update(){
		if (AnimEnabled == false){
			if (OneTime == true){
				AnimState.time = 0;
				Anim.Stop();
				OneTime = false;
			}
		}else{
			if (OneTime == true){
				AnimState.time = 0;
				StopCoroutine("AnimDelay");
				StartCoroutine("AnimDelay");
				OneTime = false;
			}
		}
	}
	
	public void OneTimeStart(bool AnimEnab){
		AnimEnabled = AnimEnab;
		OneTime = true;
	}
	
	IEnumerator AnimDelay(){
		yield return new WaitForSeconds(TStates.AnimStartTimeDelay);
		SoundPlay();
		Anim.Play(LoopAnim.name);
	}
	
	public void SoundPlay(){
		if (Vector3.Distance(CameraObject.transform.position,transform.position) < Audio.maxDistance){
			Audio.PlayOneShot(SwingAudio);
		}
	}
}
