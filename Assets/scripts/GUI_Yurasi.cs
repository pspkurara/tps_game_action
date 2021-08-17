using UnityEngine;
using System.Collections;

public class GUI_Yurasi : MonoBehaviour {
	
	//public GUIText YureGui = null;
	public Vector3 YureGuiSave;
	public float YureTime = 0.5f;
	public float Tateyure = 0.002f;
	public float Yokoyure = 0.001f;
	private bool YureFlag = false;
	private int Yures = 0;
	
	// Use this for initialization
	void Start () {
			YureGuiSave = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (YureFlag == true){
			switch(Yures){
			case 0:
				transform.position = YureGuiSave + new Vector3(Tateyure,Yokoyure,0f);
				Yures = 1;
				break;
			case 1:
				transform.position = YureGuiSave + new Vector3(-1 * Tateyure,-1 * Yokoyure,0f);
				Yures = 0;
				break;
			}
		}else{
			transform.position = YureGuiSave;
		}
	}
	
	public void Yure(){
		StopCoroutine("Yuretomaru");
		StartCoroutine(Yuretomaru(YureTime));
		Yures = 0;
		YureFlag = true;
	}
	
	IEnumerator Yuretomaru(float YureJikan){
		yield return new WaitForSeconds(YureJikan);
		YureFlag = false;
		transform.position = YureGuiSave;
		Yures = 0;
	}
}
