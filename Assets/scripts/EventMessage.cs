using UnityEngine;
using System.Collections;

public class EventMessage : MonoBehaviour {
	
	public AudioClip EnterSound = null;
	public AudioClip BackSound = null;
	
	public GUITexture BackFill = null;
	public GameObject BackWindow = null;
	
	public float OpenSpeed = 0.5f;
	private bool OnEventMessage = false;
	public float MaxWidth = 0.8f;
	public GameObject Texts = null;
	private bool OKInput = true;
	enum EventWindowMode{
		Closed = 0,
		Opened = 1,
		Closing = 2,
		Opening = 3,
	}
	private  EventWindowMode NowEventWindow;
	private GUIText TextsGUI;
	private Commands Command;
	private GameTimeStamp _GameTimeStamp;
	public bool MissionStartFlag = false;
	
	private AudioSource _audios;
	
	private Pause PFlag;
	private BGMPlay BGM;
	
	public string EventText;
	
	public bool GUIEnabled = false;
	
	public GUIStyle[] Style;
	
	public float TextAreaX, TextAreaY, FrameWidth, FrameHeight;
	
	float w;
	float h;
	
	Rect Pos;
	
	
	// Use this for initialization
	void Start () {
		BackFill.enabled = false;
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		NowEventWindow = EventWindowMode.Closed;
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		_GameTimeStamp = GameObject.Find("GameTimeStamps").GetComponent<GameTimeStamp>();
		TextsGUI = Texts.GetComponent<GUIText>();
		BackWindow.transform.localScale = new Vector3(0f,BackWindow.transform.localScale.y,BackWindow.transform.localScale.z);
		PFlag = GameObject.Find("PauseScript").GetComponent<Pause>();
		BGM = GameObject.Find("BGM").GetComponent<BGMPlay>();
		GUIEnabled = false;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		switch(NowEventWindow){
		case EventWindowMode.Closed:
			TextsGUI.enabled = false;
			MissionStartFlag = true;
			break;
		case EventWindowMode.Opened:
			TextsGUI.enabled = true;
			break;
		case EventWindowMode.Closing:
			if (BackWindow.transform.localScale.x <= 0f){
				OKInput = true;
				OnEventMessage = false;
				BackWindow.transform.localScale = new Vector3(0f,BackWindow.transform.localScale.y,BackWindow.transform.localScale.z);
				BackWindow.guiTexture.enabled = false;
				BackFill.enabled = false;
				_GameTimeStamp.StartTimeStamp();
				BGM.VolumeFlag = 0;
				NowEventWindow = EventWindowMode.Closed;
			}else{
				OKInput = false;
				BackWindow.transform.localScale = BackWindow.transform.localScale - new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case EventWindowMode.Opening:
			if (BackWindow.transform.localScale.x >= MaxWidth){
				//print ("Opening");
				OKInput = true;
				OnEventMessage = true;
				BackWindow.transform.localScale = new Vector3(MaxWidth,BackWindow.transform.localScale.y,BackWindow.transform.localScale.z);
				NowEventWindow = EventWindowMode.Opened;
			}else{
				BGM.VolumeFlag = 1;
				_GameTimeStamp.StopTimeStamp();
				OKInput = false;
				BackWindow.guiTexture.enabled = true;
				BackFill.enabled = true;
				BackWindow.transform.localScale = BackWindow.transform.localScale + new Vector3(OpenSpeed,0f,0f);
			}
			break;
		}
		if (Input.GetButtonDown(Command.EnterCommand) && OKInput == true || Input.GetButtonDown(Command.CancelCommand) && OKInput == true){
			if (OnEventMessage == true){
				//_audios.PlayOneShot(BackSound);
				WindowClose();
			}
		}if (Input.GetButtonDown(Command.EventCheckCommand) && OKInput == true && PFlag.PauseFlag == false){
			if (OnEventMessage == false){
				WindowOpen();
			}else if (OnEventMessage == true){
				//_audios.PlayOneShot(BackSound);
				WindowClose();
			}
		}
		
	}
	
	public void WindowOpen(){
		PFlag.OKInput = false;
		_audios.PlayOneShot(EnterSound);
		TextsGUI.text = EventText;
		NowEventWindow = EventWindowMode.Opening;
	}
	
	public void WindowClose(){
		PFlag.OKInput = true;
		TextsGUI.enabled = false;
		_audios.PlayOneShot(BackSound);
		NowEventWindow = EventWindowMode.Closing;
	}
	
	void PosSet(){
		w = Screen.width*FrameWidth;
		h = Screen.height*FrameHeight;
		Pos = new Rect (Screen.width-(Screen.width*TextAreaX),Screen.height-(Screen.height*TextAreaY),w,h);
	}
	
	void OnGUI(){
		if (GUIEnabled == true){
			GUI.Label (Pos,EventText,Style[0]);
		}	
	}
}
