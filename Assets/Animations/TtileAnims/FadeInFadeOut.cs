using UnityEngine;
using System.Collections;

public class FadeInFadeOut : MonoBehaviour {
	
	//色管理.
	public enum SetColors{
		black = 0,
		white = 1,
		red = 2,
	}
	
	private SetColors SC;
	private GUITexture GuiTx;
	
	//trueのときはFadeIn,falseのときはFadeOut.
	public bool I_O = false;
	
	//EndFlagがたった時、このスクリーンを消去するかどうか trueで消去.
	public bool LastDestroi = false;
	//EndFlagがたった時、このスクリーンを非表示にするか falseで非表示 またtrue時にはLastDestroiの効果はなくなる.
	public bool LastVisible = true;
	
	//色の設定.
	public int UseColor = 0;
	
	//開始時と終了時のフラグ.
	public bool EndFlag = false;
	public bool StartFlag = false;
	
	//アルファの一時保存先.
	private float AlphaColor;
	
	//アニメーションスピード.
	public float AnimInterval = 0.01f;
	
	//フェードインの動くタイミング.
	private bool LoopStart = false;
	
	//delta.timeの代入先.
	private float timer;
	
	//初期化処理.
	void Start () {
		GuiTx = GetComponent<GUITexture>();
		GuiTx.color = Color.black;
		GuiTx.enabled = false;
	}
	
	//アニメーションの再生終了を検知.
	void Update () {
		//アニメーションのスタート処理.
		if (StartFlag == true){
			SetFade();
			StartFlag = false;
			LoopStart = true;
		}
		
		//アニメーション本体.
		if (LoopStart == true){
			FadeLoop(I_O);
		}
		
		//アニメーションの最終処理.
		if (EndFlag == true){
			Last(LastVisible,LastDestroi);
		}
	}
	
	//フェードアニメーション処理.
	void FadeLoop(bool IIO){
		if (IIO == true){
			timer = Mathf.Clamp01(Time.deltaTime * AnimInterval);
			AlphaColor -= timer;
			if (AlphaColor < 0f){
				LoopStart = false;
				EndFlags();
	        }else{
				GuiTx.color = new Color(GuiTx.color.r,GuiTx.color.g,GuiTx.color.b,AlphaColor);
			}
		}else{
			timer = Mathf.Clamp01(Time.deltaTime * AnimInterval);
			AlphaColor += timer;
			if (AlphaColor > 1f){
				LoopStart = false;
				EndFlags();
	        }else{
				GuiTx.color = new Color(GuiTx.color.r,GuiTx.color.g,GuiTx.color.b,AlphaColor);
			}
		}
	}
	
	//フェードのおおまかな設定.
	public void SetFade (){
		if (I_O == true){
			FadeIn(UseColor);
		}else{
			FadeOut(UseColor);
		}
	}
	
	//色のフラグをSetColors関数に変換.
	void ColorExchange(int SetColorE){
		switch(SetColorE){
		case 0:
			SC = SetColors.black;
			break;
		case 1:
			SC = SetColors.white;
			break;
		case 2:
			SC = SetColors.red;
			break;
		}
		ColorSet(SC);
	}
	
	//FadeInの処理.
	void FadeIn (int SetColorE){
		GuiTx.enabled = true;
		ColorExchange(SetColorE);
		AlphaColor = 0.5f;
		GuiTx.color = new Color(guiTexture.color.r,guiTexture.color.g,guiTexture.color.b,AlphaColor);
	}
	
	//FadeOutの処理.
	void FadeOut (int SetColorE){
		GuiTx.enabled = true;
		ColorExchange(SetColorE);
		AlphaColor = 0f;
		GuiTx.color = new Color(guiTexture.color.r,guiTexture.color.g,guiTexture.color.b,AlphaColor);
	}
	
	//スクリーンの色を設定.
	void ColorSet (SetColors SetColor){
		switch(SetColor){
		case SetColors.black:
			GuiTx.color = Color.black;
			break;
		case SetColors.white:
			GuiTx.color = Color.white;
			break;
		case SetColors.red:
			GuiTx.color = Color.red;
			break;
		}
	}
	
	//アニメーション終了時のフラグをONにする.
	void EndFlags(){
		EndFlag = true;
	}
	
	//アニメーション終了時の最終処理.
	void Last(bool noDelete ,bool MeDestroi){
		if (!noDelete == true){
		guiTexture.enabled = false;
			if (MeDestroi == true){
				Destroy(gameObject);
			}
		}
	}
}
