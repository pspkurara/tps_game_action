using UnityEngine;
using System.Collections;

public class StageSelectSlideList: MonoBehaviour {
		
	float slide = 0f;
	float lastPos = 0f;
	public Texture2D CurrentSelectTex = null;
	public Texture2D ItemTex = null;
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
	float MaxPos;
	public bool MouseMode = false;
	public bool VerticalMode = false;
	public bool InversMode = false;
	public bool GUIEnabled = true;
	public bool OnControll = true;
	
	public GUITexture PrevImg = null;
	public GUIText LText = null;
	public FadeInFadeOut FI,FO;
	public int NowStatus = 0;
	private string NowSelectScene = null;
	
	public AudioClip EnterSound;
	public AudioClip CancelSound;
	public AudioClip SelectSound;
	public AudioClip ErrorSound;
	private AudioSource Asource;
	private OptionSetting Options;
	private ScoreResult Result;
	public float RollTime = 1f;
	public float RollWait = 0.3f;
	
	float Roll;
	int Sw = 0;
	
	Commands Command;
	private SlideListDatas Datas;
	private BGMPlay BGM;

	void Start(){
		//FI = FadeIn.GetComponent<FadeInFadeOut>();
		//FO = FadeOut.GetComponent<FadeInFadeOut>();
		Datas = GameObject.Find("Scores").GetComponent<SlideListDatas>();
		MaxButtons = Datas.ItemID.Length;
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		Asource = GetComponent<AudioSource>();
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		Result = GameObject.Find("Scores").GetComponent<ScoreResult>();
		BGM = GameObject.Find("BGM").GetComponent<BGMPlay>();
		Asource.volume = Options.SEVolume;
		FI.StartFlag = true;
		GUIEnabled = false;
		OnControll = false;
		LText.enabled = false;
		Roll = RollTime;
	}

	void Update(){
		
		switch(NowStatus){
		case 0:
			if (FI.EndFlag == true){
				LText.enabled = true;
				GUIEnabled = true;
				OnControll = true;
				selectedItem = Result.CurrentStageID;
				StartCurrentSet();
				NowStatus ++;
			}
			break;
		case 1:
			CurrentStageStatus();
			SelectMain();
			break;
		case 2:
			if (FO.EndFlag == true){
				GoScene();
			}
			break;
		}
	}
	
	void OnGUI(){
		if (GUIEnabled == true){
			GUI.matrix = Matrix4x4.identity;
			pivotPoint = new Vector2 ((Screen.width*FrameXPosition)+(w/2),(Screen.height*FrameYPosition)+(h/2));
			GUIUtility.RotateAroundPivot(rotAngle,pivotPoint);
			GUI.depth = Layer;
			
			//Style = new GUIStyle();
			
			for(int i= 0;i < (float)MaxButtons ;i++){
				GUI.enabled = Datas.ItemEnabled[i];
				Rect Pos;
				switch(VerticalMode){
				case true:
					//float WScale = Screen.width/FrameXPosition;
					//float HScale = Screen.height/FrameYPosition;
					Pos = new Rect (Screen.width-((Screen.width*FrameXPosition)-w/2),Screen.height/2-(h*(float)i+(Screen.height*FrameYPosition)-h-slide),w,h);
					Visibling(Pos,i);
					break;
				case false:
					Pos = new Rect(Screen.width-(w*(float)i+(Screen.width*FrameXPosition)-w/2-slide),Screen.height/2-((Screen.height*FrameYPosition)-h),w,h);
					Visibling(Pos,i);
					break;
				}
			}
			GUI.color = CurrentSelectTexColor;
			GUI.DrawTexture(new Rect(Screen.width-((Screen.width*FrameXPosition)-w/2),Screen.height/2-((Screen.height*FrameYPosition)-h),w,h),CurrentSelectTex,ScaleMode.StretchToFill);
		}
	}
	
	void PosSet(){
		w = Screen.width*ButtonWidth;
		h = Screen.height*ButtonHeight;
		switch(VerticalMode){
		case true:
			MaxPos = h * (MaxButtons - 1);
			break;
		case false:
			MaxPos = w * (MaxButtons - 1);
			break;
		}
	}
	
	void Round(){
		if(slide < 0f){
			Asource.PlayOneShot(ErrorSound);
			slide = 0f;
		}else if(slide > MaxPos){
			Asource.PlayOneShot(ErrorSound);
			slide = MaxPos;
		}else{
			Asource.PlayOneShot(SelectSound);
		}
	}
	
	void SelectedItem(float status){
		selectedItem = ((int)slide / (int)status);
	}
	
