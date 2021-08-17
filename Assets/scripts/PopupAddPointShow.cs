using UnityEngine;
using System.Collections;

public class PopupAddPointShow : MonoBehaviour {
	
	public float EndSpacing = 2f;
	public float SlideSpeed = 0.05f;
	public float ColorChangeSpeed = 0.05f;
	private Camera MainCam;
	private TextMesh Tex;
	private MeshRenderer MeshTex;
	public bool OnTimeStamp = false;
	// Use this for initialization
	void Start () {
		MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Tex = GetComponent<TextMesh>();
		MeshTex = GetComponent<MeshRenderer>();
		Tex.color = new Color(Tex.color.r,Tex.color.g,Tex.color.b,0f);
		Tex.lineSpacing = EndSpacing;
		MeshTex.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(MainCam.transform.forward, Vector3.up);
		if (OnTimeStamp == false){
			Show();
		}
	}
	
	void Show(){
		if(Tex.lineSpacing >= EndSpacing){
			Tex.color = new Color(Tex.color.r,Tex.color.g,Tex.color.b,0f);
			Tex.lineSpacing = EndSpacing;
			MeshTex.enabled = false;
		}else{
			MeshTex.enabled = true;
			Tex.color -= new Color(0f,0f,0f,ColorChangeSpeed);
			Tex.lineSpacing += SlideSpeed;
		}
	}
	
	public void InitSetting(string SetString){
		Tex.text = SetString;
		Tex.color = new Color(Tex.color.r,Tex.color.g,Tex.color.b,1f);
		Tex.lineSpacing = 0f;
	}
}
