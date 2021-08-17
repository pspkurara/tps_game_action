using UnityEngine;
using System.Collections;

public class CharacterIllumination : MonoBehaviour {
	
	public Title TitleScript;
	
	void Start (){
		TitleScript = GameObject.Find("Main").GetComponent<Title>();
	}
	
	public void LoadAnims(int IDx){
		TitleScript.PlayIntroAnims(IDx);
	}
	
	public void WaitChange(int Wt){
		if (Wt == 0){
			TitleScript.Wait = false;
		}else{
			TitleScript.Wait = true;
		}
	}

}
