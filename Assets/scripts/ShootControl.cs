using UnityEngine;
using System.Collections;

public class ShootControl : MonoBehaviour {
	
	public AnimationClip GunDown = null;
	public AnimationClip GunShot = null;
	public AnimationClip GunIdol = null;
	public AnimationClip GunUp = null;
	public AnimationClip GunReload = null;
	public GameObject BulletPrefab = null;
	public GameObject BulletFXPrefab = null;
	public GameObject FireEffectPrefab = null;
	public AudioClip BulletSound = null;
	public AudioClip BulletNFSound = null;
	
	//public AudioClip CarExplotion = null;
	
	public Transform GunMouth = null;
	public float bulletspeed = 500;
	public float BulletHeight = 0.95f;
	private float BulletDelay = 0f;
	public float ScorpLang = 0.0f;
	//public Camera FPSCamera = null;
	//public GUITexture Cursol = null;
	public Projector[] CursolLight = null;
	
	public AudioClip[] ReloadSounds = null;

	public int AnimLayer = 2;
	
	public int GunPosFlag = 0;
	
	public GameObject DefaultEffectPrefab = null;
	public GameObject RockEffectPrefab = null;
	public GameObject WhiteRockEffectPrefab = null;
	public GameObject GrassEffectPrefab = null;
	public GameObject DirtEffectPrefab = null;
	public GameObject TestPrefab = null;
	public GameObject MetalEffectPrefab = null;
	public GameObject WoodEffectPrefab = null;
	public GameObject DarkWoodEffectPrefab = null;
	public GameObject BlueRockEffectPrefab = null;
	public GameObject CarExplotionEffect = null;
	public GameObject TankBoomEffect = null;
	public GameObject BombEffect = null;
	private Animation _animation;
	private PlayerStatus PStatus;
	private HUD_GUIs HG;
	private int GunPosSave = 0;
	private bool StopFlag = false;
	private GUI_Yurasi GYurashi1,GYurashi2;
	public float GunPowers = 3.0f;
	
	public bool isDamageFlag = false;
	
	private MeshRenderer MRendeler;
	private ScoreResult Score;
	private BoxCollider BoxColi;
	private EnemyFollow Efollow;
	private Commands Command;
	private PopupAddPointShow Pop;
	public bool isControl = true;
	public bool ReloadOnFlag = true;
	private BulletScript BS;
	private BulletMaxCheck BMC;
	public int BulletMax = 10;

