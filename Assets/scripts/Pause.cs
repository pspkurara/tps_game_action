using UnityEngine;
using System.Collections;
//ポーズ画面のスクリプト.
public class Pause : MonoBehaviour {
	
	//決定、選択、キャンセルの効果音.
	public AudioClip EnterSound = null;
	public AudioClip SelectSound = null;
	public AudioClip BackSound = null;
	
	//FadeOutのオブジェクト.
	public GameObject PauseFadeOut = null;
	
	//FadeInFadeOutのスクリプト.
	private FadeInFadeOut FIFO = null;
	
	//ポーズされているかどうかを判断するフラグ.
	public bool PauseFlag = false;
	
	//ポーズ画面のGUI.
	public GUITexture PauseWall;
	public GUITexture PauseText;
	public GUITexture TextsBack;
	public GUITexture Text_Continue;
	public GUITexture Text_Quit;
	
	//効果音に使用するAudioSourceの宣言.
	private AudioSource _audios;
	
	//選択されている文字とそうでない文字の色を指定.
	private Color DisableColor = new Color (0.2f,0.2f,0.2f,1f);
	private Color EnableColor = Color.white;
	
	//ボタン操作のフラグ.
	public bool OKInput = true;
	
	//メニューの項目.
	enum MenuSelection{
		PContinue = 0,
		PQuit = 1,
	}
	
	//Commandファイル.
	private Commands Command;
	
	//MenuSelectionの最後の数.
	private MenuSelection EMax;
	//MenuSelectionの最初の数
	private MenuSelection EMin;
	//選択中の項目.
	private MenuSelection EnableSelect;
	//ゲーム中の処理を中断するための一時データ保存先.
	private GameTimeStamp _GameTimeStamp;
	
	//BGM関連.
	private BGMPlay BGMCtrl;
	
	//ShootControllの呼び出し.
	private ShootControl SC;
	
	void Start () {
		//MenuSelectionの最大数を代入.
		EMin = MenuSelection.PContinue;
		//MenuSelectionの最小数を代入.
		EMax = MenuSelection.PQuit;
		//効果音に使うためのメインカメラのAundioSourceを呼び出す.
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		//選択肢を初期化.
		EnableSelect = MenuSelection.PContinue;
		FIFO = PauseFadeOut.GetComponent<FadeInFadeOut>();
		
		//経過時間を保存する.
		_GameTimeStamp = GameObject.Find("GameTimeStamps").GetComponent<GameTimeStamp>();
		
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		
		if (GameObject.Find("Player")){
			SC = GameObject.Find("Player").GetComponent<ShootControl>();
		}
		
		//BGM読み込み.
		BGMCtrl = GameObject.Find("BGM").GetComponent<BGMPlay>();
		
		//メニューのGUIを全て非表示にする.
		PauseWall.enabled = false;
		PauseText.enabled = false;
		TextsBack.enabled = false;
		Text_Continue.enabled = false;
		Text_Quit.enabled = false;
	}
	
	void Update () {
		if (SC == null){
			if (GameObject.Find("Player")){
				SC = GameObject.Find("Player").GetComponent<ShootControl>();
				if (PauseFlag == true){
					SC.isControl = false;
				}else{
					SC.isControl = true;
				}
			}
		}
		//ポーズ画面非表示時の処理.
		if (PauseFlag == false && OKInput == true){
			//ポーズボタンが押されたらポーズメニューを表示.
			if (Input.GetButtonDown(Command.PauseCommand)){
				_audios.PlayOneShot(EnterSound);
				MenuOn();
			}
		//ポーズ画面表示中の処理.
		}else{
			//DigitalUpを押したら上向きにカーソルが進む(上まで来たら下にループ).
			if (Input.GetButtonDown(Command.DPadUpCommand) && OKInput == true){
				_audios.PlayOneShot(SelectSound);
				if (EMax <= EnableSelect){
				EnableSelect = EMin;
				}else{
				EnableSelect ++;
				}
			//DigitalDownを押したら下向きにカーソルが進む(下まで来たら上にループ).
			}else if (Input.GetButtonDown(Command.DPadDownCommand) && OKInput == true){
				_audios.PlayOneShot(SelectSound);
				if (EMin >= EnableSelect){
				EnableSelect = EMax;
				}else{
				EnableSelect --;
				}
			}
			//選択中の項目をハイライトし、他を暗くする.
			if (EnableSelect == MenuSelection.PContinue){
				Text_Continue.color = EnableColor;
				Text_Quit.color = DisableColor;
				EnableSelect = MenuSelection.PContinue;
			}else if (EnableSelect == MenuSelection.PQuit){
				Text_Continue.color = DisableColor;
				Text_Quit.color = EnableColor;
				EnableSelect = MenuSelection.PQuit;
			}
			//決定された項目にジャンプする　選択肢によって行く先が変わる.
			if (Input.GetButtonDown(Command.EnterCommand)){
				switch(EnableSelect){
					case MenuSelection.PContinue:
						_audios.PlayOneShot(EnterSound);
						MenuBack();
						break;
					case MenuSelection.PQuit:
						_audios.PlayOneShot(EnterSound);
						FIFO.StartFlag = true;
						_GameTimeStamp.SpritTimeStamp();
						OKInput = false;
						MenuBack();
						PauseFlag = true;
						BGMCtrl.FadeOutBG();
						break;				
				}
			}
			//ポーズ中に再度ポーズボタンを押した場合、ポーズ画面を抜け出す.
			if (Input.GetButtonDown(Command.PauseCommand) && OKInput == true || Input.GetButtonDown(Command.CancelCommand) && OKInput == true){
				_audios.PlayOneShot(BackSound);
				MenuBack();
			}
			
			//FadeOutが終了したら、シーンに移動.
			if (FIFO.EndFlag == true){
				Esel (EnableSelect);
			}
		}
	}
	//ポーズメニューを閉じる.
	void MenuBack(){
		if (SC != null){
			SC.isControl = true;
		}
		BGMCtrl.VolumeFlag = 0;
		_GameTimeStamp.StartTimeStamp();
		PauseWall.enabled = false;
		PauseText.enabled = false;
		TextsBack.enabled = false;
		Text_Continue.enabled = false;
		Text_Quit.enabled = false;
		PauseFlag = false;
	}
	//ポーズメニューを呼び出す.
	void MenuOn(){
		if (SC != null){
			SC.isControl = false;
		}
		BGMCtrl.VolumeFlag = 1;
		_GameTimeStamp.StopTimeStamp();
		PauseWall.enabled = true;
		PauseText.enabled = true;
		TextsBack.enabled = true;
		Text_Continue.enabled = true;
		Text_Quit.enabled = true;
		PauseFlag = true;
	}
	
	void Esel(MenuSelection GoNextScene){
		switch(GoNextScene){
			case MenuSelection.PContinue:
				break;
			case MenuSelection.PQuit:
				Application.LoadLevel ("StageSelect");
				_GameTimeStamp.StartTimeStamp();
				break;				
		}
	}
	
}
