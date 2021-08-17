using UnityEngine;
using System.Collections;

public class MissinonClearFlag : MonoBehaviour {
	
	public int NowClearMission = 0;
	private EnemyCreat EnemyC;
	public int[] ClearConditions = {10,50,100};
	public float NowMissionElapsedTime;
	public int[] NodamageBonusPoint = {5000,10000,50000};
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
		if (GameObject.Find("PlayerPop")){
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
		if (Pop != null){
			if (GameObject.Find("PlayerPop")){
				Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
			}
		}
	switch(NowClearMission){
		case 0:
			if (FIS.EndFlag == true){
				PStatus.NoDamageFlag = true;
				HG.GUIEnabled(true);
				EMS.EventText = "敵を10体撃破せよ";
				EMS.WindowOpen();
				EnemyLifeAdd();
				EnemyC.LoopCreate(10);
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 1:
			if(Score.NowMissionKilledEnemys == ClearConditions[0]){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[0].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[0];
				}
				EMS.EventText = "敵を50体撃破せよ";
				EMS.WindowOpen();
				Score.MissionElapsedTime[0] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				EnemyLifeAdd();
				EnemyC.LoopCreate(50);
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 2:
			if(Score.NowMissionKilledEnemys == ClearConditions[1]){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[1].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[1];
				}
				EMS.EventText = "敵を100体撃破せよ";
				EMS.WindowOpen();
				Score.MissionElapsedTime[1] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				NowMissionElapsedTime = 0f;
				EnemyLifeAdd();
				EnemyC.LoopCreate(100);
				HG.Refresh(false);
				NowClearMission ++;
			}
			break;
		case 3:
			if(Score.NowMissionKilledEnemys >= ClearConditions[2]){
				if (PStatus.NoDamageFlag == true){
					Pop.InitSetting("+" + NodamageBonusPoint[2].ToString());
					Score.NoDamageBonus = Score.NoDamageBonus + NodamageBonusPoint[2];
				}
				Score.MissionElapsedTime[2] = NowMissionElapsedTime;
				Score.NowMissionKilledEnemys = 0;
				TimeCount = false;
				NowMissionElapsedTime = Score.MissionElapsedTime[2];
				GCO.StartFlag = true;
				HG.Refresh(false);
				Score.StageLiftingOfTheBan(1,true);
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
