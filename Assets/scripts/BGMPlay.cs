using UnityEngine;
using System.Collections;

public class BGMPlay : MonoBehaviour {
	
	public float _FadeOutTime = 0.3f;
	
	public float VolumeDown = 0.3f;
	public float VolumeMain = 0.7f;
	
	public int VolumeFlag = 0;

	public float RealVolume = 0.7f;
	
	AudioSource _audioSource;
	
	private OptionSetting Options;

	void Start(){
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		_audioSource = GetComponent<AudioSource>();
		VolumeMain = Options.BGMVolume;
		VolumeDown = Options.BGMVolume / 2f;
	}
		
	
	public void BGMPlaying(){
		Start();
		_audioSource.Play();
	}

	void Update () {
		switch(VolumeFlag){
		case 0:
			_audioSource.volume = VolumeMain * RealVolume;
			break;
		case 1:
			_audioSource.volume = VolumeDown * RealVolume;
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
