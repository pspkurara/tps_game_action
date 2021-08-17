using UnityEngine;
using System.Collections;

public class EnabledBarrier : MonoBehaviour {
	
	public bool BarrierEnabled = false;
	public GameObject BarrierObj = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider coli){
		if(coli.gameObject.tag == "Player"){
			switch (BarrierEnabled){
			case true:
				BarrierObj.SetActive(true);
				break;
			case false:
				BarrierObj.SetActive(false);
				break;
			}
		}
	}
	
	public void TriggerMode(bool Trigger,bool DestroyMode){
		BarrierObj.SetActive(Trigger);
		if (Trigger == false && DestroyMode == true){
			Destroy(BarrierObj);
		}
	}
}
