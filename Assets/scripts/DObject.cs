using UnityEngine;
using System.Collections;

public class DObject : MonoBehaviour {
	
	public int Life = 1;
	public GameObject HitEffectPrefab = null;
	public GameObject DeadEffectPrefab = null;
	public AudioClip HitAudio = null;
	public AudioClip DeadAudio = null;
	public bool LifeZeroFlag = false;
	public bool DeadDestroy = false;
	private bool AudioEnabled = false;
	private AudioSource Audio;
	
	// Use this for initialization
	void Start () {
		gameObject.tag = "DestructibleObjects";
		if (Audio != null){
			AudioEnabled = true;
			Audio = GetComponent<AudioSource>();
		}
	}
	
	public void HitObj(int DamagePoint,Vector3 pos1){
		Life = Life - DamagePoint;
		if (HitEffectPrefab != null){
			Instantiate(HitEffectPrefab,pos1,transform.rotation);
		}
		if (HitAudio != null && AudioEnabled == true){
			Audio.PlayOneShot(HitAudio);
		}
		if (Life <= 0){
			if (DeadEffectPrefab != null){
				Instantiate(DeadEffectPrefab,pos1,transform.rotation);
			}
			if (DeadAudio != null && AudioEnabled == true){
				Audio.PlayOneShot(DeadAudio);
			}
			LifeZeroFlag = true;
			if (DeadDestroy == true){
				Destroy(gameObject);
			}
		}
	}
}
