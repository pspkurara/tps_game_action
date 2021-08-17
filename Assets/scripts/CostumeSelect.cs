using UnityEngine;
using System.Collections;

public class CostumeSelect : MonoBehaviour {
	
	public FadeInFadeOut Fadein, Fadeout;
	private ScoreResult Scores;
	private Commands Command;
	public GUIText TitleText;
	public GUIText LegText;
	public GUITexture CL, CR;
	private Costumes CostumeDatas;
	private int NowStatus = 0;
	private bool OnControll = false;
	private int NowSelectCostume = 0;
	public Transform Position;
	private GameObject NowSelectCostumePrefab;
	private AnimationState RotateAnimState;
	public AnimationClip RotationAnim;
	public AudioClip EnterSound;
	public AudioClip BackSound;
	public AudioClip ErrorSound;
	public AudioClip SelectSound;
	private AudioSource SESource;
	private SaveScript Save;
	public GameObject Locks;
	private BGMPlay BGM;
	
	void Start () {
		Save = GetComponent<SaveScript>();
		Save.Load();
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		SESource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		Fadein = GameObject.Find("FadeIn").GetComponent<FadeInFadeOut>();
		Fadeout = GameObject.Find("FadeOut").GetComponent<FadeInFadeOut>();
		CostumeDatas = GameObject.Find("Scores").GetComponent<Costumes>();
		Scores = GameObject.Find("Scores").GetComponent<ScoreResult>();
		BGM = GameObject.Find("BGM").GetComponent<BGMPlay>();
		RotateAnimState = Position.animation[RotationAnim.name];
		NowSelectCostume = Scores.NowCostumeID;
		OnControll = false;
		TitleText.text = "";
		LegText.text = "";
		if (NowSelectCostume == CostumeDatas.CostumeName.Length){
			CL.enabled = false;
		}else if (NowSelectCostume == 0){
			CR.enabled = false;
		}
		BGM.BGMPlaying();
	}

	void Update () {
		switch(NowStatus){
		case 0: //Start
			if (Fadein.EndFlag == true){
				NowStatus = 1;
				OnControll = true;
				SetPlayerCharacter(NowSelectCostume);
			}
			break;
		case 1:
			MainSelection();
			break;
		case 2: //Back
			if (Fadeout.EndFlag == true){
				Application.LoadLevel("Reword");
			}
			break;
		}
	}
	
	void MainSelection(){
		if (Input.GetButtonDown(Command.DPadLeftCommand) && OnControll == true){
			if (NowSelectCostume >= CostumeDatas.CostumeName.Length -1){
				CL.enabled = false;
				SESource.PlayOneShot(ErrorSound);
				NowSelectCostume = CostumeDatas.CostumeName.Length - 1;
			}else{
				SESource.PlayOneShot(SelectSound);
				if (NowSelectCostume == CostumeDatas.CostumeName.Length -1){
					CL.enabled = false;
				}else{
					CL.enabled = true;
				}
				NowSelectCostume++;
				SetPlayerCharacter(NowSelectCostume);
			}
			
		}else if (Input.GetButtonDown(Command.DPadRightCommand) && OnControll == true){
			if (NowSelectCostume <= 0){
				CR.enabled = false;
				SESource.PlayOneShot(ErrorSound);
				NowSelectCostume = 0;
			}else{
				SESource.PlayOneShot(SelectSound);
				NowSelectCostume--;
				SetPlayerCharacter(NowSelectCostume);
			}
		}
		if (NowSelectCostume == CostumeDatas.CostumeName.Length -1){
			CL.enabled = false;
		}else{
			CL.enabled = true;
		}
		if (NowSelectCostume == 0){
			CR.enabled = false;
		}else{
			CR.enabled = true;
		}
		if (CostumeDatas.CostumeUnlock[NowSelectCostume] == false){
			Locks.SetActive(true);
		}else{
			Locks.SetActive(false);
		}
		
		if (Input.GetButtonDown(Command.EnterCommand)){
			if (CostumeDatas.CostumeUnlock[NowSelectCostume] == false){
				SESource.PlayOneShot(ErrorSound);
			}else{
				OnControll = false;
				SESource.PlayOneShot(EnterSound);
				Scores.NowCostumeID = NowSelectCostume;
				Save.Save();
				NowStatus = 2;
				Fadeout.StartFlag = true;
				BGM.FadeOutBG();
			}
		}else if(Input.GetButtonDown(Command.CancelCommand)){
			OnControll = false;
			SESource.PlayOneShot(BackSound);
			NowStatus = 2;
			Fadeout.StartFlag = true;
			BGM.FadeOutBG();
		}
	}
	
	void SetPlayerCharacter(int PlayerID){
		if (NowSelectCostumePrefab != null){
			Destroy(NowSelectCostumePrefab);
		}
		NowSelectCostumePrefab = Instantiate(CostumeDatas.CostumePrefab[PlayerID],Position.transform.position,Position.transform.rotation) as GameObject;
		NowSelectCostumePrefab.transform.parent = Position.transform;
		ThirdPersonController TPC = NowSelectCostumePrefab.GetComponent<ThirdPersonController>();
		TPC.isControllable = false;
		TPC._characterState = ThirdPersonController.CharacterState.Idle;
		RotateAnimState.time = 0;
		SetTexts(PlayerID);
	}
	
	void SetTexts(int PlayerID){
		TitleText.text = CostumeDatas.CostumeName[PlayerID];
		LegText.text = CostumeDatas.CostumeLegend[PlayerID];
	}
}