	void Start () {
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		PStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
		if (GameObject.Find("GUIs")){
			HG = GameObject.Find("GUIs").GetComponent<HUD_GUIs>();
		}
		if (HG != null){
			GYurashi1 = GameObject.Find("BulletsT").GetComponent<GUI_Yurasi>();
			GYurashi2 = GameObject.Find("Ammo").GetComponent<GUI_Yurasi>();
		}
		_animation = GetComponent<Animation>();
		_animation[GunIdol.name].wrapMode = WrapMode.Loop;
		_animation[GunIdol.name].layer = AnimLayer;
		_animation[GunShot.name].wrapMode = WrapMode.Once;
		_animation[GunShot.name].layer = AnimLayer;
		_animation[GunDown.name].wrapMode = WrapMode.Once;
		_animation[GunDown.name].layer = AnimLayer;
		//_animation[GunUp.name].wrapMode = WrapMode.Once;
		_animation[GunUp.name].layer = AnimLayer;
		_animation[GunReload.name].layer = AnimLayer;
		//_animation[GunReload.name].wrapMode = WrapMode.Once;
		//コマンドを読み込み.
		Command = GameObject.Find("Commands").GetComponent<Commands>();
		Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		BS = GameObject.Find("PlayerStatus").GetComponent<BulletScript>();
		if (GameObject.Find(BS.BulletFolderSource.name)){
			if (GameObject.Find(BS.BulletFolderSource.name)){
				BMC = GameObject.Find(BS.BulletFolderSource.name).GetComponent<BulletMaxCheck>();
				if (BMC != null){
					BMC.BulletInit(BulletMax);
				}
			}
		}
		GunIdolAnim();
		GunPosFlag = 0;
		int CC = 0;
		while(CC <= 2){
			CursolLight[CC].enabled = false;
			CC ++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDamageFlag){
			if (Input.GetButtonDown(Command.GunReloadCommand) && StopFlag == false && isControl == true && ReloadOnFlag == true){
				if (HG != null){
					HG.Refresh(true);
				}
				StopFlag = true;
				GunPosSave = GunPosFlag;
				GunPosFlag = 2;
				GunReloadAnim();
			}else if (StopFlag == false){
				if (Input.GetButton(Command.GunSetupCommand) && isControl == true){
					GunUpAnim();
					GunPosFlag = 1;
					//StopCoroutine ("DownGun");
					if (Input.GetButtonDown(Command.GunShotCommand) && GunPosFlag == 1 && GunPosFlag == 1 && isControl == true && GameObject.Find(BS.BulletFolderSource.name)){
						if (PStatus.BulletTime > 0){
							PStatus.BulletTime = PStatus.BulletTime - 1;
							if (HG != null){
								HG.Refresh(false);
							}
							if (HG != null){
								GYurashi1.Yure();
							}
							audio.PlayOneShot(BulletSound);
							if (BMC == null){
								BMC = GameObject.Find(BS.BulletFolderSource.name).GetComponent<BulletMaxCheck>();
								BMC.BulletInit(BulletMax);
							}
							GameObject BulletFolder;
							BulletFolder = Instantiate(BulletFXPrefab,GunMouth.transform.position,transform.rotation) as GameObject;
							BulletFolder.transform.parent = GameObject.Find(BS.BulletFolderSource.name).transform;
							BMC.AddBullet(BulletFolder);
							GunShotAnim();
							Vector3 pos = transform.position;
							RaycastHit hit;
				      		if (Physics.Raycast(pos + transform.rotation * new Vector3(0f,BulletHeight,BulletDelay), transform.rotation * Vector3.forward, out hit, Mathf.Infinity) && ScorpLang == 0 || Physics.Raycast(pos + transform.rotation * new Vector3(0f,BulletHeight,0f), transform.rotation * Vector3.forward, out hit, ScorpLang) && ScorpLang >= 0.1f){
								Vector3 pos1 = hit.point;
								Collider coli = hit.collider;
								HitColis(coli,pos1);
							}
						}else{
							audio.PlayOneShot(BulletNFSound);
							if (HG != null){
								GYurashi1.Yure();
								GYurashi2.Yure();
							}
						}
					}
				}else{
					// if(GunPosFlag == 0){
					//if (GunPosFlag == 1){
						//GunDownAnim();	
					//}
					GunIdolAnim();
					GunPosFlag = 0;
				}
			}
		}
		if (Input.GetButton(Command.GunSetupCommand) && isControl == true){
			CursorSet(true);
		}else{
			CursorSet(false);
		}
		
	}
	
	void CursorSet(bool CE){
		int CC = 0;
		while(CC <= 2){
			CursolLight[CC].enabled = CE;
			CC ++;
		}
		CC = 0;
		//Cursol.enabled = CE;
		//FPSCamera.enabled = CE;
	}
	
	//IEnumerator DownGun () {
	//	_animation.PlayQueued(GunShot.name,QueueMode.CompleteOthers, PlayMode.StopSameLayer);
	//	yield return new WaitForSeconds(0.2f);
		//_animation.PlayQueued(GunDown.name,QueueMode.CompleteOthers, PlayMode.StopSameLayer);
	//	_animation.CrossFadeQueued(GunIdol.name,0.5f,QueueMode.CompleteOthers);
	//}
	
	void GunDownAnim(){
		_animation.CrossFadeQueued(GunIdol.name,0.5f,QueueMode.CompleteOthers);
	}
	
