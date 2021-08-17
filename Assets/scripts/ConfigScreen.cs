using UnityEngine;
using System.Collections;

public class ConfigScreen : MonoBehaviour {

	//public Texture backgroundImage;
	public AudioClip EnterSound;
	public AudioClip BackSound;
	public AudioClip SelectSound;
	public AudioClip ErrorSound;
	
	public float BGMVolumeSetting = 1f;
	private float SEVolumeSetting = 1f;
	private float BGMVolumeLevelString = 1f;
	private float SEVolumeLevelString = 1f;
	private string[] GameLevelSettingString = {"Lv.1","Lv.2","Lv.3"};
	private int GameLevelSetting = 0;
	private bool ItemOn = true;
	private string ItemOnString = "ON";
	private string ItemOnStringOn = "ON", ItemOnStringOff = "OFF";
	
	private OptionSetting Option;
	private SaveScript Save;
	private FadeInFadeOut FadeOut, FadeIn;
	private bool OKInput = true;
	private AudioSource Audios;
	private OptionBGM BGM;
	public bool GUIEnabled = true;
	public GUIStyle Style;
	private Commands Command;
	public bool OnControll = true;
	private int FIFOSwitch;
	
	bool[] NowSelectOptionH = {false,false,false,false,false,false};
	public int CurrentSelectOptionH = 0;
	public float VolumeChangeWaitTime = 1f;
	float Waits;
	
	public GUITexture MainWindow;
	
	void Awake () {
		Option = GameObject.Find("Settings").GetComponent<OptionSetting>();
		FadeIn = GameObject.Find("FadeIn").GetComponent<FadeInFadeOut>();
		FadeOut = GameObject.Find("FadeOut").GetComponent<FadeInFadeOut>();
		Audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		BGM = GameObject.Find("BGM").GetComponent<OptionBGM>();
		Save = GetComponent<SaveScript>();
	}
	
	void Start(){
		GUIEnabled = false;
		Save.Load ();
		LoadOpen();
		Audios.volume = Option.SEVolume;
		BGM.BGMPlaying();
		Waits = VolumeChangeWaitTime;
	}
	
	
	
	
	void OnGUI () {
		if (GUIEnabled == true){
			CurrentSelection();
			GUILayout.BeginArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10));
			GUILayout.BeginVertical();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Option & Settings",Style);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[0];
					GUILayout.Label ("BGM Volume",Style,GUILayout.Width(100));
					GUILayout.Space(10);
					GUILayout.Label (((int)BGMVolumeLevelString).ToString() + "%",Style,GUILayout.Width(80));
					GUILayout.Space(10);
					BGMVolumeSetting = GUILayout.HorizontalSlider (BGMVolumeSetting, 0, 1,GUILayout.MinWidth(300));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[1];
					GUILayout.Label ("SE Volume",Style,GUILayout.Width(100));
					GUILayout.Space(10);
					GUILayout.Label (((int)SEVolumeLevelString).ToString() + "%",Style,GUILayout.Width(80));
					GUILayout.Space(10);
					SEVolumeSetting = GUILayout.HorizontalSlider (SEVolumeSetting, 0, 1,GUILayout.MinWidth(300));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[2];	
					GUILayout.Label ("Game Level",Style,GUILayout.Width(130));
					GUILayout.Space(10);
					GameLevelSetting = GUILayout.Toolbar (GameLevelSetting, GameLevelSettingString,Style,GUILayout.Width(300));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
	
				GUILayout.FlexibleSpace();
			
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[3];
					GUILayout.Label ("Appearance Items",Style,GUILayout.Width(130));
					GUILayout.Space(10);
					GUILayout.Label (ItemOnString,Style,GUILayout.Width(300));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				GUILayout.FlexibleSpace();
				
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUI.enabled = NowSelectOptionH[4];
					GUILayout.Label ("Save & Close",Style);
					GUILayout.Space(20);
					GUI.enabled = NowSelectOptionH[5];
					GUILayout.Label ("Cancel",Style);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
		
				GUILayout.FlexibleSpace();
			
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	
	void SaveClose(){
		Option.BGMVolume = BGMVolumeSetting;
		Option.SEVolume = SEVolumeSetting;
		Option.GameLevel = GameLevelSetting;
		Option.ItemOn = ItemOn;
		Save.Save();
	}
	
	void LoadOpen(){
		BGMVolumeSetting = Option.BGMVolume;
		SEVolumeSetting = Option.SEVolume;
		GameLevelSetting = Option.GameLevel;
		ItemOn = Option.ItemOn;
	}
	
