using UnityEngine;
using System.Collections;

public class CameraColorSet : MonoBehaviour {

	private Camera MainCam;
	private Color MainCameraColor = new Color(0,0,0,0);
	private StageColorPallet SCP;

	void Start () {
		if (GameObject.Find("GameMissionClearFlag")){
			SCP = GameObject.Find("GameMissionClearFlag").GetComponent<StageColorPallet>();
			MainCameraColor = SCP.InitMainCameraColor;
			MainCam = GetComponent<Camera>();
			MainCam.backgroundColor = MainCameraColor;
		}
	}
}
