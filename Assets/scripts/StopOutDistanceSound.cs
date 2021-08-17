using UnityEngine;
using System.Collections;

public class StopOutDistanceSound : MonoBehaviour {

	AudioSource Sound;
	GameObject CameraObject;
	bool ScriptEnable = true;
	
	void Start (){
		if (GameObject.Find("Main Camera")){
			CameraObject = GameObject.Find("Main Camera").gameObject;
		}else{
			CameraObject = this.gameObject;
		}
		try{
			Sound = GetComponent<AudioSource>();
		}catch{
			ScriptEnable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ScriptEnable == true){
			if (Vector3.Distance(CameraObject.transform.position,transform.position) < Sound.maxDistance){
				SoundPlay(true);
			}else{
				SoundPlay(false);
			}
		}
	}
	
	void SoundPlay(bool Start){
		switch(Start){
		case true:
			if (!Sound.isPlaying){
				Sound.PlayOneShot(Sound.clip);
			}
			break;
		case false:
			Sound.Stop();
			break;
		}
	}
}
