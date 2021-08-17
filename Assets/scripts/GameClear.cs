using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {
	
	private int NowMission = 0;
	public bool StartFlag = false;
	public GUITexture MissionClearBackground = null;
	public GameObject FadeOutBlack = null;
	public GameObject GCLogoObj = null;
	private GUITexture GCLogo = null;
	public float WaitTimeLogo = 0.5f;
	
	public AudioClip GameClearMusic = null;
	
	private FadeInFadeOut FOBlack;
	
	private Animation GOLogoAnim;
	private GUI_Yurasi Yure;
	
	private BGMPlay BGMCtrl;
	
	private AudioSource _audios;
	
	
	// Use this for initialization
	void Start () {
		FOBlack = FadeOutBlack.GetComponent<FadeInFadeOut>();
		GCLogo = GCLogoObj.GetComponent<GUITexture>();
		Yure = GCLogoObj.GetComponent<GUI_Yurasi>();
		GCLogo.enabled = false;
		BGMCtrl = GameObject.Find("BGM").GetComponent<BGMPlay>();
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		MissionClearBackground.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (StartFlag == true){
		switch(NowMission){
			case 0:
				//BGMCtrl.BGMStop();
				BGMCtrl.VolumeFlag = 1;
				_audios.PlayOneShot(GameClearMusic);
				GCLogo.enabled = true;
				MissionClearBackground.enabled = true;
				Yure.Yure();
				NowMission = 1;
				break;
			case 1:
				StartCoroutine(WaitTime(5f));
				break;
			case 2:
				FOBlack.StartFlag = true;
				BGMCtrl.FadeOutBG();
				NowMission = 2;
				break;
			case 3:
				if (FOBlack.EndFlag == true){
					NowMission = 4;
				}
				break;
			case 4:
				Application.LoadLevel("Result");
				//Destroy(gameObject);
				break;
			}
		}
	}
	
	void OnStartFlag(){
		StartFlag = true;
	}
	
	void WaitTimeX(float Wait){		
		if (Wait <= 0f){
			NowMission = 3;
		}else{
			Wait -= Time.deltaTime;
		}
	}
	
	IEnumerator WaitTime(float Wait){
		yield return new WaitForSeconds(Wait);
		NowMission ++;
	}
}
