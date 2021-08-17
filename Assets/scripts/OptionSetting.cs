using UnityEngine;
using System.Collections;

public class OptionSetting : MonoBehaviour {

	public float BGMVolume = 0.3f;
	public float SEVolume = 1f;
	public int GameLevel = 1;
	public bool ItemOn = true;
	public bool DebugMode = false;

	void OnGUI () {
		if (DebugMode == true){
			GUI.Label(new Rect (0,0,Screen.width,Screen.height),"ON_DEBUG_MODE");
		}
	}
	
	void Awake (){
		DontDestroyOnLoad(this);
		gameObject.name = "Settings";
	}
}
