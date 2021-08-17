using UnityEngine;
using System.Collections;

public class CarStatus : MonoBehaviour {
	
	public int CarLife = 1;
	public GameObject ZanryuObject = null;
	public int DamageLevel = 5;
	public int BonusScorePoint = 100;
	public GameObject CarExplotionEffect = null;
	private DestroiAttackScript DSC;
	private ScoreResult Score;
	private PopupAddPointShow Pop;
	
	
	// Use this for initialization
	void Start () {
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		if (GameObject.Find("Player")){
			Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Pop == null){
			if (GameObject.Find("Player")){
				Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
			}
		}
	}
	
	public void CarHit(int HitLevel,Vector3 pos1,GameObject MetalEffectPrefab){
		CarLife = CarLife - HitLevel;
		Instantiate(MetalEffectPrefab,pos1,transform.rotation);
			if (CarLife <= 0){
				Instantiate(CarExplotionEffect,transform.position,transform.rotation);
				if (ZanryuObject == true){
					Instantiate(ZanryuObject,gameObject.transform.position,gameObject.transform.rotation);
				}
				Pop.InitSetting("+" + BonusScorePoint.ToString());
				Score.ObjectDestructionBonus += BonusScorePoint;
				DSC = CarExplotionEffect.GetComponent<DestroiAttackScript>();
				DSC.DamageLevelSet(DamageLevel);
				Destroy(gameObject);
			}
	}
	
}
