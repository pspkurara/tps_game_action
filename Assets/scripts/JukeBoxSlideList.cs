using UnityEngine;
using System.Collections;

public class JukeBoxSlideList : MonoBehaviour {
	
	float slide = 0f;
	public Texture2D CurrentSelectTex = null;
	public Texture2D ItemTex = null;
	private bool BackWindowText = false;
	public GUIStyle[] Style = {null,null};
	public Color CurrentSelectTexColor = Color.white;
	public Color DisabledColor = Color.gray;
	public int selectedItem = 0;
	float w;
	float h;
	public float rotAngle = 0f;
	Vector2 pivotPoint;
	public float ButtonWidth = 0.3f;
	public float ButtonHeight = 0.1f;
	public float FrameYPosition = 0.5f;
	public float FrameXPosition = 0.5f;
	public int MaxButtons = 1;
	public int Layer = 1;
	private GUITexture BackWindow;
	public GameObject WindowBack;
	float MaxPos;
	float MaxWidth;
	public bool InversMode = false;
	public bool GUIEnabled = true;
	public bool OnControll = true;
	public float OpenSpeed = 0.1f;
	public Color GuiColor, DisableColor;
	public Vector2 BackWindowSize = Vector2.zero;
	public Vector2 BackWindowPosition = Vector2.zero;
	public float LoopingTextInterval = 0.05f;
	string SonMessage;
	bool FirstCount = true;
	public AnimationClip LogoJumpAnim;
	private Animation LogoAnim;
	public float AnimationLoopInterval = 5f;

	private FadeInFadeOut FadeIn,FadeOut;

	enum EventWindowMode{
		ClosedStart = 0,
		Opened = 1,
		Closing = 2,
		Opening = 3,
		ClosedEnd = 4,
	}

	private EventWindowMode NowEventWindow;

	public AudioClip EnterSound;
	public AudioClip BackSound;
	public AudioClip SelectSound;
	public AudioClip ErrorSound;
	private AudioSource Asource, _audios;
	private OptionSetting Options;
	public float RollTime = 1f;
	public float RollWait = 0.3f;
	private bool NowPlayBGM = false;
	float Waits;
	
	float Roll;
	int Sw = 0;
	
	Commands Command;
	private JukeBoxSongDatabase Datas;
	public Transform VolumeBarTransform;
	public float VolumeChangeWaitTime = 1f;
	private Rect ExportRect = new Rect(0f,0f,0f,0f);
	GUIStyle StyleX;
	int TextLoopNumber = 0;
	float LogoJumpAnimWaitTime = 0f;
	
