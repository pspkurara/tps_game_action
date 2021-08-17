using UnityEngine;
using System.Collections;

public class RewordSelect : MonoBehaviour {
	
	public AudioClip EnterSound;
	public AudioClip BackSound;
	public AudioClip SelectSound;
	
	public GUITexture MainWindow;
	
	private OptionSetting Option;
	private SaveScript Save;
	private FadeInFadeOut FadeOut, FadeIn;
	private bool OKInput = true;
	private AudioSource Audios;
	private BGMPlay BGM;
	public bool GUIEnabled = true;
	public GUIStyle Style;
	private Commands Command;
	public bool OnControll = true;
	
	bool[] NowSelectOptionH = {false,false,false,false,false,false};
	public int CurrentSelectOptionH = 0;
	private int FIFOSwitch;
	private string LoadLevel;
	public float ButtonWidth = 0.3f;
	public float TitleWidth = 0.4f;
	public float StrecheSpeed = 1f;
	Vector3 SaveWidth;
	float Times = 0f;
	
	void Start(){
		Option = GameObject.Find("Settings").GetComponent<OptionSetting>();
		FadeOut = GameObject.Find("FadeOut").GetComponent<FadeInFadeOut>();
		FadeIn = GameObject.Find("FadeIn").GetComponent<FadeInFadeOut>();
		Audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		BGM = GameObject.Find("BGM").GetComponent<BGMPlay>();
		Save = GetComponent<SaveScript>();
		Save.Load ();
		Audios.volume = Option.SEVolume;
		BGM.BGMPlaying();
		GUIEnabled = false;
		OnControll = false;
		SaveWidth = MainWindow.gameObject.transform.localScale;
		MainWindow.gameObject.transform.localScale = new Vector3(0f,MainWindow.gameObject.transform.localScale.y,MainWindow.gameObject.transform.localScale.z);
		FIFOSwitch = 0;
	}
	
	void Update (){
		switch(FIFOSwitch){
		case 0:
			if (FadeIn.EndFlag == true){
				MainWindow.enabled = true;
				Times = 0f;
				MainWindow.gameObject.transform.localScale = new Vector3(0f,MainWindow.gameObject.transform.localScale.y,MainWindow.gameObject.transform.localScale.z);
				FIFOSwitch = 3;
				Audios.PlayOneShot(EnterSound);
			}
			break;
		case 1:
			if (FadeOut.EndFlag == true){
				Application.LoadLevel(LoadLevel);
			}
			break;
		case 2:
			Controll();
			break;
		case 3:
			if (0.001f <= Times){
				if (SaveWidth.x <= MainWindow.gameObject.transform.localScale.x){
					MainWindow.gameObject.transform.localScale = new Vector3(SaveWidth.x,MainWindow.gameObject.transform.localScale.y,MainWindow.gameObject.transform.localScale.z);
					FIFOSwitch = 2;
					GUIEnabled = true;
					OnControll = true;
				}else{
					MainWindow.gameObject.transform.localScale += new Vector3(SaveWidth.x/StrecheSpeed,0f,0f);
				}
				Times = 0f;
			}else{
				Times+=Time.deltaTime;
			}
			break;
		case 4:
			if (0.001f <= Times){
				if (0f >= MainWindow.gameObject.transform.localScale.x){
					MainWindow.gameObject.transform.localScale = new Vector3(0f,MainWindow.gameObject.transform.localScale.y,MainWindow.gameObject.transform.localScale.z);
					FIFOSwitch = 1;
				}else{
					MainWindow.gameObject.transform.localScale -= new Vector3(SaveWidth.x/StrecheSpeed,0f,0f);
				}
				Times = 0f;
			}else{
				Times+=Time.deltaTime;
			}
			break;
		}
		
	}


	
	void OnGUI () {
		if (GUIEnabled == true){
			CurrentSelection();
			GUILayout.BeginArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10));
			GUILayout.BeginVertical();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Rewords",Style,GUILayout.Width(Screen.width*TitleWidth));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[0];
				GUILayout.Label ("High Score Check",Style,GUILayout.Width(Screen.width*ButtonWidth));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[1];
				GUILayout.Label ("Costume Select",Style,GUILayout.Width(Screen.width*ButtonWidth));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[2];
				GUILayout.Label ("Trophy",Style,GUILayout.Width(Screen.width*ButtonWidth));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.FlexibleSpace();
				
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[3];
				GUILayout.Label ("Juke Box",Style,GUILayout.Width(Screen.width*ButtonWidth));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
	
				GUILayout.FlexibleSpace();
				
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[4];
				GUILayout.Label ("Back",Style,GUILayout.Width(Screen.width*ButtonWidth));
						GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
		
				GUILayout.FlexibleSpace();
			
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	
	void CurrentSelection(){
		for(int i= 0;i < NowSelectOptionH.Length ;i++){
			NowSelectOptionH[i] = false;
		}
		NowSelectOptionH[Mathf.Clamp(CurrentSelectOptionH,0,NowSelectOptionH.Length - 1)] = true;
	}
	
	void Controll(){
		if (CurrentSelectOptionH < 0){
			CurrentSelectOptionH = NowSelectOptionH.Length - 2;
		}else if (CurrentSelectOptionH > NowSelectOptionH.Length - 2){
			CurrentSelectOptionH = 0;
		}else{
			if (Input.GetButtonDown(Command.DPadDownCommand) && OnControll == true){
				CurrentSelectOptionH ++;
				Audios.PlayOneShot(SelectSound);
			}else if (Input.GetButtonDown(Command.DPadUpCommand) && OnControll == true){
				CurrentSelectOptionH --;
				Audios.PlayOneShot(SelectSound);
			}
		}
		
		switch(CurrentSelectOptionH){
		case 0:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true){
				Selections("HighScore",true);
			}
			break;
		case 1:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true){
				Selections("CostumeSelect",true);
			}
			break;
		case 2:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true){
				Selections("Trophy",true);
			}
			break;
		case 3:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true){
				Selections("JukeBox",true);
			}
			break;
		case 4:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true){
				Selections("Title",true);
			}
			break;
		}
		
		if(Input.GetButtonDown(Command.CancelCommand) && OKInput == true){
			Selections("Title",false);
		}
		
	}
	
	void Selections(string Apptitle,bool EnterBack){
		switch(EnterBack){
		case true:
			Audios.PlayOneShot(EnterSound);
			break;
		case false:
			Audios.PlayOneShot(BackSound);
			break;
		}
		BGM.FadeOutBG();
		Save.Save();
		FadeOut.StartFlag = true;
		GUIEnabled = false;
		OnControll = false;
		LoadLevel = Apptitle;
		FIFOSwitch = 4;
	}
	
}
