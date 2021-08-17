using UnityEngine;
using System.Collections;

public class DirectionShadow : MonoBehaviour {
	
	public Transform Direction;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	transform.rotation = Direction.transform.rotation;
	}
}
