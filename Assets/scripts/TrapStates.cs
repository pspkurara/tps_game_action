using UnityEngine;
using System.Collections;

public class TrapStates : MonoBehaviour {
	
	public int Damage;
	public float AnimStartTimeDelay = 0f;
	public GameObject TrapNeedle;
	TrapDamageScript Tdamage;
	
	void Start(){
		Tdamage = TrapNeedle.GetComponent<TrapDamageScript>();
	}
	
	void SoundPlay(){
		Tdamage.SoundPlay();
	}
}
