using UnityEngine;
using System.Collections;

public class MissionFlagController002_001 : MonoBehaviour {
	
	private MissionClearFlagStage2 MFlag;
	public GameObject[] EnemyPortals = {null,null};
	public GameObject BarrierGate;
	private EnemyCreateZoneVer[] ECZ = {null,null};
	public int MaxKilledEnemy;
	private ScoreResult Score;
	public GameObject[] Needles = {null,null};

	void Start () {
		MFlag = GameObject.Find("GameMissionClearFlag").GetComponent<MissionClearFlagStage2>();
		ECZ[0] = EnemyPortals[0].GetComponent<EnemyCreateZoneVer>();
		ECZ[1] = EnemyPortals[1].GetComponent<EnemyCreateZoneVer>();
		MaxKilledEnemy = ECZ[0].AppearanceEnemys.Length;
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
	}
	
	void Update(){
		if (ECZ[1].AllowedToAppearAtLeastOnce == true){
			MaxKilledEnemy = ECZ[0].AppearanceEnemys.Length +  ECZ[1].AppearanceEnemys.Length;
		}
		if (MFlag.NowClearMission == 2 && Score.NowMissionKilledEnemys >= MaxKilledEnemy){
			MFlag.ClearConditions[1] = true;
			Destroy(EnemyPortals[0]);
			Destroy(EnemyPortals[1]);
			Destroy(BarrierGate);
			Needles[0].SetActive(true);
			Needles[1].SetActive(true);
		}
	}
	
	void OnTriggerEnter(Collider coli){
		if(coli.gameObject.tag == "Player" && MFlag.NowClearMission == 1){
			MFlag.ClearConditions[0] = true;
		}
	}
}
