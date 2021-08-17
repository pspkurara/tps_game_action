using UnityEngine;
using System.Collections;

public class ScoreResult : MonoBehaviour {
	
	public int TotalScore = 0;
	public int NowMissionKilledEnemys = 0;
	public int TotalKilledEnemys = 0;
	public float[] MissionElapsedTime = {0f,0f,0f};
	public float ClearTimeNowGame = 0f;
	public float[] ClearTimeHighscore = {0f};
	public int[] TotalHighscore = {0};
	public int HaveCashes = 0;
	public int BonusCashe = 0;
	
	public int MainScore = 0;
	public int NoDamageBonus = 0;
	public int ObjectDestructionBonus = 0;
	public int EntrainmentEnemyKillBonus = 0;
	public int CurrentStageID = 0;
	private SlideListDatas SData;
	public int NowCostumeID = 0;
	public ArrayList ClearTrophys = new ArrayList();
	private TrophyDatas TData;

	void GetTrophy(int index){
		if (TData.TrophyClear[index] == false){
			ClearTrophys.Add(index);
			print ("GettedTrophy (" + index + ")");
		}
	}
	
	void Awake (){
		DontDestroyOnLoad(this);
		gameObject.name = "Scores";
		SData = GetComponent<SlideListDatas>();
		TData = GetComponent<TrophyDatas>();
	}
	
	void StageSet(int StageSet){
		CurrentStageID = StageSet;
	}
	
	public void ScoreReset(){
		TotalScore = 0;
		NowMissionKilledEnemys = 0;
		TotalKilledEnemys = 0;
		for (int i = 0;i > MissionElapsedTime.Length;i++){
			MissionElapsedTime[i] = 0f;
		}
		ClearTimeNowGame = 0f;
		MainScore = 0;
		NoDamageBonus = 0;
		ObjectDestructionBonus = 0;
		EntrainmentEnemyKillBonus = 0;
		BonusCashe = 0;
		ClearTrophys.Clear();
	}
	
	public void StageLiftingOfTheBan(int StageID, bool Enabled){
		SData.ItemEnabled[StageID] = Enabled;
	}

	/*void OnGUI(){
		if (GUI.Button(new Rect(0,0,100,50),"Add Trophy")){
			GetTrophy(0);
			GetTrophy(1);
			GetTrophy(2);
			print ("Add Trophy");
		}
	}*/

}
