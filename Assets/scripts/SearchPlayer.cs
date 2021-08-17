using UnityEngine;
using System.Collections;

public class SearchPlayer : MonoBehaviour {
	
	public EnemyFollow EnemyObject;
	
	void OnTriggerEnter(Collider coli){
		if (coli.gameObject.tag == "Player"){
			EnemyObject.PlayerSearched(true);
		}
	}
	
	//void OnTriggerExit(Collider coli){
	//	if (coli.gameObject.tag == "Player"){
	//		EnemyObject.PlayerSearched(false);
	//	}
	//}
}