	void GunIdolAnim(){
		_animation.CrossFade(GunIdol.name,0.2f);
	}
	
	void GunUpAnim(){
		//_animation.CrossFadeQueued(GunUp.name,0.5f,QueueMode.CompleteOthers);
		_animation.CrossFade(GunUp.name,0.2f);
		
	}
	
	void GunReloadAnim() {
		_animation.PlayQueued(GunReload.name,QueueMode.PlayNow, PlayMode.StopSameLayer);
	}
	
	void GunShotAnim(){
		_animation.PlayQueued(GunShot.name,QueueMode.PlayNow, PlayMode.StopSameLayer);
	}
	
	void GunReloadOver(){
		PStatus.BulletTime = PStatus.BulletTimeDefault;
		if (HG != null){
			HG.Refresh(false);
		}
		GunPosFlag = GunPosSave;
		if (GunPosFlag == 1){
			GunUpAnim();
		}else if (GunPosFlag == 0){
			GunDownAnim();
			//GunIdolAnim();
		}
		StopFlag = false;
	}
	
	void StopFlagOn(){
		StopFlag = true;
	}
	
	void ReloadSound(AudioClip Sizes){
		audio.PlayOneShot(Sizes);
	}
	
	void HitColis(Collider coli, Vector3 pos1){
		Rigidbody Rig = coli.gameObject.rigidbody;
		if (Rig != null){
			Rig.AddForce((new Vector3(180f,180f,180f) - pos1) * GunPowers,ForceMode.Impulse);
		}
		if (coli.gameObject.tag == "Rock"){
			Instantiate(RockEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "WhiteRock"){
			Instantiate(WhiteRockEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Grass"){
			Instantiate(GrassEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Dirt"){
			Instantiate(DirtEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Metal"){
			Instantiate(MetalEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Wood"){
			Instantiate(WoodEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "DarkWood"){
			Instantiate(DarkWoodEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "BlueRock"){
			Instantiate(BlueRockEffectPrefab,pos1,transform.rotation);
		}else if (coli.gameObject.tag == "Effect" || coli.gameObject.tag == "Player" || coli.gameObject.tag == "Bullet"){
		}else if (coli.gameObject.tag == "Enemy01"){
			Enemy01Die(coli.gameObject,pos1);
		}else if (coli.gameObject.tag == "Cars" || coli.gameObject.tag == "OilTanks"){
			CilldCollision CS = coli.gameObject.GetComponent<CilldCollision>();
			CS.ColliHit(1,pos1,MetalEffectPrefab);
		}else if (coli.gameObject.tag == "DestructibleObjects"){
			DObject DObj = coli.gameObject.GetComponent<DObject>();
			DObj.HitObj(1,pos1);
			
		}else{
			//Instantiate(DefaultEffectPrefab,pos1,transform.rotation);
		}
		if (coli.gameObject.tag != "Enemy01"){
			//Instantiate(FireEffectPrefab,pos1 + new Vector3(0f,Random.Range(-30.0f,30.0f) / 10f,0f),transform.rotation);
			Instantiate(FireEffectPrefab,pos1,transform.rotation);
		}
	}
	
	public void Enemy01Die(GameObject GObj , Vector3 pos1){
		Instantiate(BlueRockEffectPrefab,pos1,transform.rotation);
		Efollow = GObj.transform.parent.gameObject.GetComponent<EnemyFollow>();
		Efollow.EnemyDamage(1);
		if (Efollow.EnemyLife <= 0){
			Pop.InitSetting("+" + Efollow.EnemyKilledScore.ToString());
			Score.MainScore = Score.MainScore + Efollow.EnemyKilledScore;
			Score.NowMissionKilledEnemys = Score.NowMissionKilledEnemys + 1;
			Score.TotalKilledEnemys = Score.TotalKilledEnemys + 1;
			if (HG != null){
				HG.Refresh(false);
			}
		}
	}
	
}