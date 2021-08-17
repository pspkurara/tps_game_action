using UnityEngine;
using System.Collections;

public class CameraNearClipChange : MonoBehaviour {
	
	public float CameraNearClipLengh = 3.2f;
	public float DefaultCameraNearClipLengh = 0.3f;
	private Camera MainCamera;
	
	// Use this for initialization
	void Start () {
	MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider Coli){
		if (Coli.gameObject.tag == "Player"){
			CameraNearChange(CameraNearClipLengh);
		}
	}
	
	void OnTriggerExit(Collider Coli){
		if (Coli.gameObject.tag == "Player"){
			CameraNearChange(DefaultCameraNearClipLengh);
		}
	}
	
	void CameraNearChange(float NearClip){
		MainCamera.nearClipPlane = NearClip;
	}
}
