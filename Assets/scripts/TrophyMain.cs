using UnityEngine;
using System.Collections;

public class TrophyMain : MonoBehaviour {

	public AudioClip EnterSound, BackSound, SelectSound, ErrorSound;
	public GUIText HighScoreTex;
	private FadeInFadeOut FadeIn, FadeOut;
	private AudioSource Audios;
	private int WindowLevel = 0;
	public GameObject Wind = null;
	private float MaxWidth = 1f;
	public float OpenSpeed = 0.5f;
	private GUITexture WindTex;
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
	public int CCize = 10;
	public int TNameSize = 150;
	public int LegSize = 500;
	public int PrizeSize = 300;
	public int SpaceSize = 10;
	
	private TrophyDatas Datas;

	void Start () {
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		BGM = GetComponent<BGMPlay>();
		FadeIn = GameObject.Find("FadeInObj").GetComponent<FadeInFadeOut>();
		FadeOut = GameObject.Find("FadeOutObj").GetComponent<FadeInFadeOut>();
		Audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Datas = GameObject.Find("Scores").GetComponent<TrophyDatas>();
		WindTex = Wind.GetComponent<GUITexture>();
		WindTex.enabled = false;
		EnableTexts(false);
		Wind.transform.localScale = new Vector3(0f,Wind.transform.localScale.y,Wind.transform.localScale.z);
		FadeIn.StartFlag = true;
		Waits = ScrollWait;
		BGM.BGMPlaying();
	}

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
		if (GUIEnabled == true){
			ScrollAreaScale.x = CCize + TNameSize + LegSize + PrizeSize;
			Rect ScrArea = new Rect(0 + (Screen.width / GUISide), 0 + (Screen.height / GUITop), (Screen.width - (Screen.width / GUISide)*2) ,Screen.height - ((Screen.height / GUITop) + (Screen.height / GUIBottom)));
			GUILayout.BeginArea(ScrArea);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,Style,GUILayout.MaxWidth(Screen.width - (Screen.width / GUISide)*2),GUILayout.MaxHeight(Screen.height - ((Screen.height / GUITop) + (Screen.height / GUIBottom))));
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label ("C",Style,GUILayout.Width(CCize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Trophy Name",Style,GUILayout.Width(TNameSize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Legend",Style,GUILayout.Width(LegSize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Prize",Style,GUILayout.Width(PrizeSize));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(SpaceSize);
			ScrollAreaScale.y = 0;
			ScrollAreaScale.y = Style.fontSize*10 + SpaceSize;
			for(int i= 0;i < Datas.TrophyEnable.Length ;i++){
				ScoreLoad(i);
				ScrollAreaScale.y += GraphFontSize * Style.fontSize + SpaceSize;
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
	
	void EnableTexts(bool Enable){
		HighScoreTex.enabled = Enable;
		GUIEnabled = Enable;
	}
	
	void ScoreLoad(int index){
		GUI.enabled = Datas.TrophyEnable[index];
		string Check;
		string TName;
		string TLeg;
		string Prize;

		if (Datas.TrophyEnable[index] == false){
			Check = "X";
			TName = "???";
			TLeg = "??????";
			Prize = "???";
		}else{
			if (Datas.TrophyClear[index] == true){
				Check = "O";
			}else{
				Check = "X";
			}
			TName = Datas.TrophyName[index];
			TLeg = Datas.TrophyLegend[index];
			Prize = Datas.TrophyPrize[index];
		}

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label (Check,Style,GUILayout.Width(CCize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label (TName,Style,GUILayout.Width(TNameSize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label (TLeg,Style,GUILayout.Width(LegSize));
			GUILayout.FlexibleSpace();
			GUILayout.Space(SpaceSize);
			GUILayout.FlexibleSpace();
			GUILayout.Label (Prize,Style,GUILayout.Width(PrizeSize));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(SpaceSize);

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
}
