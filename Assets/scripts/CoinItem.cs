using UnityEngine;
using System.Collections;

public class CoinItem : MonoBehaviour {

public int CoinPoint = 10;
	private ScoreResult Score;
	public AudioClip GetSound;
	private AudioSource PlayerAudio;
	private Animation Anim;
	public AnimationClip RotationAnim;
	private PopupAddPointShow Pop;
	
	// Use this for initialization
	void Start ()
	{
		Anim = GetComponent<Animation>();
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
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
			Score.MainScore += CoinPoint;
			Pop.InitSetting("+" + CoinPoint.ToString());
			PlayerAudio.PlayOneShot(GetSound);
			Destroy(gameObject);
		}
	}
}
