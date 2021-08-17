using UnityEngine;
using System.Collections;

public class Comes : MonoBehaviour {
	
	private Transform target;
	
	void Start(){
		target = GameObject.Find("BloodShot").transform;
	}
	
	void Update () {
		transform.position = target.transform.position;
	}
}
