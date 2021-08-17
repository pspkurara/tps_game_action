using UnityEngine;
using System.Collections;

public class TimeRelease : MonoBehaviour {
	public float ReleaseTime = 1.0f;
	
	IEnumerator Start () {
		if (ReleaseTime == 0f){
			yield return new WaitForSeconds(particleSystem.duration);
		}else{
			yield return new WaitForSeconds(ReleaseTime);
		}
		Destroy(gameObject);
	}
}
