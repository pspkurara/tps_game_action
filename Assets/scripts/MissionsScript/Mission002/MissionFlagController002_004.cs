using UnityEngine;
using System.Collections;

public class MissionFlagController002_004 : MonoBehaviour {

	private MissionClearFlagStage2 MFlag;
	private ShootControl SControl = null;
	public GameObject Needles;
	
	void Start () {
		MFlag = GameObject.Find("GameMissionClearFlag").GetComponent<MissionClearFlagStage2>();
	}
	
	
	void OnTriggerEnter (Collider coli){
		if (MFlag.NowClearMission == 5){
			if (GameObject.Find("Player") && SControl == null){
				SControl = GameObject.Find("Player").GetComponent<ShootControl>();
			}
			MFlag.ClearConditions[4] = true;
			SControl.ReloadOnFlag = false;
			Destroy(Needles);
		}
	}
}
