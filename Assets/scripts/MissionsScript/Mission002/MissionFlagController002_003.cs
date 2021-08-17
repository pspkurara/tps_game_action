using UnityEngine;
using System.Collections;

public class MissionFlagController002_003 : MonoBehaviour {

	public GameObject Wall;
	public GameObject Traps;
	private MissionClearFlagStage2 MFlag;
	private DObject DObj;
	private ShootControl SControl;
	
	// Use this for initialization
	void Start () {
		MFlag = GameObject.Find("GameMissionClearFlag").GetComponent<MissionClearFlagStage2>();
		DObj = GetComponent<DObject>();
		if (GameObject.Find("Player")){
			SControl = GameObject.Find("Player").GetComponent<ShootControl>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (DObj.LifeZeroFlag == true && MFlag.NowClearMission == 6){
			Destroy(Traps);
			Destroy(Wall);
			if (GameObject.Find("Player") && SControl == null){
				SControl = GameObject.Find("Player").GetComponent<ShootControl>();
			}
			SControl.ReloadOnFlag = true;
			MFlag.ClearConditions[5] = true;
		}
	}
}
