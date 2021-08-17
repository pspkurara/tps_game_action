using UnityEngine;
using System.Collections;

public class HitSound : MonoBehaviour {
	public AudioClip HitSFX;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision other){
		audio.PlayOneShot(HitSFX);
	}
}
