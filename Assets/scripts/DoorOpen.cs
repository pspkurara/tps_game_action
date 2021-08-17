using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {
	public GameObject Door;
	AnimationState animstate;
	public AudioClip DoorSFX;
	public AnimationClip DoorOpenAnim;
	private OptionSetting Opt;
	
	// Use this for initialization
	void Start () {
		Opt = GameObject.Find("Settings").GetComponent<OptionSetting>();
		animstate = Door.animation[DoorOpenAnim.name];
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale != 0f){
			if (Door.animation.isPlaying){
				Door.audio.volume = Opt.SEVolume;
				
			}else{
				Door.audio.volume = 0;
			}
		}else{
			Door.audio.volume = 0;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player")){
			animstate.speed = 1.0f;
			StopCoroutine("SoundPlays");
			StartCoroutine("SoundPlays");
			if (!Door.animation.isPlaying){
				Door.animation.Play(DoorOpenAnim.name);
			}
		}
	}
	void OnTriggerExit(Collider other){
		if (other.CompareTag("Player")){
			animstate.speed = -1.0f;
			StopCoroutine("SoundPlays");
			StartCoroutine("SoundPlays");
			if (!Door.animation.isPlaying){
				animstate.time = animstate.length;
			}
		}
	}	
	public bool checkAnim(){
		return Door.animation.isPlaying;
	}
	
	IEnumerator SoundPlays(){
		Door.audio.PlayOneShot(DoorSFX);
		yield return new WaitForSeconds(DoorSFX.length);
		if (Door.animation.isPlaying){
			StartCoroutine("SoundPlays");
		}
	}
	
}