	void Visibling(Rect Pos,int i){
		if (i == selectedItem){
			GUI.color = Datas.ItemColor[i];
		}else{
			GUI.color = new Color(Datas.ItemColor[i].r,Datas.ItemColor[i].g,Datas.ItemColor[i].b,DisabledColor.a);
		}
		GUI.DrawTexture(new Rect(Pos.x,Pos.y,w,h),ItemTex,ScaleMode.StretchToFill);
		//GUILayout.BeginArea(new Rect(Pos.x,Pos.y,w,h));
		GUI.Label(new Rect(Pos.x,Pos.y,w,h),Datas.ItemName[i],Style[1]);
		GUI.Label(new Rect(Pos.x,Pos.y,w,h),Datas.ItemName[i],Style[0]);
		//GUILayout.EndArea();
	}
	
	void CurrentStageStatus(){
		PrevImg.texture = Datas.ItemPrevs[selectedItem];
		LText.text = Datas.ItemLegend[selectedItem];
		NowSelectScene = Datas.ItemFileName[selectedItem];
	}
	
	void SelectMain(){
		if (GUIEnabled == true){
			
			PosSet();
			Vector3 pos = Vector3.zero;
			
			switch(MouseMode){
			case true:
				//マウスで動かす.
				if (OnControll == true){
					pos = Input.mousePosition;
				}else{
					pos = Vector3.zero;
				}
				
				switch(VerticalMode){
				case true:
					if(lastPos > 0){
						if (InversMode == true){
							slide -= ((float)pos.y - lastPos) ;
						}else{
							slide += ((float)pos.y + lastPos) ;
						}
						slide -= ((float)pos.y - lastPos) ;
						lastPos = (float)pos.y;
						Round();
					}else{
						int k = (((int)slide + (int)h / 2) / (int)h);
						float dif = k*h-slide;
						if(Mathf.Abs((float)dif) < 0.1f){
							slide += dif;
							SelectedItem(h);
						}else{
							slide += 0.1f * dif;
						}
					}
			
					if(Input.GetMouseButtonDown(0)) {
						lastPos = pos.y;
					}else if(Input.GetMouseButtonUp(0)){
						lastPos=0;
					}
					break;
				case false:
					if(lastPos > 0){
						if (InversMode == true){
							slide -= ((float)pos.x - lastPos);
						}else{
							slide += ((float)pos.x + lastPos);
						}
						lastPos = (float)pos.x;
						Round();
					}else{
						int k = (((int)slide + (int)w / 2) / (int)w);
						float dif = k*w-slide;
						if(Mathf.Abs((float)dif) < 0.1f){
							slide += dif;
							SelectedItem(w);
						}else{
							slide += 0.1f * dif;
						}
					}
			
					if(Input.GetMouseButtonDown(0)) {
						lastPos = pos.x;
					}else if(Input.GetMouseButtonUp(0)){
						lastPos=0;
					}
					break;
				}
				break;
			case false:
				//キーで動かす.
				switch(VerticalMode){
				case true:
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
					}else{
						Roll = RollTime;
						Sw = 0;
					}
					break;
				case false:
					if (Input.GetButton(Command.DPadLeftCommand) && OnControll == true){
						switch(Sw){
						case 0:
							if (InversMode == true){
								slide += w;
							}else{
								slide -= w;
							}
							Round();
							SelectedItem(w);
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
									slide += w;
								}else{
									slide -= w;
								}
								Round();
								SelectedItem(w);
								Roll = RollTime;
							}else{
								Roll -= Time.deltaTime;
								break;
								}
							break;
						}
					}else if (Input.GetButton(Command.DPadRightCommand) && OnControll == true){
						switch(Sw){
						case 0:
							if (InversMode == true){
								slide -= w;
							}else{
								slide += w;
							}
							Round();
							SelectedItem(w);
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
									slide -= w;
								}else{
									slide += w;
								}
								Round();
								SelectedItem(w);
							}else{
								Roll -= Time.deltaTime;
								break;
							}
							break;
						}
					}else{
						Roll = RollTime;
						Sw = 0;
					}
					break;
				}
				break;
			}
			if(Input.GetButtonDown(Command.EnterCommand)){
				if (Datas.ItemEnabled[selectedItem] == true){
					Asource.PlayOneShot(EnterSound);
					OnControll = false;
					GUIEnabled = false;
					FO.StartFlag = true;
					NowStatus++;
					BGM.FadeOutBG();
				}else{
					Asource.PlayOneShot(ErrorSound);
				}
			}else if(Input.GetButtonDown(Command.CancelCommand)){
				Asource.PlayOneShot(CancelSound);
				NowSelectScene = "Title";
				OnControll = false;
				GUIEnabled = false;
				FO.StartFlag = true;
				NowStatus++;
				BGM.FadeOutBG();
			}
		}
	}
	
	void GoScene(){
		if (NowSelectScene !=null || NowSelectScene != ""){
			Result.CurrentStageID = selectedItem;
			Application.LoadLevel(NowSelectScene);
		}else{
			Debug.Log("LoadLevelError");
		}
	}
	
	void StartCurrentSet(){
		switch(VerticalMode){
		case true:
			for(int i= 0;i < selectedItem ;i++){
				slide += h;
			}
			break;
		case false:
			for(int i= 0;i < selectedItem ;i++){
				slide += w;
			}
			break;
		}
	}
	
}