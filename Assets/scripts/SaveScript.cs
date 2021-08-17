using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SaveScript : MonoBehaviour {
	
	private OptionSetting Options;
	private ScoreResult Scores;
	private SlideListDatas Datas;
	private Costumes Costume;
	private TrophyDatas Trophy;
	public GameObject SettingsPrefab;
	public GameObject ScoresPrefab;
	//private string projectFolder;
	
	void Awake (){
		if (!GameObject.Find("Scores")){
			Instantiate(ScoresPrefab,new Vector3(0f,0f,0f),transform.rotation);
		}
		if (!GameObject.Find("Settings")){
			Instantiate(SettingsPrefab,new Vector3(0f,0f,0f),transform.rotation);
		}
	}
	
	void Start () {
		//projectFolder = System.IO.Directory.GetCurrentDirectory();
		//print (projectFolder);
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		Scores = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Datas = GameObject.Find("Scores").GetComponent<SlideListDatas>();
		Costume = GameObject.Find("Scores").GetComponent<Costumes>();
		Trophy = GameObject.Find("Scores").GetComponent<TrophyDatas>();
	}
	
	void Update () {
	}
	
	public void Save(){
		
		//Debug.Log("Save is started.");
		
		Start();
		
		FileStream BinaryFile= null;
		
		try{
			BinaryFile = new FileStream("Save.dat", FileMode.Create, FileAccess.ReadWrite);
	        BinaryWriter  Writer = new BinaryWriter(BinaryFile);
	        
	        Writer.Write((double)Options.BGMVolume);
			Writer.Write((double)Options.SEVolume);
			Writer.Write(Options.GameLevel);
			Writer.Write(Options.ItemOn);
			for(int i = 0;i < Datas.ItemID.Length ;i++){
				Writer.Write(Datas.ItemEnabled[i]);
				Writer.Write((double)Scores.ClearTimeHighscore[i]);
				Writer.Write(Scores.TotalHighscore[i]);
			}
			Writer.Write(Scores.CurrentStageID);
			for(int i = 0;i <Costume.CostumeUnlock.Length; i++){
				Writer.Write(Costume.CostumeUnlock[i]);
			}
			Writer.Write(Scores.NowCostumeID);
			for(int i = 0;i < Trophy.TrophyEnable.Length; i++){
				Writer.Write(Trophy.TrophyEnable[i]);
				Writer.Write(Trophy.TrophyClear[i]);
			}
			Writer.Write(Scores.HaveCashes);
			//Debug.Log("Save is completed.");
			
		}catch(Exception){
			Debug.Log("Save is error.");
		}
		finally{
			if(BinaryFile!=null){
			BinaryFile.Close();
			}
		}
	}
	
	public void Load(){
		
		Start ();
		
		//Debug.Log("Load is started.");
		
		FileStream BinaryFile= null;
		
		try{


			BinaryFile = new FileStream("Save.dat", FileMode.OpenOrCreate, FileAccess.Read);
			BinaryReader Reader = new BinaryReader(BinaryFile);
			
			BinaryFile.Seek(0, SeekOrigin.Begin);
			
	        Options.BGMVolume = (float)Reader.ReadDouble();
	        Options.SEVolume  = (float)Reader.ReadDouble();
	        Options.GameLevel = Reader.ReadInt32();
	        Options.ItemOn = Reader.ReadBoolean();
			for(int i = 0;i < Datas.ItemID.Length ;i++){
				Datas.ItemEnabled[i] = Reader.ReadBoolean();
				Scores.ClearTimeHighscore[i] = (float)Reader.ReadDouble();
				Scores.TotalHighscore[i] = Reader.ReadInt32();
			}
			Scores.CurrentStageID = Reader.ReadInt32();
			for(int i = 0;i <Costume.CostumeUnlock.Length; i++){
				Costume.CostumeUnlock[i] = Reader.ReadBoolean();
			}
			Scores.NowCostumeID = Reader.ReadInt32();
			for(int i = 0;i < Trophy.TrophyEnable.Length; i++){
				Trophy.TrophyEnable[i] = Reader.ReadBoolean();
				Trophy.TrophyClear[i] = Reader.ReadBoolean();
			}
			Scores.HaveCashes = Reader.ReadInt32();

			//Debug.Log("Load is completed.");
			
		}catch(Exception){
			Debug.Log("Load is error.");
			Options.BGMVolume = 1;
			Options.SEVolume = 1;
			Options.GameLevel = 0;
			Options.ItemOn = true;
			for(int i= 0;i < Datas.ItemID.Length ;i++){
				Datas.ItemEnabled[i] = false;
				Scores.ClearTimeHighscore[i] = 0f;
				Scores.TotalHighscore[i] = 0;
			}
			Datas.ItemEnabled[0] = true;
			Scores.CurrentStageID = 0;
			for(int i= 0;i < Costume.CostumeUnlock.Length; i++){
				Costume.CostumeUnlock[i] = false;
			}
			Costume.CostumeUnlock[0] = true;
			Scores.NowCostumeID = 0;
			for(int i = 0;i < Trophy.TrophyEnable.Length; i++){
				Trophy.TrophyEnable[i] = false;
				Trophy.TrophyClear[i] = false;
			}
			for(int i = 0;i < 2; i++){
				Trophy.TrophyEnable[i] = true;
				Trophy.TrophyClear[i] = true;
			}
			Scores.HaveCashes = 0;

		}
		finally{
			if(BinaryFile!=null){
			BinaryFile.Close();
			}

			if (Options.DebugMode == true){
				for(int i = 0;i < Datas.ItemID.Length ;i++){
					Datas.ItemEnabled[i] = true;
				}
				for(int i = 0;i <Costume.CostumeUnlock.Length; i++){
					Costume.CostumeUnlock[i] = true;
				}
				for(int i = 0;i < Trophy.TrophyEnable.Length; i++){
					Trophy.TrophyEnable[i] = true;
					Trophy.TrophyClear[i] = true;
				}
				Scores.HaveCashes = 99999999;
			}

		}
	}
	
	
}