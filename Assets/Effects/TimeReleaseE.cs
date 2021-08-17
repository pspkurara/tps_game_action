using UnityEngine;
using System.Collections;

public class TimeReleaseE : MonoBehaviour {
	public float sounddelay = 0f;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(10.0f);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
