using UnityEngine;
using System.Collections;

public class NowEnemyPlayScream : MonoBehaviour {

	private AudioSource Audio;
	private EnemyCreat Ecreate;

	void Start () {
		Ecreate = GameObject.Find("EnemysScript").GetComponent<EnemyCreat>();
		Audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Ecreate.NowEnemys > 0) {
						Audio.mute = false;
				} else {
						Audio.mute = true;
				}
	}
}
