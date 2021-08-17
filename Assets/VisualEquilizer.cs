using UnityEngine;
using System.Collections;

public class VisualEquilizer : MonoBehaviour {

	float[] EqBar;
	public GameObject BackEQImage;
	public float MaxDistance = 2f;

	//AudioSource Audio;
	void Start () {
		//Audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//float[] EqBar = Audio.GetSpectrumData(5000,0,FFTWindow.Blackman);
		//print (EqBar[0]);
		//float BarDistance = MaxDistance/(EqBar[0]/100);
		//BackEQImage.transform.localScale = new Vector3(BackEQImage.transform.localScale.x,BarDistance,BackEQImage.transform.localScale.z);
	}
}
