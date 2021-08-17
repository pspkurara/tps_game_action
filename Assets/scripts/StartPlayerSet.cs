using UnityEngine;
using System.Collections;

public class StartPlayerSet : MonoBehaviour {

	private ScoreResult Score;
	private Costumes Costume;
	public Transform PlayerStartPosition;
	private GameObject PlayerObj;
	
	void Start () {
		Score = GameObject.Find("Scores").GetComponent<ScoreResult>();
		Costume = GameObject.Find("Scores").GetComponent<Costumes>();
		PlayerObj = Instantiate(Costume.CostumePrefab[Score.NowCostumeID],PlayerStartPosition.transform.position,PlayerStartPosition.transform.rotation) as GameObject;
		PlayerObj.name = "Player";
	}
}
