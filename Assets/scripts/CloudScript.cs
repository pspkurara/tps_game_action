using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {
	
	
	public float velocity = 0.02f;
	public float limit = 50.0f;
	private int wayFactor = 1;
	// Use this for initialization
	void Start () {
		wayFactor = ( Random.Range( 0, 1) *2 )-1;		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		pos.x = pos.x +  Time.deltaTime * velocity * wayFactor;
		
		if( pos.x > limit || pos.x < -limit )
		{
			wayFactor *= -1;	
		}
		
		transform.position = pos;
	}
}