using UnityEngine;
using System.Collections;

public class StartEnabled : MonoBehaviour {

	public bool StartEnable = true;
	
	void Start () {
		gameObject.SetActive(StartEnable);
	}
}
