using UnityEngine;
using System.Collections;

public class AnimationEndFlag : MonoBehaviour {

	public bool AnimEndFlag = false;
	
	void AnimationEnd(){
		AnimEndFlag = true;
	}
}
