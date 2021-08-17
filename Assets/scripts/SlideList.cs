using UnityEngine;
using System.Collections;

public class SlideList : MonoBehaviour {
		
	float slide = 0f;
	float lastPos = 0f;
	public Texture2D CurrentSelectTex = null;
	public Texture2D ItemTex = null;
	public GUIStyle Style;
	public Color CurrentSelectTexColor = Color.white;
	public Color DisabledColor = Color.gray;
	public int selectedItem = 0;
	public int GUIFontSize = 5;
	float w;
	float h;
	public float rotAngle = 0f;
    Vector2 pivotPoint;
	public float ButtonWidth = 96f;
	public float ButtonHeight = 64f;
	public float FrameXPosition = 240f;
	public float FrameYPosition = 320f;
	public int MaxButtons = 1;
	public int Layer = 1;
	float MaxPos;
	public bool MouseMode = false;
	public bool VerticalMode = false;
	public bool InversMode = false;
	public bool GUIEnabled = true;
	public bool OnControll = true;
	
	Commands Command;
	SlideListDatas Datas;

	void Start(){
		Datas = GetComponent<SlideListDatas>();
		MaxButtons = Datas.ItemID.Length;
		Command = GameObject.Find("Commands").GetComponent<Commands>();
	}

	void Update(){
		
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
					if (Input.GetButtonDown(Command.DPadUpCommand) && OnControll == true){
						if (InversMode == true){
							slide -= h;
						}else{
							slide += h;
						}
						Round();
						SelectedItem(h);
					}else if (Input.GetButtonDown(Command.DPadDownCommand) && OnControll == true){
						if (InversMode == true){
							slide += h;
						}else{
							slide -= h;	
						}
						Round();
						SelectedItem(h);
					}
					break;
				case false:
					if (Input.GetButtonDown(Command.DPadLeftCommand) && OnControll == true){
						if (InversMode == true){
							slide += w;
						}else{
							slide -= w;
						}
						Round();
						SelectedItem(w);
					}else if (Input.GetButtonDown(Command.DPadRightCommand) && OnControll == true){
						if (InversMode == true){
							slide -= w;
						}else{
							slide += w;
						}
						Round();
						SelectedItem(w);
					}
					break;
				}
				break;
			}
		}
	}
	
	void OnGUI(){
		if (GUIEnabled == true){
			GUI.matrix = Matrix4x4.identity;
			pivotPoint = new Vector2 (FrameXPosition+(w/2),FrameYPosition+(h/2));
			GUIUtility.RotateAroundPivot(rotAngle,pivotPoint);
			GUI.depth = Layer;
			
			//Style = new GUIStyle();
			
			for(int i= 0;i < (float)MaxButtons ;i++){
				GUI.enabled = Datas.ItemEnabled[i];
				Rect Pos;
				switch(VerticalMode){
				case true:
					Pos = new Rect (FrameXPosition-w/2,h*(float)i+FrameYPosition-h-slide,w,h);
					Visibling(Pos,i);
					break;
				case false:
					Pos = new Rect(w*(float)i+FrameXPosition-w/2-slide,FrameYPosition-h,w,h);
					Visibling(Pos,i);
					break;
				}
			}
			GUI.color = CurrentSelectTexColor;
			GUI.DrawTexture(new Rect(FrameXPosition-w/2,FrameYPosition-h,w,h),CurrentSelectTex,ScaleMode.StretchToFill);
		}
	}
	
	void PosSet(){
		w = ButtonWidth;
		h = ButtonHeight;
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
			slide = 0f;
		}else if(slide > MaxPos){
			slide = MaxPos;
		}
	}
	
	void SelectedItem(float status){
		selectedItem = (int)slide / (int)status;
	}
	
	void Visibling(Rect Pos,int i){
		if (i == selectedItem){
			GUI.color = Datas.ItemColor[i];
		}else{
			GUI.color = new Color(Datas.ItemColor[i].r,Datas.ItemColor[i].g,Datas.ItemColor[i].b,DisabledColor.a);
		}
		GUI.DrawTexture(Pos,ItemTex,ScaleMode.StretchToFill);
		GUILayout.BeginArea(Pos);
		GUILayout.Label(Datas.ItemName[i],Style);
		GUILayout.EndArea();
	}
	
}