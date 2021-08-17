using UnityEngine;
using System.Collections;

public class ResultMain : MonoBehaviour {
	
	public AudioClip EnterSound, BackSound, SelectSound, ErrorSound;
	public GUIText HighScoreTex;
	private FadeInFadeOut FadeIn, FadeOut;
	private AudioSource Audios;
	private int WindowLevel = 0;
	public GameObject Wind = null;
	private float MaxWidth = 1f;
	public float OpenSpeed = 0.5f;
	private GUITexture WindTex;
	private ScoreResult Score;
	private BGMPlay BGM;
	private string TimeSet;
	private Commands Command;
	public bool GUIEnabled = false;
	public float ScrollBarScale = 30f;
	public Vector2 ScrollAreaScale = new Vector2(Screen.width,50f);
	public float GUISide = 30f;
	public float GUITop = 30f;
	public float GUIBottom = 30f;
	public GUIStyle Style;
	public Vector2 scrollPosition = Vector2.zero;
	public float GraphFontSize = 10f;
	public float ScrollSpeed = 1f;
	public float ScrollWait = 0.5f;
	float Waits;
	
	private SlideListDatas Datas;

	
	// Use this for initialization
	void Start () {
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		BGM = GetComponent<BGMPlay>();
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		FadeIn = GameObject.Find("FadeInObj").GetComponent<FadeInFadeOut>();
		FadeOut = GameObject.Find("FadeOutObj").GetComponent<FadeInFadeOut>();
		Audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Datas = GameObject.Find("Scores").GetComponent<SlideListDatas>();
		WindTex = Wind.GetComponent<GUITexture>();
		WindTex.enabled = false;
		EnableTexts(false);
		Wind.transform.localScale = new Vector3(0f,Wind.transform.localScale.y,Wind.transform.localScale.z);
		FadeIn.StartFlag = true;
		Waits = ScrollWait;
		BGM.BGMPlaying();
	}
	
	// Update is called once per frame
	void Update () {
	switch(WindowLevel){
		case 0:
			if (FadeIn.EndFlag == true){
				WindowLevel = 1;
				//Audios.PlayOneShot(EnterSound);
				WindTex.enabled = true;
			}
			break;
		case 1:
			if (Wind.transform.localScale.x >= MaxWidth){
				Wind.transform.localScale = new Vector3(MaxWidth,Wind.transform.localScale.y,Wind.transform.localScale.z);
				WindowLevel = 2;
				EnableTexts (true);
			}else{
				Wind.transform.localScale = Wind.transform.localScale + new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case 2:
			Controll();
			break;
		case 3:
			if (Wind.transform.localScale.x <= 0f){
				Wind.transform.localScale = new Vector3(0f,Wind.transform.localScale.y,Wind.transform.localScale.z);
				WindowLevel = 4;
				WindTex.enabled = false;
				FadeOut.StartFlag = true;
			}else{
				Wind.transform.localScale = Wind.transform.localScale - new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case 4:
			if (FadeOut.EndFlag == true){
				Application.LoadLevel("Reword");
			}
			break;
		}
	}
	
	void OnGUI(){
		//ScrollAreaScale, ScrollAreaScale, Screen.width - (GUISide*2) - ScrollAreaScale*2, Screen.height - (GUIBottom + GUITop) - ScrollAreaScale*2
		if (GUIEnabled == true){
			//GUILayout.BeginArea(new Rect(GUISide, GUITop, Screen.width - (GUISide * 2), Screen.height - (GUIBottom + GUITop)));
			ScrollAreaScale.x = 300 + 130 + 200;
			Rect ScrArea = new Rect(0 + (Screen.width / GUISide), 0 + (Screen.height / GUITop), (Screen.width - (Screen.width / GUISide)*2) ,Screen.height - ((Screen.height / GUITop) + (Screen.height / GUIBottom)));
			//Rect ScrArea = new Rect(0, 0, ScrollAreaScale.x,ScrollAreaScale.y);
			GUILayout.BeginArea(ScrArea);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,Style,GUILayout.MaxWidth(Screen.width - (Screen.width / GUISide)*2),GUILayout.MaxHeight(Screen.height - ((Screen.height / GUITop) + (Screen.height / GUIBottom))));
			//scrollPosition = GUI.BeginScrollView(new Rect(ScrollBarScale + GUISide, ScrollBarScale + GUITop, Screen.width - (GUISide*2) - ScrollBarScale*2, Screen.height - (GUIBottom + GUITop) - ScrollBarScale*2), scrollPosition,ScrArea);
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Stage Name",Style,GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Clear Time",Style,GUILayout.Width(130));
			GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Total Score",Style,GUILayout.Width(200));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(15);
			ScrollAreaScale.y = 0;
			ScrollAreaScale.y = Style.fontSize*10 + 10;
			for(int i= 0;i < Datas.ItemID.Length ;i++){
				ScoreLoad(i);
				ScrollAreaScale.y += GraphFontSize * Style.fontSize + 10;
			}
			//GUILayout.Label (" ",Style);
			//GUILayout.Space(15);
			
			GUILayout.EndVertical();
			//GUI.EndScrollView();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
	
	void EnableTexts(bool Enable){
		HighScoreTex.enabled = Enable;
		GUIEnabled = Enable;
	}
	
	void ScoreLoad(int index){
		GUI.enabled = Datas.ItemEnabled[index];
			TimeRefresh(Score.ClearTimeHighscore[index]);
	
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label (Datas.ItemName[index],Style,GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			GUILayout.FlexibleSpace();
			GUILayout.Label (TimeSet,Style,GUILayout.Width(130));
			GUILayout.FlexibleSpace();
			GUILayout.Space(10);
			GUILayout.FlexibleSpace();
			GUILayout.Label (Score.TotalHighscore[index].ToString(),Style,GUILayout.Width(200));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
		//}
	}
	
	void Controll(){
		//print ((ScrollAreaScale.y - (GUITop + GUIBottom + (GraphFontSize * Style.fontSize + 10))).ToString());
		if (Input.GetButton(Command.DPadRightCommand)){
			if (Waits <= 0f){
				scrollPosition += new Vector2(ScrollSpeed, 0f);
				Audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
			}else{
				Waits -= Time.deltaTime;
			}
		}else if (Input.GetButton(Command.DPadLeftCommand)){
			if (Waits <= 0f){
				scrollPosition -= new Vector2(ScrollSpeed, 0f);
				Audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
			}else{
				Waits -= Time.deltaTime;
			}
		}
		
		if (Input.GetButton(Command.DPadDownCommand)){
			if (Waits <= 0f){
				scrollPosition += new Vector2(0f, ScrollSpeed);
				Audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
			}else{
				Waits -= Time.deltaTime;
			}
		}else if (Input.GetButton(Command.DPadUpCommand)){
			if (Waits <= 0f){
				scrollPosition -= new Vector2(0f, ScrollSpeed);
				Audios.PlayOneShot(SelectSound);
				Waits = ScrollWait;
				}else{
				Waits -= Time.deltaTime;
			}
		}
		
		if (Input.GetButtonDown(Command.EnterCommand)|| Input.GetButtonDown(Command.CancelCommand)){
			Audios.PlayOneShot(BackSound);
			EnableTexts (false);
			BGM.FadeOutBG();
			WindowLevel = 3;
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
}
