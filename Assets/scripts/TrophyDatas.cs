using UnityEngine;
using System.Collections;

public class TrophyDatas : MonoBehaviour {

	public bool[] TrophyClear = {false};
	public string[] TrophyName = {"New Trophy"};
	public string[] TrophyLegend = {"New Trophy Legend"};
	public string[] TrophyPrize = {"Cash: $1000"};
	public bool[] TrophyEnable = {false};

	
	public void TrophyGetted(int index){
		if (TrophyClear[index] == true){
			print("[" + TrophyName + "](" + index.ToString() + ")は、すでに獲得済みのトロフィーです。");
			return;
		}
		TrophyClear[index] = true;
		switch(index){
		case 0:
			
			break;
		case 1:
			
			break;
		case 2:

			break;
		case 3:

			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		}	
	}
}