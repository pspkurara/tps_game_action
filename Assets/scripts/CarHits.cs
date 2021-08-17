using UnityEngine;
using System.Collections;

public class CarHits : MonoBehaviour {
	
	public int CarHitLife = 0;
	public CarStatus CS;
	// Use this for initialization
	void Start () {
		CS = GetComponent<CarStatus>();
	}
	
	// Update is called once per frame
	void Update () {
		CarHitLife = CS.CarLife;
	}
}
