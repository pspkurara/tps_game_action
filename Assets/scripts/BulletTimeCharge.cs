using UnityEngine;
using System.Collections;

public class BulletTimeCharge : MonoBehaviour {
	
	public float ReleaseTime = 1.0f;
	private BulletMaxCheck BMC;
	private BulletScript BS;

	IEnumerator Start () {
		BS = GameObject.Find("PlayerStatus").GetComponent<BulletScript>();
		BMC = GameObject.Find(BS.BulletFolderSource.name).GetComponent<BulletMaxCheck>();
		if (ReleaseTime == 0f){
			yield return new WaitForSeconds(particleSystem.duration);
		}else{
			yield return new WaitForSeconds(ReleaseTime);
		}
		BMC.RemoveBullet(gameObject);
		Destroy(gameObject);
	}
}
