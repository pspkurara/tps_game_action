using UnityEngine;
using System.Collections;

public class AnimEndFlag : MonoBehaviour {

	public bool AnimStartFlag = true;
	public bool AnimEndedFlag = false;
	private AnimationState Animstate;
	AnimationClip AnimClip;

	void Start(){
		if (animation.playAutomatically == true){
			AnimStartFlag = true;
		}
		AnimClip = this.gameObject.animation.clip;
		Animstate = animation[AnimClip.name];
	}


	void StartAnimPlay(){
		AnimStartFlag = true;
	}

	void EndAnimPlay(){
		AnimEndedFlag = true;

	}

	public void AnimGoLastFrame(){
		if (AnimEndedFlag == false){
			Animstate.time = Animstate.length;
		}
	}
}
