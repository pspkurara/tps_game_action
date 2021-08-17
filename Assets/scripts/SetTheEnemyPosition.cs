using UnityEngine;
using System.Collections;

public class SetTheEnemyPosition : MonoBehaviour {

	private EnemyFollow EF;
	
	void Start () {
		GameObject objColliderTriggerParent = gameObject.transform.parent.gameObject;
		EF = objColliderTriggerParent.GetComponent<EnemyFollow>();
	}
	
	void OnCollisionEnter(Collision coli){
		EF.RelayOnCollisionEnter(coli);
	}
	
	void OnTriggerEnter(Collider coli){
		EF.RelayOnTriggerEnter(coli);
	}
		
}
