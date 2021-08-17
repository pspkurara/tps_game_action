using UnityEngine;
using System.Collections;

public class Text : MonoBehaviour {
	
	public GUIText BaseGT = null;
	public TextMesh BaseTM = null;
	private TextMesh Tex;
	private MeshRenderer MeshTex,MeshMy = null;
	
	void Update () {
		if(BaseGT != null){
			guiText.text = BaseGT.text;
			guiText.enabled = BaseGT.enabled;
		}
		if(BaseTM != null){
			Tex = GetComponent<TextMesh>();
			MeshMy = GetComponent<MeshRenderer>();
			MeshTex = Tex.gameObject.GetComponent<MeshRenderer>();
			Tex.text = BaseTM.text;
			Tex.color = new Color (Tex.color.r,Tex.color.g,Tex.color.b,BaseTM.color.a);
			Tex.lineSpacing = BaseTM.lineSpacing;
			MeshMy.enabled = MeshTex.enabled;
		}
	}
}
