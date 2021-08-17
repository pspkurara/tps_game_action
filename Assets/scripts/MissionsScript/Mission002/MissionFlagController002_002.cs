using UnityEngine;
using System.Collections;

public class MissionFlagController002_002 : MonoBehaviour {
	
	private MissionClearFlagStage2 MFlag;
	public GameObject EnemyPortals;
	public GameObject BarrierGate;
	private EnemyCreateZoneVer ECZ;
	public GameObject Needles;
	public GameObject Needles2;

	void Start () {
		MFlag = GameObject.Find("GameMissionClearFlag").GetComponent<MissionClearFlagStage2>();
		ECZ = EnemyPortals.GetComponent<EnemyCreateZoneVer>();
	}
	
	void Update(){
		if (MFlag.NowClearMission == 4 && ECZ.AllEnemyDestroy == true){
			MFlag.ClearConditions[3] = true;
			Destroy(EnemyPortals);
			Destroy(BarrierGate);
			Needles2.SetActive(true);
		}
	}
	
	void OnTriggerEnter(Collider coli){
		if(coli.gameObject.tag == "Player" && MFlag.NowClearMission == 3){
			MFlag.ClearConditions[2] = true;
			Destroy(Needles);
		}
	}
}
