using UnityEngine;
using System.Collections;

public class ForceTelepo : MonoBehaviour {
	
	//The tag that we use to teleport.
	public string playerTag = "Player";
	//the transform where you should teleport to.
	public Transform targetOut;
	
	public float heightOffset = 4f;
	
	public float teleportRadius = 5f;
	
	public AudioClip teleportExitAC1;

	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag.Equals(playerTag))
		{
			GameObject go = col.gameObject;
			audio.PlayOneShot(teleportExitAC1);
			Physics.OverlapSphere(transform.position,teleportRadius);
			CanTeleport ct = go.GetComponent<CanTeleport>();
			if(ct)
			{
				Vector3 vec = targetOut.transform.position;
				vec.y += ct.teleportHeightOffset;
				go.transform.position = vec;
			}
		}
	}
}
