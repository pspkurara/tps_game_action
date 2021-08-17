using UnityEngine;
using System.Collections;

public class OptionBGM : MonoBehaviour {

public float _FadeOutTime = 0.3f;
	
	public float VolumeDown = 0.3f;
	public float VolumeMain = 0.7f;
	
	public int VolumeFlag = 0;
	
	AudioSource _audioSource;

	private ConfigScreen CSs;
	
	void Start(){
		StartMain();
	}
	
	void StartMain(){
		_audioSource = GetComponent<AudioSource>();
		CSs = GameObject.Find("Main").GetComponent<ConfigScreen>();
	}
		
	
	public void BGMPlaying(){
		StartMain();
		CSs = GameObject.Find("Main").GetComponent<ConfigScreen>();
		VolumeMain = CSs.BGMVolumeSetting;
		VolumeDown = CSs.BGMVolumeSetting / 2f;
		_audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		VolumeMain = CSs.BGMVolumeSetting;
		VolumeDown = CSs.BGMVolumeSetting / 2f;
		switch(VolumeFlag){
		case 0:
			_audioSource.volume = VolumeMain;
			break;
		case 1:
			_audioSource.volume = VolumeDown;
			break;
		}
	}
	
	public void FadeOutBG(){
		StartCoroutine(FadeOut(_FadeOutTime));
	}
	
	public void BGMStop(){
		_audioSource.Stop();
	}
	
	IEnumerator FadeOut(float time){
		
		IEnumerator e = Fade(_audioSource.volume,0,time);
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
