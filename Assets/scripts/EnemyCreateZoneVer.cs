using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (BoxCollider))]

public class EnemyCreateZoneVer : MonoBehaviour {
	
	public GameObject[] EnemyEmergenceZone = {null};
	public GameObject[] AppearanceEnemys = {null};
	//public GameObject[] AppearanceEnemysDefault = {null};
	public bool[] EnemyPrefabFlag = {false};
	private GameObject EnemysObj;
	public int ZoneID = 0;
	private int ZoneType = 0;
	public int NowEnemys = 0;
	public int UnHideEnemys = 0;
	public float EnemySpeed = 0.07f;
	public int EnemyLifeSet = 1;
	public int EnemyDamageSet = 1;
	private EnemyFollow EnemyF = null;
	private EnemyCreat EC = null;
	public bool AllEnemyDestroy = false;
	bool OnEnemyFlag = false;
	public bool AllowedToAppearAtLeastOnce = false;
	private int RepopCount = 0;
	public float WaitInterval = 0.5f;
	private float Times = 0f;
	private int RandZoneObj;
	private Transform RandZoneTransform;
	private Vector3 RandomTransform;
			
	void Start () {
		EC = GameObject.Find ("EnemysScript").GetComponent<EnemyCreat> ();
		NowEnemys = AppearanceEnemys.Length;
		//AppearanceEnemysDefault = new GameObject[AppearanceEnemys.Length];
		EnemyPrefabFlag = new bool[AppearanceEnemys.Length];
		RepopCount = AppearanceEnemys.Length;
		for (int i = 0;i < AppearanceEnemys.Length;i++){
			//AppearanceEnemysDefault[i] = AppearanceEnemys[i];
			//AppearanceEnemys[i] = AppearanceEnemysDefault[i];
			EnemyPrefabFlag[i] = false;
		}
	}
	
	void Create(){
		//print ("AppearanceEnemys");
		//for (int i = 0;i < AppearanceEnemys.Length;i++){
			//print ("AppearanceEnemys ++");
		UnHideEnemys = 0;
		int i = RepopCount;
			if (AppearanceEnemys[i] != null){
				//print ("AppearanceEnemys Create Enemy");
				if (!GameObject.Find("Enemys")){
					EnemysObj = Instantiate(EC.EnemysPrefab,Vector3.zero,transform.rotation) as GameObject;
					EnemysObj.name = "Enemys";
				}else{
					EnemysObj = GameObject.Find("Enemys");
				}
				if (EnemyPrefabFlag[i] == false){
					RandomPos();
					AppearanceEnemys[i] = Instantiate(AppearanceEnemys[i],RandomTransform,transform.rotation) as GameObject;
					AppearanceEnemys[i].SetActive(true);
					EnemyPrefabFlag[i] = true;
					EnemyF = AppearanceEnemys[i].GetComponent<EnemyFollow>();
					EnemyF.EnemyAffiliationZoneID = ZoneID;
					EnemyF.EnemyAffiliationZoneType = ZoneType;
					EnemyF.EnemyCharacteristicID = i;
					EnemyF.EnemyLife = EnemyLifeSet;
					EnemyF.Damage = EnemyDamageSet;
					AppearanceEnemys[i].transform.parent = EnemysObj.transform;
				}else{
					RandomPos();
					AppearanceEnemys[i].SetActive(true);
					AppearanceEnemys[i].transform.position = RandomTransform;
				}
			UnHideEnemys += 1;
				//break;
			}else{
				//print ("AppearanceEnemys +Null");
			}
		//}
	}
	
	void RandomPos(){
		RandZoneObj = Random.Range(0,EnemyEmergenceZone.Length);
		RandZoneTransform = EnemyEmergenceZone[RandZoneObj].transform;
		RandomTransform = new Vector3(Random.Range(RandZoneTransform.position.x,RandZoneTransform.position.x + RandZoneTransform.localScale.x),RandZoneTransform.position.y + RandZoneTransform.localScale.y,Random.Range(RandZoneTransform.position.z,RandZoneTransform.position.z + RandZoneTransform.localScale.z));
	}
	
	void Update(){

		EC.NowEnemys = UnHideEnemys;

		if (AllEnemyDestroy == false){
			int AppearanceEnemysCheck = 0;
			for (int i = 0;i < AppearanceEnemys.Length;i++){
				if (AppearanceEnemys[i] == null){
					AppearanceEnemysCheck ++;
				}
				if (AppearanceEnemysCheck == AppearanceEnemys.Length){
					AllEnemyDestroy = true;
				}
				NowEnemys = AppearanceEnemysCheck;
			}
		}
		
		if (RepopCount < AppearanceEnemys.Length){
			if (Times >= WaitInterval){
				Create ();
				RepopCount ++;
				Times = 0f;
			}else{
				Times += Time.deltaTime;
			}
		}
	}

	public void EnemyDestroyed(int EnemyCharacteristicID){
		//AppearanceEnemys[EnemyCharacteristicID]
		UnHideEnemys--;
	}
	
	void HideEnemys(){
		for (int i = 0;i < AppearanceEnemys.Length;i++){
			if (AppearanceEnemys[i] != null){
				//Destroy(AppearanceEnemys[i]);
				if (EnemyPrefabFlag[i] == true){
					AppearanceEnemys[i].SetActive(false);
					UnHideEnemys --;
				}
			}
		}
	}
	
	public void RelayOnTriggerEnter(Collider Coli){
		if (Coli.CompareTag("Player")){
			if(OnEnemyFlag == false){
				RepopCount = 0;
				OnEnemyFlag = true;
				AllowedToAppearAtLeastOnce = true;
			}
		}
	}

	void EnemyCheck(){
		int AppearanceEnemysCheck = 0;
		for (int i = 0;i < AppearanceEnemys.Length;i++){
			if (AppearanceEnemys[i] == null){
				AppearanceEnemysCheck ++;
			}
			if (AppearanceEnemysCheck == AppearanceEnemys.Length){
				AllEnemyDestroy = true;
			}
			NowEnemys = AppearanceEnemysCheck;
		}
	}
	
	public void RelayOnTriggerExit(Collider Coli){
		if (Coli.CompareTag("Player")){
			if (OnEnemyFlag == true){
				RepopCount = AppearanceEnemys.Length;
				HideEnemys();
				OnEnemyFlag = false;
			}
		}
		if (Coli.CompareTag("Enemy01")){
			for (int i = 0;i < AppearanceEnemys.Length;i++){
				if (Coli.gameObject == AppearanceEnemys[i]){
					int RandZoneObj = Random.Range(0,EnemyEmergenceZone.Length);
					Transform RandZoneTransform = EnemyEmergenceZone[RandZoneObj].transform;
					Coli.gameObject.transform.position = new Vector3(Random.Range(RandZoneTransform.position.x,RandZoneTransform.position.x + RandZoneTransform.localScale.x),RandZoneTransform.position.y + RandZoneTransform.localScale.y,Random.Range(RandZoneTransform.position.z,RandZoneTransform.position.z + RandZoneTransform.localScale.z));
				}
			}
		}
	}
}