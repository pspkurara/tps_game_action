using UnityEngine;
using System.Collections;

public class GameOverEffect : MonoBehaviour {
	
	private int NowMission = 0;
	public bool StartFlag = false;
	public GameObject FadeOutRed = null;
	public GameObject FadeOutBlack = null;
	public GameObject LogoYurashi = null;
	public GameObject GOLogoObj = null;
	private GUITexture GOLogo = null;
	public AnimationClip GOLogoAnimation = null;
	public float WaitTimeLogo = 0.5f;
	
	public AudioClip GameOverMusic= null;
	
	private FadeInFadeOut FORed, FOBlack;
	
	private Animation GOLogoAnim;
	private GUI_Yurasi Yure;
	
	private BGMPlay BGMCtrl;
	
	private AudioSource _audios;
	private Pause PauseFlag;
	private OptionSetting Options;
	
	
	// Use this for initialization
	void Start () {
		PauseFlag = GameObject.Find("PauseScript").GetComponent<Pause>();
		FORed = FadeOutRed.GetComponent<FadeInFadeOut>();
		FOBlack = FadeOutBlack.GetComponent<FadeInFadeOut>();
		GOLogoAnim = GOLogoObj.GetComponent<Animation>();
		GOLogo = GOLogoObj.GetComponent<GUITexture>();
		Yure = LogoYurashi.GetComponent<GUI_Yurasi>();
		GOLogo.enabled = false;
		BGMCtrl = GameObject.Find("BGM").GetComponent<BGMPlay>();
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (StartFlag == true){
		switch(NowMission){
			case 0:
				PauseFlag.OKInput = false;
				BGMCtrl.BGMStop();
				_audios.volume = Options.BGMVolume;
				_audios.PlayOneShot(GameOverMusic);
				FORed.StartFlag = true;
				NowMission ++;
				break;
			case 1:
				if (FORed.EndFlag == true){
					GOLogo.enabled = true;
					Yure.Yure();
					GOLogoAnim.Play(GOLogoAnimation.name);
					NowMission ++;
				}
				break;
			case 2:
				GameOverWait();
				break;
			case 3:
				if (FOBlack.EndFlag == true){
					NowMission ++;
				}
				break;
			case 4:
				Application.LoadLevel("Title");
				_audios.volume = Options.SEVolume;
				//Destroy(gameObject);
				break;
			}
		}
	}
	
	void OnStartFlag(){
		StartFlag = true;
		FORed.StartFlag = true;
	}
	
	void GameOverWait(){
		if (WaitTimeLogo <= 0f){
			FOBlack.StartFlag = true;
			NowMission ++;
		}else{
			WaitTimeLogo -= Time.deltaTime;
		}
	}
}
