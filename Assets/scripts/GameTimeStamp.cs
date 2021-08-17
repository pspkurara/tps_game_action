using UnityEngine;
using System.Collections;

public class GameTimeStamp : MonoBehaviour {
	
	private float timestamp;
	private float EnemyCreatestamp;
	private EnemyCreat EnemyC;
	private PopupAddPointShow Pop;

	void Start () {
		timestamp = Time.timeScale;
		EnemyC = GameObject.Find("EnemysScript").GetComponent<EnemyCreat>();
		if (GameObject.Find("PlayerPop")){
			Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
		}
		EnemyCreatestamp = EnemyC.EnemySpeed;
	}
	
	void Update (){
		if (Pop != null){
			Pop = GameObject.Find("PlayerPop").GetComponent<PopupAddPointShow>();
			if (Time.timeScale == 0f){
				Pop.OnTimeStamp = true;
			}else{
				Pop.OnTimeStamp = false;
			}
		}
	}
	
	public void StopTimeStamp(){
		Time.timeScale = 0f;
		EnemyC.EnemySpeed = 0f;
		if (Pop != null){
			Pop.OnTimeStamp = true;
		}
	}
	
	public void StartTimeStamp(){
		Time.timeScale = timestamp;
		EnemyC.EnemySpeed = EnemyCreatestamp;
		if (Pop != null){
			Pop.OnTimeStamp = false;
		}
	}
	
	public void ChangeTimeStamp(float EditTime, float EditEnemySpeed, float EditSlideSpeed, float EditColorSpeed){
		Time.timeScale = EditTime;
		EnemyC.EnemySpeed = EditEnemySpeed;
		Pop.SlideSpeed = EditSlideSpeed;
		Pop.ColorChangeSpeed = EditColorSpeed;
	}
	
	public void SpritTimeStamp(){
		Time.timeScale = timestamp / 2f;
		EnemyC.EnemySpeed = EnemyCreatestamp / 2f;
		Pop.SlideSpeed = Pop.SlideSpeed / 2f;
		Pop.ColorChangeSpeed = Pop.ColorChangeSpeed / 2f;
	}

}
