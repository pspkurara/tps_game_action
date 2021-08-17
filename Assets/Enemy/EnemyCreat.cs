using UnityEngine;
using System.Collections;

public class EnemyCreat : MonoBehaviour {
	
	public GameObject EnemysPrefab;
	private GameObject EnemysObj;
	public int EnemyMax = 3;
	public float MaxSizeX = 85f;
	public float MinSizeX = -85f;
	public float MaxSizeZ = 45f;
	public float MinSizeZ = -85f;
	public float AddHeight = 80f;
	//public int StartEnterEnemys = 0;
	public int NowEnemys = 0;
	public int KilledEnemys = 0;
	public GameObject Enemy = null;
	public float EnemySpeed = 0.07f;
	public int EnemyLifeSet = 1;
	public int EnemyDamageSet = 1;
	private GameObject EnemyStatusAdd;
	private EnemyFollow EnemyF = null;
	public bool EnemySpeedShare = true;

	void Start () {
	}
	
	//void OnGUI(){
	//if (GUILayout.Button("Create")) Create();
	//}

	public void EnemyZoneTypeDecision(int ZoneType, int ZoneID, int CID){
		GameObject[] SearchObj = {null};
		switch (ZoneType) {
		case 0:
			EnemyCreateZoneVer EZone;
			SearchObj = new GameObject[GameObject.FindGameObjectsWithTag("EnemyPortal:Type0").Length];
			for (int i = 0;i < GameObject.FindGameObjectsWithTag("EnemyPortal:Type0").Length;i++){
				EZone = SearchObj[i].GetComponent<EnemyCreateZoneVer>();
				if (EZone.ZoneID == ZoneID){
					EZone.EnemyDestroyed(CID);
				}
			};
			break;
		default:
			break;
		}
	}
	
	void Create(){
		if (!GameObject.Find("Enemys")){
			EnemysObj = Instantiate(EnemysPrefab,Vector3.zero,transform.rotation) as GameObject;
			EnemysObj.name = "Enemys";
		}else{
			EnemysObj = GameObject.Find("Enemys");
		}
		EnemyStatusAdd = Instantiate(Enemy,new Vector3(Random.Range(MinSizeX,MaxSizeX),AddHeight,Random.Range(MinSizeZ,MaxSizeZ)),transform.rotation) as GameObject;
		EnemyStatusAdd.transform.parent = EnemysObj.transform;
		EnemyF = EnemyStatusAdd.GetComponent<EnemyFollow>();
		if (EnemySpeedShare == true){
			EnemyF.EnemyLife = EnemyLifeSet;
		}
		EnemyF.Damage = EnemyDamageSet;
		NowEnemys = NowEnemys + 1;
	}
	
	public void LoopCreate(int StartEnterEnemys){
		for(int i = 1;i < StartEnterEnemys + 1;i++){
			Create();
		}
	} 
}
