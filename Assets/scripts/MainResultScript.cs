using UnityEngine;
using System.Collections;

public class MainResultScript : MonoBehaviour {
	
	public string[] ClearTimeText = null;
	public string TotalTimeText = null;
	public string TotalScoreText = null;
	public string GettedCasheText = null;
	public GUITexture ResultLogo = null;
	
	public AudioClip EnterSound = null;
	public AudioClip BackSound = null;
	public AudioClip SelectSound = null;
	
	public GameObject FadeInTexture = null;
	public GameObject FadeOutTexture = null;
	
	public GameObject WindowBack = null;
	
	private FadeInFadeOut FadeOut, FadeIn;
	
	public float OpenSpeed = 0.5f;
	public float MaxWidth = 0.9f;
	enum EventWindowMode{
		ClosedStart = 0,
		Opened = 1,
		Closing = 2,
		Opening = 3,
		ClosedEnd = 4,
	}
	private EventWindowMode NowEventWindow;
	private AudioSource _audios;
	private ScoreResult SR;
	private string TimeSet;
	
	private GUITexture BackWindow;
	
	private SaveScript Save;
	private OptionSetting Opt;
	private Commands Command;
	private int Missions = 0;
	
	public bool GUIEnabled = false;
	public float ScrollBarScale = 30f;
	public Vector2 ScrollAreaScale = new Vector2(Screen.width,50f);
	public float GUISide = 30f;
	public float GUITop = 30f;
	public float GUIBottom = 30f;
	public GUIStyle Style, Style2;
	public Vector2 scrollPosition = Vector2.zero;
	public float GraphFontSize = 10f;
	public float ScrollSpeed = 1f;
	public float ScrollWait = 0.5f;
	float Waits;

	private int GetCashe = 0;
	
	private SlideListDatas Datas;
	private TrophyDatas TDatas;

	public string[] TrophyGetTexts = {null,null};
	
