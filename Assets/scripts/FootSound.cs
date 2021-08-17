using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(AudioSource))]

public class FootSound : MonoBehaviour {
	float playInterval;
	public float walkint = 0.5f;
	public float runint = 0.04f;
	public float runintDef = 0.02f;
	float nextPlayTime = 0;
	public AudioClip[] otherClips;
	public AudioClip[] woodClips;
	public AudioClip[] dirtClips;
	public AudioClip[] grassClips;
	public AudioClip[] earthClips;
	public AudioClip[] metalClips;
	CharacterController controller;
	public GameObject GrassEffectPrefab;
	AudioSource audioSource;
	protected string gettag = null;
	private ShootControl SC;
	float volsave;
	private Commands Command;
	void Awake(){
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		controller = GetComponent<CharacterController>();
		SC = GetComponent<ShootControl>();
		audioSource = GetComponent<AudioSource>();
		volsave = audioSource.volume;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton(Command.RunCommand)){
			if (Input.GetButton(Command.GunSetupCommand)){
				playInterval = runint;
			}else{
				playInterval = runintDef;	
			}
		}else{
			playInterval = walkint;
		}
		if(Time.time>=nextPlayTime){
			if(controller.isGrounded && controller.velocity!=Vector3.zero && SC.isDamageFlag == false){
				nextPlayTime = Time.time + playInterval;
				if(Input.GetAxis(Command.Horizontal) >= 0.8f || Input.GetAxis(Command.Horizontal) <= -0.8f || Input.GetAxis(Command.Vertical) >= 0.8f || Input.GetAxis(Command.Vertical) <= -0.8f){
				OnFootStrike();
				PlayEffect();
				}
			}
		}
	}
	//
	void OnFootStrike(){
		float volume = Mathf.Clamp01(0.1f+controller.velocity.magnitude*0.3f);
		AudioClip clip = GetAudioClip();
		audioSource.PlayOneShot(clip,volume);
	}
	
	void Down(){
		AudioClip clip = GetAudioClip();
		audioSource.PlayOneShot(clip,volsave);
	}
	
	AudioClip GetAudioClip(){
		AudioClip clip;
		string tag = null;
		RaycastHit hit;
		if(Physics.Raycast(transform.position+new Vector3(0,0.5f,0), -Vector3.up, out hit)){
			tag = hit.collider.tag;
			gettag = tag;
		}
		if(tag=="Wood" || tag=="DarkWood"){
			clip = woodClips[Random.Range(0,woodClips.Length)];
		}else if(tag=="Dirt"){
			clip = dirtClips[Random.Range(0,dirtClips.Length)];
		}else if(tag=="Grass"){
			clip = grassClips[Random.Range(0,grassClips.Length)];
		}else if(tag=="Earth"){
			clip = grassClips[Random.Range(0,earthClips.Length)];
		}else if(tag=="Metal" || tag=="Cars"){
			clip = grassClips[Random.Range(0,metalClips.Length)];
		}else{
			clip = otherClips[Random.Range(0,otherClips.Length)];
		}
		return clip;
	}
	void PlayEffect(){
		RaycastHit hit;
			if(Physics.Raycast(transform.position+new Vector3(0,0.5f,0), -Vector3.up, out hit)){
				if (gettag=="Grass"){
					GameObject EffectGras = (GameObject)Instantiate(GrassEffectPrefab,transform.position,transform.rotation);
					EffectGras.particleSystem.transform.position += new Vector3(0f,0.5f,0f);
				}
			}
	}
}
