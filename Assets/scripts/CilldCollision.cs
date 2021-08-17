using UnityEngine;
using System.Collections;

public class CilldCollision : MonoBehaviour {
	
	CarStatus CS;
	public GameObject CarObjes; 
	
	void Start () {
		CS = CarObjes.GetComponent<CarStatus>();
	}
	
	public void ColliHit(int HitLevel,Vector3 pos1,GameObject MetalEffectPrefab){
		CS.CarHit(HitLevel,pos1,MetalEffectPrefab);
	}
}
