using UnityEngine;
using System.Collections;

public class ColliderTriggerParent : MonoBehaviour {
	
	private EnemyCreateZoneVer CColT;
	public bool EnterMode = true;
	public bool ExitMode = true;
	public bool EnterisDestroy = false;
	public bool ExitisDestroy = false;
    void Start () {
		GameObject objColliderTriggerParent = gameObject.transform.parent.gameObject;
		CColT = objColliderTriggerParent.GetComponent<EnemyCreateZoneVer>();
    }

	public void RelayOnTriggerEnter(Collider collider){
		if (EnterMode == true){
        	CColT.RelayOnTriggerEnter(collider);
			if (EnterisDestroy == true){
				Destroy(gameObject);
			}
		}
    }
	
	public void RelayOnTriggerExit(Collider collider){
		if (ExitMode == true){
        	CColT.RelayOnTriggerExit(collider);
			if (ExitisDestroy == true){
				Destroy(gameObject);
			}
		}
    }
}