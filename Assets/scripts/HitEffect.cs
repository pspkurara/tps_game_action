using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {
	public GameObject DefaultEffectPrefab;
	public GameObject RockEffectPrefab;
	public GameObject WhiteRockEffectPrefab;
	public GameObject GrassEffectPrefab;
	public GameObject DirtEffectPrefab;
	public GameObject TestPrefab;
	public GameObject MetalEffectPrefab;
	public GameObject WoodEffectPrefab;
	public GameObject DarkWoodEffectPrefab;

	// Use this for initialization
	void Start () {
	//Vector3 pos1 = transform.position + new Vector3(0f,0f,0f);
	//pos1 += transform.rotation * Vector3.forward;
	//GameObject effect = (GameObject)Instantiate(TestPrefab,pos1,transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision coli){
		//print ("yieldout");
		Vector3 pos1 = transform.position + new Vector3(0f,0f,0f);
		pos1 += transform.rotation * Vector3.back;
		if (coli.gameObject.tag == "Rock"){
			//GameObject effect = (GameObject)Instantiate(RockEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "WhiteRock"){
			//GameObject effect = (GameObject)Instantiate(WhiteRockEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Grass"){
			//GameObject effect = (GameObject)Instantiate(GrassEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Dirt"){
			//GameObject effect = (GameObject)Instantiate(DirtEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Metal"){
			//GameObject effect = (GameObject)Instantiate(MetalEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Wood"){
			//GameObject effect = (GameObject)Instantiate(WoodEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "DarkWood"){
			//GameObject effect = (GameObject)Instantiate(DarkWoodEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Player" || coli.gameObject.tag == "Bullet"){
		}else{
			//GameObject effect = (GameObject)Instantiate(DefaultEffectPrefab,pos1,transform.rotation);
		}
		Destroy(gameObject);
	}
}