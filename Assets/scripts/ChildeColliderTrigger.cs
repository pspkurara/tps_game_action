using UnityEngine;
using System.Collections;

public class ChildeColliderTrigger : MonoBehaviour {

	private ColliderTriggerParent colliderTriggerParent;

    // Use this for initialization
	void Start () {
		GameObject objColliderTriggerParent = gameObject.transform.parent.gameObject;
		colliderTriggerParent = objColliderTriggerParent.GetComponent<ColliderTriggerParent>();
    }

    void OnTriggerEnter(Collider collider){
        colliderTriggerParent.RelayOnTriggerEnter(collider);
    }
	
	void OnTriggerExit(Collider collider){
        colliderTriggerParent.RelayOnTriggerExit(collider);
    }

    // Update is called once per frame
    void Update () {

    }
}