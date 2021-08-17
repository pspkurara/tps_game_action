using UnityEngine;
using System.Collections;

public class RecoverItem : MonoBehaviour
{
	public int RecoverLevel = 10;
	private PlayerStatus PStatus;
	public AudioClip GetSound;
	private AudioSource PlayerAudio;
	private Animation Anim;
	public AnimationClip RotationAnim;
	private HUD_GUIs HUD;
	private ScoreResult Score;
	private PopupAddPointShow Pop;
	
	// Use this for initialization
	void Start ()
	{
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		HUD = GameObject.Find("GUIs").GetComponent<HUD_GUIs>();
		Anim = GetComponent<Animation>();
		PStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
		if (GameObject.Find("Player")){
			PlayerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
			Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		}
		Anim.Play(RotationAnim.name);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerAudio == null || Pop == null){
			if (GameObject.Find("Player")){
				PlayerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
				Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
			}
		}
	}
	
	void OnTriggerEnter(Collider Coli){
		if (Coli.gameObject.tag == "Player"){
			PStatus.PlayerLife += RecoverLevel;
			if (PStatus.PlayerLifeMax < PStatus.PlayerLife){
				int RecovePoint = (PStatus.PlayerLife - PStatus.PlayerLifeMax) * 100;
				Pop.InitSetting("+" + RecovePoint.ToString());
				Score.MainScore = Score.MainScore + RecovePoint;
				PStatus.PlayerLife = PStatus.PlayerLifeMax;
			}
			HUD.Refresh(false);
			if (PlayerAudio == null){
				if (GameObject.Find("Player")){
					PlayerAudio = GameObject.Find("Player").GetComponent<AudioSource>();
				}
			}else{
				PlayerAudio.PlayOneShot(GetSound);
			}
			Destroy(gameObject);
		}
	}
}

