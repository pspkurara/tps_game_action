using UnityEngine;
using System.Collections;

public class DestroiAttackScript : MonoBehaviour {
	
	private int DamageLevel = 5;
	private DamageScript DScr;
	private EnemyFollow EF;
	private ScoreResult SR;
	private PlayerStatus Pstatus;
	private PopupAddPointShow Pop;
	public float BombRadius = 1f;
	// Use this for initialization
	void Start () {
		SR = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		Pstatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DamageLevelSet(int DamageLevelSetting){
		DamageLevel = DamageLevelSetting;
	}
	
	void SRayHit(){
		RaycastHit hit;
		if (Physics.SphereCast(transform.position, BombRadius, transform.up, out hit, 1f)){
			Destruction(hit.collider);
		}
	}

	void Destruction(Collider coli){
		if (coli.gameObject.tag == "Player"){
			DScr = coli.gameObject.GetComponent<DamageScript>();
			Pstatus.NoDamageFlag = false;
			DScr.DamageHit(DamageLevel);
		}
		if (coli.gameObject.tag == "Enemy01"){
			EF = coli.gameObject.transform.parent.gameObject.GetComponent<EnemyFollow>();
			if (EF != null){
			EF.EnemyDamage(DamageLevel);
			}
			if(EF.EnemyLife <= 0){
				Pop.InitSetting("+" + (EF.EnemyDestroiAttackBonusScore + EF.EnemyKilledScore).ToString());
				SR.EntrainmentEnemyKillBonus += EF.EnemyDestroiAttackBonusScore;
				SR.MainScore += EF.EnemyKilledScore;
				SR.NowMissionKilledEnemys++;
			}
		}
	}
}
