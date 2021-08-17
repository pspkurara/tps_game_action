using UnityEngine;
using System.Collections;

public class MissionClearFlagStage2 : MonoBehaviour {
	
	public int NowClearMission = 0;
	private EnemyCreat EnemyC;
	public bool[] ClearConditions = {false,false,false};
	public float NowMissionElapsedTime;
	public int[] NodamageBonusPoint = {5000,10000,50000,1000,1000};
	private GameClear GCO;
	private EventMessage EMS;
	private ScoreResult Score;
	private HUD_GUIs HG;
	private BGMPlay BGM;
	private float TimeConvert;
	private PlayerStatus PStatus;
	public int[] EnemyLifeMagnification = {1,3,5};
	public int[] EnemyAttackMagnification = {1,2,3};
	private OptionSetting Opt;
	private FadeInFadeOut FIS;
	public GameObject Items;
	public bool TimeCount = true;
	private PopupAddPointShow Pop;
	
	
	void EnemyLifeAdd(){
		switch(Opt.GameLevel){
		case 1:
			EnemyC.EnemyLifeSet *= EnemyLifeMagnification[Opt.GameLevel - 1];
			EnemyC.EnemyDamageSet *= EnemyAttackMagnification[Opt.GameLevel - 1];
			break;
		case 2:
			EnemyC.EnemyLifeSet *= EnemyLifeMagnification[Opt.GameLevel - 1];
			EnemyC.EnemyDamageSet *= EnemyAttackMagnification[Opt.GameLevel - 1];
			break;
		case 3:
			EnemyC.EnemyLifeSet *= EnemyLifeMagnification[Opt.GameLevel - 1];
			EnemyC.EnemyDamageSet *= EnemyAttackMagnification[Opt.GameLevel - 1];
			break;
		}
	}
	
	// Use this for initialization
	void Start () {
		HG = GameObject.Find("GUIs").GetComponent<HUD_GUIs>();
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Opt = GameObject.Find("Settings").GetComponent<OptionSetting>();
		PStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
		if (GameObject.Find("Player")){
			Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		}
		EnemyC = GameObject.Find("EnemysScript").GetComponent<EnemyCreat>();
		EMS = GameObject.Find("MissionMessage").GetComponent<EventMessage>();
		GCO = GameObject.Find("GameClearScript").GetComponent<GameClear>();
		FIS = GameObject.Find("1stFadeIn").GetComponent<FadeInFadeOut>();
		BGM = GameObject.Find("BGM").GetComponent<BGMPlay>();
		NowClearMission = 0;
		Score.ScoreReset();
		if (Opt.ItemOn == true){
			Items.SetActive(true);
		}else{
			Items.SetActive(false);
		}
		HG.GUIEnabled(false);
		FIS.StartFlag = true;
		BGM.VolumeFlag = 0;
		BGM.BGMPlaying();
	}
	
	// Update is called once per frame
	void Update () {
		if (Pop == null){
			if (GameObject.Find("Player")){
				Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
			}
		}
	switch(NowClearMission){
		case 0:
			if (FIS.EndFlag == true){
				PStatus.NoDamageFlag = true;
				HG.GUIEnabled(true);
				EMS.EventText = "先へ進め";
				EMS.WindowOpen();
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 1:
			if (ClearConditions[0] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[0].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[0];
				}
				EMS.EventText = "出現する全ての敵を撃破せよ";
				EMS.WindowOpen();
				Score.MissionElapsedTime[0] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				NowClearMission ++;
				HG.Refresh(false);
			}
			break;
		case 2:
			if (ClearConditions[1] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[1].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[1];
				}
				EMS.EventText = "先へ進め";
				EMS.WindowOpen();
				Score.MissionElapsedTime[1] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 3:
			if (ClearConditions[2] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[2].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[2];
				}
				EMS.EventText = "敵を100体撃破せよ";
				EMS.WindowOpen();
				Score.MissionElapsedTime[2] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 4:
			if (ClearConditions[3] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[3].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[3];
				}
				EMS.EventText = "先へ進め";
				EMS.WindowOpen();
				Score.MissionElapsedTime[3] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 5:
			if (ClearConditions[4] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[4].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[4];
				}
				EMS.EventText = "特定のニードルトラップを破壊せよ";
				EMS.WindowOpen();
				Score.MissionElapsedTime[4] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 6:
			if(ClearConditions[5] == true){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[5].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[5];
				}
				Score.MissionElapsedTime[5] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				TimeCount = false;
				NowMissionElapsedTime = Score.MissionElapsedTime[5];
				GCO.StartFlag = true;
				HG.Refresh(false);
			}
			break;
		}
		TimeCounter();
	}
	
	void TimeCounter(){
		TimeConvert = NowMissionElapsedTime;
		HG.TimeRefresh(TimeConvert);
		if (TimeCount == true){
			NowMissionElapsedTime += Time.deltaTime;
		}
	}
}
