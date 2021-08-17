using UnityEngine;
using System.Collections;
using System.Globalization;
using System;

public class HUD_GUIs : MonoBehaviour {

	public GUIText LifeBar = null;
	public GUIText BulletTBar = null;
	public GUIText EnemyKilled = null;
	public GUIText LifeBarText = null;
	public GUIText BulletTBarText = null;
	public GUIText EnemyKilledText = null;
	public GUIText ElapsedTimeText = null;
	private PlayerStatus PStatus = null;
	private EnemyCreat EC = null;
	private int BL = 0;
	private int LL = 0;
	private string BulletString = "";
	private string LifeString = "";
	private string EnemyString = "";
	private string ElapsedTimeString = "";
	
	// Use this for initialization
	void Start () {
		EC = GameObject.Find("EnemysScript").GetComponent<EnemyCreat>();
		PStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
		Refresh(false);
		TimeRefresh(0f);
	}
	
	// Update is called once per frame
	void Update () {
		BulletTBar.text = BulletString;
		LifeBar.text = LifeString;
		EnemyKilled.text = EnemyString;
		ElapsedTimeText.text = ElapsedTimeString;
		EnemyRef();
	}
	
	public void Refresh (bool ReloadFlag){
		BulletRef(ReloadFlag);
		LifeRef();
		EnemyRef();
	}
	
	public void BulletRef(bool ReloadFlag){
		BL = 0;
		BulletString = "";
		if (ReloadFlag == false){
			if (PStatus.BulletTime > 0){
				while(PStatus.BulletTime > BL){
					BulletString = BulletString + "|";
					BL = BL + 1;
				}
			}else{
					BulletString = "Please Reload!!!";
			}
		}else if(ReloadFlag == true){
			BulletString = "Now Reloading...";
		}
	}
	
	public void LifeRef(){
		LL = 0;
		LifeString = "";
		while(PStatus.PlayerLife > LL){
			LifeString = LifeString + "|";
			LL = LL + 1;
		}
	}
	
	public void EnemyRef(){
		EnemyString = EC.NowEnemys.ToString();
	}
	
	public void TimeRefresh(float ETime){
		int s = 0,m = 0,h = 0,s2 = 0,s2Leng = 2;
		string sZero,mZero,hZero,s2Zero,s2HeadCut;
		m = (int)ETime / 60;
		s = (int)ETime % 60;
		h = m / 60;
		m = m % 60;
		s2 = (int)Mathf.Round(ETime * 100f);
		
		
		if (s2 < 10){
			s2Zero = "0" + s2.ToString();
		}else{
			s2Leng = s2.ToString().Length;
			s2HeadCut = s2.ToString().Substring(s2Leng - 2,2);
			s2Zero = s2HeadCut;
		}
		if (s < 10){
			sZero = "0" + s.ToString();
		}else{
			sZero = s.ToString();
		}
		if (m < 10){
			mZero = "0" + m.ToString();
		}else{
			mZero = m.ToString();
		}
		if (h < 10){
			hZero = "0" + h.ToString();
		}else{
			hZero = h.ToString();
		}
		
		ElapsedTimeString = "Time:" + hZero + ":" + mZero + ":" + sZero + ":" + s2Zero;
	}
	
	public void GUIEnabled(bool GUIEn){
		LifeBar.enabled = GUIEn;
		BulletTBar.enabled = GUIEn;
		EnemyKilled.enabled = GUIEn;
		LifeBarText.enabled = GUIEn;
		BulletTBarText.enabled = GUIEn;
		EnemyKilledText.enabled = GUIEn;
		ElapsedTimeText.enabled = GUIEn;
	}
	
}