	void Start(){
		LogoAnim = GameObject.Find("JukeBoxLogo").GetComponent<Animation>();
		FadeIn = GameObject.Find("FadeIn").GetComponent<FadeInFadeOut>();
		FadeOut = GameObject.Find("FadeOut").GetComponent<FadeInFadeOut>();
		Datas = GetComponent<JukeBoxSongDatabase>();
		BackWindow = WindowBack.GetComponent<GUITexture>();
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		_audios = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Asource = GetComponent<AudioSource>();
		Asource.clip = Datas.JukeBoxAudioClip[0];
		if (Asource.playOnAwake == true){
			Asource.Stop();
		}
		Asource.loop = true;
		_audios.volume = Options.SEVolume;
		Asource.volume = Options.BGMVolume;
		FadeIn.StartFlag = true;
		GUIEnabled = false;
		OnControll = false;
		Roll = RollTime;
		NowPlayBGM = false;
		MaxWidth = WindowBack.transform.localScale.x;
		MaxButtons = Datas.JukeBoxAudioClip.Length;
		NowEventWindow = EventWindowMode.ClosedStart;
		BackWindow.enabled = false;
		BackWindowText = false;
		WindowBack.transform.localScale = new Vector3(0f,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
		Waits = VolumeChangeWaitTime;
		LogoJumpAnimWaitTime = AnimationLoopInterval;
	}
	
	void Update(){
		
		switch(NowEventWindow){
		case EventWindowMode.ClosedStart:
			if (FadeIn.EndFlag == true){
				_audios.PlayOneShot(EnterSound);
				BackWindow.enabled = true;
				NowEventWindow = EventWindowMode.Opening;
			}
			break;
		case EventWindowMode.Opening:
			if (WindowBack.transform.localScale.x >= MaxWidth){
				WindowBack.transform.localScale = new Vector3(MaxWidth,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
				NowEventWindow = EventWindowMode.Opened;
				BackWindowText = true;
				RollTextInitialization("Title: "+Datas.JukeBoxAudioName[selectedItem]+"\n"+"\n"+"Artist: "+Datas.JukeBoxAudioArtist[selectedItem]+"\n"+"\n"+"Release Date: "+Datas.JukeBoxAudioRelease[selectedItem]);
			}else{
				WindowBack.transform.localScale = WindowBack.transform.localScale + new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case EventWindowMode.Opened:
			GUIEnabled = true;
			OnControll = true;
			SelectMain();
			LogoJumpAnimation();
			break;
		case EventWindowMode.Closing:
			if (WindowBack.transform.localScale.x <= 0f){
				WindowBack.transform.localScale = new Vector3(0f,WindowBack.transform.localScale.y,WindowBack.transform.localScale.z);
				BackWindow.enabled = false;
				NowEventWindow = EventWindowMode.ClosedEnd;
				FadeOut.StartFlag = true;
				BackWindowText = false;
			}else{
				WindowBack.transform.localScale = WindowBack.transform.localScale - new Vector3(OpenSpeed,0f,0f);
			}
			break;
		case EventWindowMode.ClosedEnd:
			if (FadeOut.EndFlag == true){
				Application.LoadLevel("Reword");
			}
			break;
		}
	}

	void LogoJumpAnimation(){
		if (NowPlayBGM == true){
			if(LogoJumpAnimWaitTime >= AnimationLoopInterval){
				LogoAnim.Play(LogoJumpAnim.name);
				LogoJumpAnimWaitTime = 0f;
			}else{
				LogoJumpAnimWaitTime += Time.deltaTime;
			}
		}else{
			LogoJumpAnimWaitTime = AnimationLoopInterval;
		}
	}

	void VolumeChange(){
		if (Input.GetButton(Command.DPadLeftCommand) && OnControll == true){
			if (Waits <= 0f){
				Asource.volume -= 0.01f;
				Asource.volume = Mathf.Clamp(Asource.volume,0f,1f);
				if (NowPlayBGM == false){
					if (Asource.volume < 0f){
						_audios.PlayOneShot(ErrorSound);
					}else{
						_audios.PlayOneShot(SelectSound);
					}
				}
				Waits = VolumeChangeWaitTime;
			}else{
				Waits -= Time.deltaTime;
			}
		}else if (Input.GetButton(Command.DPadRightCommand) && OnControll == true){
			if (Waits <= 0f){
				Asource.volume += 0.01f;
				Asource.volume = Mathf.Clamp(Asource.volume,0f,1f);
				if (NowPlayBGM == false){
					if (Asource.volume > 1f){
						_audios.PlayOneShot(ErrorSound);
					}else{
						_audios.PlayOneShot(SelectSound);
					}
				}
				Waits = VolumeChangeWaitTime;
			}else{
				Waits -= Time.deltaTime;
			}
		}
		
	}

	void VolumeChanger(){
		Rect Pos = new Rect (Screen.width/100 * VolumeBarTransform.position.x,Screen.height/100 * VolumeBarTransform.position.y,(Screen.width/100 * VolumeBarTransform.position.x)+(Screen.width/100 * VolumeBarTransform.localScale.x),(Screen.height/100 * VolumeBarTransform.position.y)+(Screen.height/100 * VolumeBarTransform.localScale.y));
		print(Pos);

		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Label ("Volume",Style[0],GUILayout.Width(100));
		GUILayout.Space(10);
		Asource.volume = GUILayout.HorizontalSlider (Asource.volume, 0, 1,GUILayout.Width(Pos.width - 10));
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}
	
	void OnGUI(){
		Rect Pos;
		StyleX = new GUIStyle();
		StyleX.imagePosition = ImagePosition.TextOnly;

		if (GUIEnabled == true){

			StyleX.alignment = TextAnchor.MiddleCenter;
			GUI.matrix = Matrix4x4.identity;
			pivotPoint = new Vector2 ((Screen.width*FrameXPosition)+(w/2),(Screen.height*FrameYPosition)+(h/2));
			GUIUtility.RotateAroundPivot(rotAngle,pivotPoint);
			GUI.depth = Layer;

			StyleX.alignment = TextAnchor.MiddleCenter;
			StyleX.imagePosition = ImagePosition.TextOnly;
			StyleX.font = Style[0].font;
			StyleX.fontSize = Style[0].fontSize;
			
			for(int i= 0;i < MaxButtons ;i++){

				Pos = new Rect (Screen.width-((Screen.width*FrameXPosition)-w/2),
				                Screen.height/2-(h*(float)i+(Screen.height*FrameYPosition)-h-slide),w,h);

				GUI.enabled = Datas.JukeBoxAudioEnable[i];

				if (i == selectedItem){
					GUI.color = GuiColor;
				}else{
					GUI.color = DisabledColor;
				}

				StyleX.normal.textColor = Color.black;
				StyleX.contentOffset = new Vector2(2f,2f);

				if (Datas.JukeBoxAudioEnable[i] == true){
					GUI.enabled = true;
					GUI.DrawTexture(new Rect(Pos.x,Pos.y,w,h),ItemTex,ScaleMode.StretchToFill);
					GUI.Label(new Rect(Pos.x,Pos.y,w,h),Datas.JukeBoxAudioName[i],StyleX);
					StyleX.normal.textColor = Color.white;
					StyleX.contentOffset = Vector2.zero;
					GUI.Label(new Rect(Pos.x,Pos.y,w,h),Datas.JukeBoxAudioName[i],StyleX);
				}else{
					GUI.enabled = false;
					GUI.DrawTexture(new Rect(Pos.x,Pos.y,w,h),ItemTex,ScaleMode.StretchToFill);
					GUI.Label(new Rect(Pos.x,Pos.y,w,h),"???",StyleX);
					StyleX.normal.textColor = Color.white;
					StyleX.contentOffset = Vector2.zero;
					GUI.Label(new Rect(Pos.x,Pos.y,w,h),"???",StyleX);
				}
			}
			GUI.color = CurrentSelectTexColor;
			GUI.DrawTexture(new Rect(Screen.width-((Screen.width*FrameXPosition)-w/2),
			                         Screen.height/2-((Screen.height*FrameYPosition)-h),w,h),
			                		 CurrentSelectTex,ScaleMode.StretchToFill);

			TransformToRect2(new Vector2(VolumeBarTransform.position.x,VolumeBarTransform.position.y),new Vector2(VolumeBarTransform.localScale.x,VolumeBarTransform.localScale.y));
			Pos = ExportRect;

			StyleX.font = Style[1].font;
			StyleX.fontSize = Style[1].fontSize;

			GUILayout.BeginArea(Pos);
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(" ",StyleX);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			ShowGUITexts("Volume");
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			Asource.volume = GUILayout.HorizontalSlider (Asource.volume, 0, 1,GUILayout.Width(Pos.width - 10));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();

		}

		if (BackWindowText == true){
			StyleX.alignment = TextAnchor.UpperLeft;
			StyleX.contentOffset = Style[2].contentOffset;

			TransformToRect2(BackWindowPosition,BackWindowSize);
			Pos = ExportRect;

			StyleX.font = Style[2].font;
			StyleX.fontSize = Style[2].fontSize;

			GUILayout.BeginArea(Pos);
			GUILayout.BeginVertical();
			//GUILayout.FlexibleSpace();
			GUILayout.Label(" ",StyleX);
			GUILayout.Label(" ",StyleX);
			ShowRollMessage(SonMessage);
			GUILayout.Label(" ",StyleX);
			GUILayout.Label(" ",StyleX);
			GUILayout.Label(" ",StyleX);
			GUILayout.Label(" ",StyleX);
			//GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}

	void RollTextInitialization(string Text){
		TextLoopNumber = 0;
		FirstCount = true;
		SonMessage = Text;
	}

	void ShowRollMessage(string Text){
		float TimeLoop = 0f;
		if (LoopingTextInterval > TimeLoop){
			TimeLoop = 0f;
			if (FirstCount == true){
				ShowGUITexts("");
				FirstCount = false;
			}else{
				ShowGUITexts(Text.Substring(0,TextLoopNumber));
				if (TextLoopNumber < Text.Length){
					TextLoopNumber++;
				}else{
					TimeLoop = 0f;
					return;
				}
			}
		}else{
			TimeLoop+=Time.timeScale;
		}
	}

	void ShowGUITexts(string Text){
		StyleX.contentOffset = new Vector2(2f,2f);
		StyleX.normal.textColor = Color.black;
		GUI.Label (GUILayoutUtility.GetLastRect(),Text,StyleX);
		StyleX.normal.textColor = Color.white;
		StyleX.contentOffset = Vector2.zero;
		GUI.Label (GUILayoutUtility.GetLastRect(),Text,StyleX);
	}

	void TransformToRect(Transform Base){

		ExportRect = new Rect(Screen.width*(Base.position.x-Base.localScale.x/2),
		                      Screen.height*(Base.position.y-Base.localScale.y/2),
		                      Screen.width*(Base.position.x+Base.localScale.x/2),
		                      Screen.height*(Base.position.y+Base.localScale.y/2));
	}

	void TransformToRect2(Vector2 Pos,Vector2 Scale){
		
		ExportRect = new Rect((Screen.width * Pos.x - Screen.width * Pos.x / 2),
		                      (Screen.height * Pos.y - Screen.height * Pos.y / 2),
		                      ((Screen.width * Pos.x - Screen.width * Pos.x / 2)+(Screen.width * Scale.x - Screen.width * Scale.x / 2)),
		                      (Screen.height * Pos.y - Screen.height * Pos.y / 2)+(Screen.height * Scale.y - Screen.height * Scale.y / 2));
	}


	void PosSet(){
		w = Screen.width*ButtonWidth;
		h = Screen.height*ButtonHeight;
		MaxPos = h * (MaxButtons - 1);
	}
	
	void Round(){
		if(slide < 0f){
			if (NowPlayBGM == false){
				_audios.PlayOneShot(ErrorSound);
			}
			slide = 0f;
		}else if(slide > MaxPos){
			if (NowPlayBGM == false){
				_audios.PlayOneShot(ErrorSound);
			}
			slide = MaxPos;
		}else{
			RollTextInitialization("Title: "+Datas.JukeBoxAudioName[selectedItem]+"\n"+"\n"+"Artist: "+Datas.JukeBoxAudioArtist[selectedItem]+"\n"+"\n"+"Release Date: "+Datas.JukeBoxAudioRelease[selectedItem]);
			if (NowPlayBGM == false){
				_audios.PlayOneShot(SelectSound);
			}
		}
	}
	
	void SelectedItem(float status){
		selectedItem = ((int)slide / (int)status);
	}

	void SelectMain(){
		if (GUIEnabled == true){
			PosSet();
			if (Input.GetButton(Command.DPadUpCommand) && OnControll == true){
				switch(Sw){
				case 0:
					if (InversMode == true){
						slide -= h;
					}else{
						slide += h;
					}
					Round();
					SelectedItem(h);
					Sw = 1;
					Roll = RollTime;
					break;
				case 1:
					Roll -= Time.deltaTime;
					if (Roll <= 0f){
						Sw = 2;
						Roll = RollWait;
					}
					break;
				case 2:
					if (0f <= Roll){
						if (InversMode == true){
							slide -= h;
						}else{
							slide += h;
						}
						Round();
						SelectedItem(h);
						Roll = RollTime;
					}else{
						Roll -= Time.deltaTime;
						break;
					}
					break;
				}
				//RollTextInitialization("Title: "+Datas.JukeBoxAudioName[selectedItem]+"\n"+"\n"+"Artist: "+Datas.JukeBoxAudioArtist[selectedItem]+"\n"+"\n"+"Release Date: "+Datas.JukeBoxAudioRelease[selectedItem]);
			}else if (Input.GetButton(Command.DPadDownCommand) && OnControll == true){
				switch(Sw){
				case 0:
					if (InversMode == true){
						slide += h;
					}else{
						slide -= h;	
					}
					Round();
					SelectedItem(h);
					Sw = 1;
					Roll = RollTime;
					break;
				case 1:
					Roll -= Time.deltaTime;
					if (Roll <= 0f){
						Sw = 2;
						Roll = RollWait;
					}
					break;
				case 2:
					if (0f <= Roll){
						if (InversMode == true){
							slide += h;
						}else{
							slide -= h;	
						}
						Round();
						SelectedItem(h);
						Roll = RollTime;
					}else{
						Roll -= Time.deltaTime;
						break;
					}
					break;
				}
				//RollTextInitialization("Title: "+Datas.JukeBoxAudioName[selectedItem]+"\n"+"\n"+"Artist: "+Datas.JukeBoxAudioArtist[selectedItem]+"\n"+"\n"+"Release Date: "+Datas.JukeBoxAudioRelease[selectedItem]);
			}else{
				Roll = RollTime;
				Sw = 0;
			}
			if(Input.GetButtonDown(Command.EnterCommand) && OnControll == true){
				if (Datas.JukeBoxAudioEnable[selectedItem] == true){
					if (NowPlayBGM == true && Asource.clip == Datas.JukeBoxAudioClip[selectedItem]){
						Asource.Pause();
						NowPlayBGM = false;
					}else if (NowPlayBGM == true && Asource.clip != Datas.JukeBoxAudioClip[selectedItem]){
						Asource.Stop();
						Asource.clip = Datas.JukeBoxAudioClip[selectedItem];
						Asource.Play();
					}else if (NowPlayBGM == false && Asource.clip == Datas.JukeBoxAudioClip[selectedItem]){
						Asource.Play();
						NowPlayBGM = true;
					}else if (NowPlayBGM == false && Asource.clip != Datas.JukeBoxAudioClip[selectedItem]){
						_audios.PlayOneShot(EnterSound);
						Asource.clip = Datas.JukeBoxAudioClip[selectedItem];
						Asource.Play();
						NowPlayBGM = true;
					}
				}else{
					if (NowPlayBGM == false){
						_audios.PlayOneShot(ErrorSound);
					}
				}
			}else if(Input.GetButtonDown(Command.CancelCommand) && OnControll == true){
				if (NowPlayBGM == true){
					_audios.PlayOneShot(BackSound);
					Asource.Stop();
					NowPlayBGM = false;
				}else{
					_audios.PlayOneShot(BackSound);
					OnControll = false;
					GUIEnabled = false;
					FadeOut.StartFlag = true;
					NowEventWindow = EventWindowMode.Closing;
				}
			}
		}
		VolumeChange();
	}
	
}