using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	
	public GameObject BulletFolderSource;
	private GameObject BulletFolder;
	// Use this for initialization
	void Start () {
		BulletFolder = Instantiate(BulletFolderSource) as GameObject;
		BulletFolder.name = BulletFolderSource.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
