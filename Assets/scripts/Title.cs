using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	
	public AudioClip EnterSound = null;
	public AudioClip SelectSound = null;
	public AudioClip BackSound = null;
	public AudioClip OpeningFx = null;
	public AudioClip BomSFX = null;
	
	public float FadeOutTime = 3;
	
	enum MenuSelection{
		TStart = 0,
		TReword = 1,
		TOptions = 2,
		TQuit = 3,
	}
	
	private MenuSelection EMax;
	private MenuSelection EMin;
	private MenuSelection EnableSelect;
	private AudioSource _audios;
	private AudioSource BGMaudios;
	private Color DisableColor;
	private Color EnableColor;
	
	public GUITexture Text_Options;
	public GUITexture Text_Reword;
	public GUITexture Text_Start;
	public GUITexture Text_Quit;
	
	public GameObject MainLogo = null;
	public GameObject CharacterIllust = null;
	public GameObject CharacterIllustNoise = null;
	public GameObject CharacterIllustBack = null;
	public GameObject Texs = null;
	public GameObject WhiteOut = null;
	public GameObject Noise03 = null;
	public GameObject FadeIn = null;
	public GameObject FadeOut = null;
	
	private Color MainLogoColor;
	private Color CharacterIllustColor;
	
	public bool Wait = true;
	private bool GoFlag = false;
	
	private FadeInFadeOut FI;
	private FadeInFadeOut FO;
	
	private SaveScript Saves;
	private OptionSetting Opts;
	private ScoreResult Score;
	private Commands Command;
	int NowAnimPlay = 0;

	bool animstartflg = false;
	AnimEndFlag[] AnimEnd;

	void Start () {
		Saves = GetComponent<SaveScript>();
		Saves.Load();
		Saves.Save();
		Colorset();
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		Opts = GameObject.Find("Settings").GetComponent<OptionSetting>();
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Score.ScoreReset();
		Wait = true;
		_audios = GetComponent<AudioSource>();
		BGMaudios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		FO = FadeOut.GetComponent<FadeInFadeOut>();
		FI = FadeIn.GetComponent<FadeInFadeOut>();
		EMin = MenuSelection.TStart;
		EMax = MenuSelection.TQuit;
		
		_audios.volume = Opts.SEVolume;
		BGMaudios.volume = Opts.BGMVolume;
		BGMaudios.Play();	
		animstartflg = true;
	}
	
	void Colorset(){
		DisableColor = new Color (0.2f,0.2f,0.2f,1f);
		EnableColor = Color.white;
		
		MainLogoColor = MainLogo.guiTexture.color;
		CharacterIllustColor = CharacterIllust.guiTexture.color;
		
		Text_Start.color = Color.black;
		Text_Reword.color = Color.black;
		Text_Options.color = Color.black;
		Text_Quit.color = Color.black;
		MainLogo.guiTexture.color = Color.black;
		CharacterIllust.guiTexture.color = Color.black;
	}
	
	public void PlayIntroAnims(int PlayIDx){
		switch (PlayIDx){
		case 0:
			AnimEnd = new AnimEndFlag[3];
			//NowAnimPlay = 0;
			FI.StartFlag = true;
			_audios.PlayOneShot(OpeningFx);
			CharacterIllust.animation.Play("CharacterIllust");
			AnimEnd[0] = CharacterIllust.gameObject.GetComponent<AnimEndFlag>();
			MainLogo.animation.Play("MainLogo");
			AnimEnd[1] = MainLogo.gameObject.GetComponent<AnimEndFlag>();
			Texs.animation.Play("Texs");
			AnimEnd[2] = Texs.gameObject.GetComponent<AnimEndFlag>();
			break;
		case 1:
			
			NowAnimPlay = 1;
			_audios.Stop ();
			for (int i = 0; i < 3;i++){
				if (AnimEnd[i].AnimEndedFlag == false){
					AnimEnd[i].AnimGoLastFrame();
				}
			}
			_audios.PlayOneShot(BomSFX);
			WhiteOut.animation.Play("WhiteOut");
			Noise03.animation.Play("Noise3Effect");
			CharacterIllustBack.animation.Play("CharacterIllustBack");
			CharacterIllustNoise.animation.Play("CharacterIllust_Noise");
			MainLogo.animation.Play("MainTitleBure01");
			
			Text_Start.color = EnableColor;
			Text_Reword.color = DisableColor;
			Text_Options.color = DisableColor;
			Text_Quit.color = DisableColor;
			MainLogo.guiTexture.color = MainLogoColor;
			CharacterIllust.guiTexture.color = CharacterIllustColor;
			break;
		}
		
	}
	
	void Update () {

		if(Wait == true && NowAnimPlay == 0){

			if(animstartflg){
				animstartflg = false;
				PlayIntroAnims(0);
			}
			if (Input.GetButtonDown(Command.EnterCommand)){
				PlayIntroAnims(1);
				animstartflg = false;
			}

		}else if(Wait == false && NowAnimPlay == 1){

			if (Input.GetButtonDown(Command.DPadDownCommand)){
				_audios.PlayOneShot(SelectSound);
				if (EMax <= EnableSelect){
					EnableSelect = EMin;
				}else{
					EnableSelect ++;
				}
			}else if (Input.GetButtonDown(Command.DPadUpCommand)){
				_audios.PlayOneShot(SelectSound);
				if (EMin >= EnableSelect){
					EnableSelect = EMax;
				}else{
					EnableSelect --;
				}
			}
			
			if (EnableSelect == MenuSelection.TStart){
				Text_Start.color = EnableColor;
				Text_Reword.color = DisableColor;
				Text_Options.color = DisableColor;
				Text_Quit.color = DisableColor;
				EnableSelect = MenuSelection.TStart;
			}else if (EnableSelect == MenuSelection.TReword){
				Text_Start.color = DisableColor;
				Text_Reword.color = EnableColor;
				Text_Options.color = DisableColor;
				Text_Quit.color = DisableColor;
				EnableSelect = MenuSelection.TReword;
			}else if (EnableSelect == MenuSelection.TOptions){
				Text_Start.color = DisableColor;
				Text_Reword.color = DisableColor;
				Text_Options.color = EnableColor;
				Text_Quit.color = DisableColor;
				EnableSelect = MenuSelection.TOptions;
			}else if (EnableSelect == MenuSelection.TQuit){
				Text_Start.color = DisableColor;
				Text_Reword.color = DisableColor;
				Text_Options.color = DisableColor;
				Text_Quit.color = EnableColor;
				EnableSelect = MenuSelection.TQuit;
			}
			
			if (Input.GetButtonDown(Command.EnterCommand)){
				_audios.PlayOneShot(EnterSound);
				StartCoroutine(FadeOutBGM(FadeOutTime));
				Wait = true;
				GoFlag = true;
				FO.StartFlag = true;
			}
		}else{
			if (GoFlag == true){
				if (FO.EndFlag == true){
					GoToThe(EnableSelect);
				}
			}
		}
			
	}
		
	void GoToThe (MenuSelection Basho){
			EnterSelect(Basho);
			Wait = false;
	}
	
	void EnterSelect(MenuSelection MSelect){
		switch(MSelect){
			case MenuSelection.TStart:
				Application.LoadLevel ("StageSelect");
				break;
			case MenuSelection.TReword:
				Application.LoadLevel ("Reword");
				break;
			case MenuSelection.TOptions:
				Application.LoadLevel ("Options");
				break;
			case MenuSelection.TQuit:
				Application.Quit();
				break;
		}
	}
	
	public void WaitEdit(bool Waiting){
		Wait = Waiting;
	}
	
	IEnumerator FadeOutBGM(float time){
		IEnumerator e = Fade(Opts.BGMVolume,0,time);
		while(e.MoveNext()){
			yield return e.Current;
		}
		
	}
	
	IEnumerator Fade(float startVol,float endVol,float time){
		float timeRatio = 1/time;
		float ntime = 0;
		while(ntime<=1.0){
			ntime += timeRatio*Time.deltaTime;
			BGMaudios.volume = Mathf.Lerp(startVol,endVol,ntime);
			yield return 0;
		}
	}
}
