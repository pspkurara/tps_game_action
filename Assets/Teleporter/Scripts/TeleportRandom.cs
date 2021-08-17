using UnityEngine;
using System.Collections;

public class TeleportRandom : MonoBehaviour {
	
	//The tag that we use to teleport.
	public string playerTag = "Player";
	//the transform where you should teleport to.
	
	//Sound to play when the player enters the teleporter
	public AudioClip teleportEnterAC;
	//Sound to play when the teleporter charges up
	public AudioClip teleportWaitAC;
	//Sound to play when the teleporter finished teleporting
	public AudioClip teleportExitAC;
	
	//The effect to create when the teleporter finishes
	public GameObject effectOnExit;
	//The effect to create when the teleporter charges up
	public GameObject effectOnChargeUp;
	//The time the effects should last
	public float effectOnChargeUpTTL = 1;
	//When you teleport a unit you want to change the height offset
	public float heightOffset = 1;
	//The number of times you want to charge before teleporting
	public int timesToCharge = 3;
	//The time between charging up
	public float chargeWaitTime = 1f;
	//The time before the teleporter finishes charging up and teleporting
	public float waitTime = 1f;	
	//The radius that that teleport uses to teleport everyone to the out positon
	public float teleportRadius = 5f;
	public Transform TeleportScale = null;
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.tag.Equals(playerTag))
		{
			//print ("Enter");
			audio.PlayOneShot(teleportEnterAC);
			StartCoroutine("chargeUpTeleporter",col.transform);
			//handleTeleportExit(col.transform);
		}
	}
	
	IEnumerator chargeUpTeleporter(Transform targetTransform)
	{
		for(int i=0; i<timesToCharge; i++)
		{
			createPowerup( effectOnChargeUp,transform.position);
			yield return new WaitForSeconds(chargeWaitTime);
			audio.PlayOneShot(teleportWaitAC);
			createPowerup( effectOnChargeUp,transform.position);
		}
		yield return new WaitForSeconds(waitTime);
			//print ("tel");
		handleTeleportExit();
	}
	void handleTeleportExit()
	{
		audio.PlayOneShot(teleportExitAC);
		Collider[] col = 
			Physics.OverlapSphere( transform.position,teleportRadius);
		for(int i=0; i<	col.Length; i++)
		{
			GameObject go = col[i].gameObject;
			if(go)
			{
				Vector3 offset = 
					go.transform.position - transform.position;
				
				CanTeleport ct = go.GetComponent<CanTeleport>();
				if(ct)
				{
					RaycastHit hit;
					Vector3 rand = new Vector3(Random.Range(TeleportScale.position.x,TeleportScale.position.x + TeleportScale.localScale.x),80f,Random.Range(TeleportScale.position.z,TeleportScale.position.z + TeleportScale.localScale.z));
					Physics.Raycast(rand, transform.rotation * Vector3.down, out hit, Mathf.Infinity);
					Vector3 pos1 = hit.point + new Vector3(0f,heightOffset,0f);
					Vector3 vec = rand;
					vec.x += offset.x;
					vec.z += offset.z;
					
					vec.y = ct.teleportHeightOffset + pos1.y;
					
					go.transform.position = vec;
				}
			}
		}
	}
	void createPowerup(GameObject g0,Vector3 pos)
	{
		if(g0)
		{
			GameObject newObject = (GameObject)Instantiate(g0,pos,Quaternion.identity);
			if(newObject)
			{
				Destroy(newObject,effectOnChargeUpTTL);
			}
		}
	}
	void OnTriggerExit(Collider col){
		if(col.tag.Equals(playerTag))
		{
			//print ("Exit");
			StopCoroutine("chargeUpTeleporter");
			//handleTeleportExit(col.transform);
		}
	}
}