	// Use this for initialization
	void Start () {
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		SR = GameObject.Find("Scores").GetComponent<ScoreResult>();
		TDatas = GameObject.Find("Scores").GetComponent<TrophyDatas>();
		Opt = GameObject.Find("Settings").GetComponent<OptionSetting>();
		BackWindow = WindowBack.GetComponent<GUITexture>();
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Datas = GameObject.Find("Scores").GetComponent<SlideListDatas>();
		BackWindow.enabled = false;
		Save = GetComponent<SaveScript>();
		TextEnabled(false);
		FadeOut = FadeOutTexture.GetComponent<FadeInFadeOut>();
		FadeIn = FadeInTexture.GetComponent<FadeInFadeOut>();
		FadeIn.StartFlag = true;
		NowEventWindow = EventWindowMode.ClosedStart;
		WindowBack.transform.localScale = new Vector3(0f,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
		ResultSetting();
	}
	
	// Update is called once per frame
	void Update () {
	switch(NowEventWindow){
		case EventWindowMode.ClosedStart:
			if (FadeIn.EndFlag == true){
				WindowOpen();
			}
			break;
		case EventWindowMode.Opening:
			if (WindowBack.transform.localScale.x >= MaxWidth){
				TextEnabled(true);
				WindowBack.transform.localScale = new Vector3(MaxWidth,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
				NowEventWindow = EventWindowMode.Opened;
			}else{
				WindowBack.transform.localScale = WindowBack.transform.localScale + new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case EventWindowMode.Opened:
			Controll();
			break;
		case EventWindowMode.Closing:
			if (WindowBack.transform.localScale.x <= 0f){
				WindowBack.transform.localScale = new Vector3(0f,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
				BackWindow.enabled = false;
				NowEventWindow = EventWindowMode.ClosedEnd;
				FadeOut.StartFlag = true;
			}else{
				TextEnabled(false);
				WindowBack.transform.localScale = WindowBack.transform.localScale - new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case EventWindowMode.ClosedEnd:
			if (FadeOut.EndFlag == true){
				Application.LoadLevel("StageSelect");
			}
			break;
		}
	}
	
	void WindowOpen(){
		_audios.PlayOneShot(EnterSound);
		BackWindow.enabled = true;
		NowEventWindow = EventWindowMode.Opening;
	}
	
	void WindowClose(){
		_audios.PlayOneShot(BackSound);
		NowEventWindow = EventWindowMode.Closing;
	}
	
	void TextEnabled(bool Enab){
		GUIEnabled = Enab;
		ResultLogo.enabled = Enab;
	}

	void ResultSetting(){
		SR = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Missions = 0;
		SR.TotalScore = 0;
		SR.ClearTimeNowGame = 0f;
		for (int i = 0;i < SR.MissionElapsedTime.Length;i++){
			ClearTimeText[i] = "";
			TimeRefresh(SR.MissionElapsedTime[i]);
			SR.ClearTimeNowGame += SR.MissionElapsedTime[i];
			if (SR.MissionElapsedTime[i] > 0f){
				Missions += 1;
				ClearTimeText[i] = TimeSet;
			}
		}
		TimeRefresh(SR.ClearTimeNowGame);
		TotalTimeText = TimeSet;
		if (SR.ClearTimeNowGame > SR.ClearTimeHighscore[SR.CurrentStageID]){
			SR.ClearTimeHighscore[SR.CurrentStageID] = SR.ClearTimeNowGame;
		}
		TimeScoreCheck(SR.ClearTimeNowGame);
		TotalScoreCheck();
		OptionBonusCheck();
		if (SR.TotalScore <= 0){
			SR.TotalScore = 0;
		}else if(SR.TotalScore > SR.TotalHighscore[SR.CurrentStageID]){
			SR.TotalHighscore[SR.CurrentStageID] = SR.TotalScore;
		}
		TotalScoreText = SR.TotalScore.ToString();
		CasheMath();
		GettedCasheText = GetCashe.ToString();
		SR.HaveCashes += GetCashe;
	}
	
	void TotalScoreCheck(){
		SR.TotalScore += SR.MainScore;
		SR.TotalScore += SR.NoDamageBonus;
		SR.TotalScore += SR.ObjectDestructionBonus;
		SR.TotalScore += SR.EntrainmentEnemyKillBonus;
	}
	
	void OptionBonusCheck(){
		if (Opt.ItemOn == false){
			SR.TotalScore += 2000;
		}
		switch(Opt.GameLevel){
		case 1:
			SR.TotalScore += 0;
			break;
		case 2:
			SR.TotalScore += 5000;
			break;
		case 3:
			SR.TotalScore += 10000;
			break;
		}
	}
	
	void TimeScoreCheck(float ETime){
		if ((int)ETime < 3600){
			SR.TotalScore += 500;
		}
		if ((int)ETime < 1800){
			SR.TotalScore += 1000;
		}
		if ((int)ETime < 900){
			SR.TotalScore += 2000;
		}
		if ((int)ETime < 600){
			SR.TotalScore += 3000;
		}
		if ((int)ETime < 300){
			SR.TotalScore += 5000;
		}
		if ((int)ETime < 180){
			SR.TotalScore += 10000;	
		}
		if ((int)ETime < 60){
			SR.TotalScore += 50000;	
		}
		if ((int)ETime < 30){
			SR.TotalScore += 100000;
		}
		if ((int)ETime < 10){
			SR.TotalScore += 500000;
		}
		if ((int)ETime < 5){
			SR.TotalScore += 1000000;
		}
		if ((int)ETime < 1){
			SR.TotalScore += 10000000;
		}
		if ((int)ETime <= 0){
			SR.TotalScore = 0;
		}
	}

	void CasheMath(){
		GetCashe = SR.TotalScore / 100;
		if (GetCashe > 99999999){

		}
	}
	
	void TimeRefresh(float ETime){
		int s = 0,m = 0,h = 0,s2 = 0,s2Leng = 2;
		string sZero,mZero,hZero,s2Zero,s2HeadCut;
		m = (int)ETime / 60;
		s = (int)ETime % 60;
		h = m / 60;
		m = m % 60;
		s2 = (int)Mathf.Round(ETime * 100f);
		
		
		if (s2 < 10){
			s2Zero = "0" + s2.ToString();
		}else{
			s2Leng = s2.ToString().Length;
			s2HeadCut = s2.ToString().Substring(s2Leng - 2,2);
			s2Zero = s2HeadCut;
		}
		if (s < 10){
			sZero = "0" + s.ToString();
		}else{
			sZero = s.ToString();
		}
		if (m < 10){
			mZero = "0" + m.ToString();
		}else{
			mZero = m.ToString();
		}
		if (h < 10){
			hZero = "0" + h.ToString();
		}else{
			hZero = h.ToString();
		}
		
		TimeSet = hZero + ":" + mZero + ":" + sZero + ":" + s2Zero;
	}
	
	void OnGUI(){
		if (GUIEnabled == true){
			ResultSetting();
			ScrollAreaScale.x = 300 + 130 + 200;
			Rect ScrArea = new Rect(Screen.width*GUISide, Screen.height*GUITop, (Screen.width - ((Screen.width*GUISide)*2)) ,Screen.height - (Screen.height*GUITop + Screen.height*GUIBottom));
			GUILayout.BeginArea(ScrArea);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,Style,GUILayout.MaxWidth(Screen.width - ((Screen.width*GUISide)*2)),GUILayout.MaxHeight(Screen.height - (Screen.height*GUITop + Screen.height*GUIBottom)));
			GUILayout.BeginVertical();
			//GUILayout.FlexibleSpace();
			GUILayout.Label ("Stage Name : " + Datas.ItemName[SR.CurrentStageID],Style,GUILayout.Width(300));
			//GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			//GUILayout.FlexibleSpace();
			GUILayout.Label ("Total Clear Time : " + TotalTimeText,Style,GUILayout.Width(130));
			//GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			
			ScrollAreaScale.y = 0;
			ScrollAreaScale.y = Style.fontSize*10 + 10;
			
			for (int i = 0;i < Missions;i++){
				//GUILayout.FlexibleSpace();
				GUILayout.Label ("Mission " + (i + 1).ToString() + " Clear Time : " + ClearTimeText[i],Style,GUILayout.Width(130));
				//GUILayout.FlexibleSpace();
				GUILayout.Space(10);
				ScrollAreaScale.y += GraphFontSize * Style.fontSize + 10;
			}
			
			//GUILayout.FlexibleSpace();
			GUILayout.Label ("Total Score : " + TotalScoreText,Style,GUILayout.Width(200));
			//GUILayout.FlexibleSpace();
			GUILayout.Space(10);

			GUILayout.Label ("Prize Cashe : " + GettedCasheText,Style,GUILayout.Width(200));

			GUILayout.Space(10);

			for (int i = 0;i < SR.ClearTrophys.Count ;i++){
				if (TDatas.TrophyClear[i] == false){
					if (TDatas.TrophyEnable[i] == false){
						TDatas.TrophyEnable[(int)SR.ClearTrophys[i]] = true;
					}
					GUILayout.Label (TrophyGetTexts[0] + TDatas.TrophyName[(int)SR.ClearTrophys[i]] + TrophyGetTexts[1],Style2,GUILayout.Width(ScrollAreaScale.y));
					TDatas.TrophyGetted((int)SR.ClearTrophys[i]);
				}
			}

			GUILayout.Space(15);
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}	
	}
	
	void Controll(){
		
		if (Input.GetButton(Command.DPadDownCommand)){
			if (Waits <= 0f){
				scrollPosition += new Vector2(0f, ScrollSpeed);
				_audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
			}else{
				Waits -= Time.deltaTime;
			}
		}else if (Input.GetButton(Command.DPadUpCommand)){
			if (Waits <= 0f){
				scrollPosition -= new Vector2(0f, ScrollSpeed);
				_audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
				}else{
				Waits -= Time.deltaTime;
			}
		}
		
		if (Input.GetButtonDown(Command.EnterCommand) || Input.GetButtonDown(Command.CancelCommand)){
			Save.Save();
			WindowClose();
		}
		
	}
	
}