	void Update (){
		switch(FIFOSwitch){
		case 0:
			if (FadeIn.EndFlag == true){
				MainWindow.enabled = true;
				GUIEnabled = true;
				FIFOSwitch = 2;
				OnControll = true;
			}
			break;
		case 1:
			if (FadeOut.StartFlag == true){
				OnControll = false;
			}
			if (FadeOut.EndFlag == true){
				Application.LoadLevel("Title");
			}
			break;
		case 2:
			BGMVolumeLevelString = BGMVolumeSetting * 100f;
			SEVolumeLevelString = SEVolumeSetting * 100f;
			Audios.volume = SEVolumeSetting;
			Controll();
			break;
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
			CurrentSelectOptionH = NowSelectOptionH.Length - 1;
		}else if (CurrentSelectOptionH > NowSelectOptionH.Length - 1){
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
			VolumeChanger();
			NextSelect();
			break;
		case 1:
			VolumeChanger();
			NextSelect();
			break;
		case 2:
			ChangeLevel();
			NextSelect();
			break;
		case 3:
			ItemEnabled();
			NextSelect();
			break;
		case 4:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true)
			{
				Audios.PlayOneShot(EnterSound);
				BGM.FadeOutBG();
				SaveClose();
				FadeOut.StartFlag = true;
				FIFOSwitch = 1;
				GUIEnabled = false;
			}
			break;
		case 5:
			if(Input.GetButtonDown(Command.EnterCommand) && OKInput == true)
			{
				Audios.PlayOneShot(BackSound);
				BGM.FadeOutBG();
				FadeOut.StartFlag = true;
				FIFOSwitch = 1;
				GUIEnabled = false;
			}
			break;
		}
		
		if(Input.GetButtonDown(Command.CancelCommand) && OKInput == true)
		{
			Audios.PlayOneShot(BackSound);
			BGM.FadeOutBG();
			FadeOut.StartFlag = true;
			FIFOSwitch = 1;
			GUIEnabled = false;
		}
		
	}
	
	void ItemEnabled(){
		if (Input.GetButtonDown(Command.DPadLeftCommand) && OnControll == true || Input.GetButtonDown(Command.DPadRightCommand) && OnControll == true){
			Audios.PlayOneShot(SelectSound);
			switch(ItemOn){
				case true:
					ItemOnString = ItemOnStringOff;
					ItemOn = false;
					break;
				case false:
					ItemOnString = ItemOnStringOn;
					ItemOn = true;
					break;
			}
		}
	}
	
	void ChangeLevel(){
		if (GameLevelSetting < 0){
			GameLevelSetting = GameLevelSettingString.Length - 1;
		}else if (GameLevelSetting > GameLevelSettingString.Length - 1){
			GameLevelSetting = 0;
		}else{
			if (Input.GetButtonDown(Command.DPadLeftCommand) && OnControll == true){
				GameLevelSetting --;
				Audios.PlayOneShot(SelectSound);
			}else if (Input.GetButtonDown(Command.DPadRightCommand) && OnControll == true){
				GameLevelSetting ++;
				Audios.PlayOneShot(SelectSound);
			}
		}
	}
	
	void NextSelect(){
		if (CurrentSelectOptionH > NowSelectOptionH.Length - 1){
			CurrentSelectOptionH = 0;
		}else{
			if (Input.GetButtonDown(Command.EnterCommand) && OnControll == true){
				CurrentSelectOptionH ++;
				Audios.PlayOneShot(EnterSound);
			}
		}
	}
	
	void VolumeChanger(){
		if (Input.GetButton(Command.DPadLeftCommand) && OnControll == true){
			if (Waits <= 0f){
				switch(CurrentSelectOptionH){
				case 0:
					BGMVolumeSetting -= 0.01f;
					if (BGMVolumeSetting < 0f){
						Audios.PlayOneShot(ErrorSound);
					}else{
						Audios.PlayOneShot(SelectSound);
					}
					BGMVolumeSetting = Mathf.Clamp(BGMVolumeSetting,0f,1f);
					break;
				case 1:
					SEVolumeSetting -= 0.01f;
					if (BGMVolumeSetting < 0f){
						Audios.PlayOneShot(ErrorSound);
					}else{
						Audios.PlayOneShot(SelectSound);
					}
					SEVolumeSetting = Mathf.Clamp(SEVolumeSetting,0f,1f);
					break;
				}
				Waits = VolumeChangeWaitTime;
			}else{
				Waits -= Time.deltaTime;
			}
		}else if (Input.GetButton(Command.DPadRightCommand) && OnControll == true){
			if (Waits <= 0f){
				switch(CurrentSelectOptionH){
				case 0:
					BGMVolumeSetting += 0.01f;
					if (BGMVolumeSetting > 1f){
						Audios.PlayOneShot(ErrorSound);
					}else{
						Audios.PlayOneShot(SelectSound);
					}
					BGMVolumeSetting = Mathf.Clamp(BGMVolumeSetting,0f,1f);
					break;
				case 1:
					SEVolumeSetting += 0.01f;
					if (SEVolumeSetting > 1f){
						Audios.PlayOneShot(ErrorSound);
					}else{
						Audios.PlayOneShot(SelectSound);
					}
					SEVolumeSetting = Mathf.Clamp(SEVolumeSetting,0f,1f);
					break;
				}
				Waits = VolumeChangeWaitTime;
			}else{
				Waits -= Time.deltaTime;
			}
		}
		
	}

}
