using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(BoxCollider))]

public class BgmPlayer : MonoBehaviour {
	//
	public float bgmVolume = 0.5f;
	public float _FadeInTime = 3;
	public float _FadeOutTime = 3;
	AudioSource _audioSource;
	//
	void Awake(){
		_audioSource = GetComponent<AudioSource>();
	}
	
	void OnTriggerEnter(Collider coli){
		if(coli.name!="Player"){
			return;
		}
		StopAllCoroutines();
		StartCoroutine(FadeIn(_FadeInTime));
	}
	
	void OnTriggerExit(Collider coli){
		if(coli.name!="Player"){
			return;
		}
		StopAllCoroutines();
		StartCoroutine(FadeOut(_FadeOutTime));
	}
	
	IEnumerator FadeIn(float time){
		_audioSource.Play();
		IEnumerator e = Fade(0,bgmVolume,time);
		while(e.MoveNext()){
			yield return e.Current;
		}
	}
	
	IEnumerator FadeOut(float time){
		IEnumerator e = Fade(bgmVolume,0,time);
		while(e.MoveNext()){
			yield return e.Current;
		}
		_audioSource.Stop();
	}
	
	IEnumerator Fade(float startVol,float endVol,float time){
		float timeRatio = 1/time;
		float ntime = 0;
		while(ntime<=1.0){
			ntime += timeRatio*Time.deltaTime;
			_audioSource.volume = Mathf.Lerp(startVol,endVol,ntime);
			yield return 0;
		}
	}
}
