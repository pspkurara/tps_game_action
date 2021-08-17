using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class VolumeSet : MonoBehaviour {
	
	AudioSource Audios;
	private OptionSetting Options;
	
	void Start () {
		Audios = GetComponent<AudioSource>();
		Options = GameObject.Find("Settings").GetComponent<OptionSetting>();
		Audios.volume = Options.SEVolume;
	}
}
