using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleEmitter))]

public class EnemyDestroi : MonoBehaviour {
	
	public ParticleEmitter _Emitting;
	
	void Awake(){
		_Emitting = GetComponent<ParticleEmitter>();
	}
	
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		_Emitting.emit = false;
		yield return new WaitForSeconds(3.0f);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
