using UnityEngine;
using System.Collections;

public class BulletMaxCheck : MonoBehaviour {
	
	ArrayList BulletTimes = new ArrayList();
	public int MaxBullet = 10;
	
	void Start () {
		MaxBullet = 10;
	}
	
	public void BulletInit(int BulletMaxTimes){
		MaxBullet = BulletMaxTimes;
	} 
	
	public void AddBullet(GameObject Bullet){
		if (BulletTimes.Count >= MaxBullet){
		//while(BulletTimes.Count > MaxBullet){
			//print("remo");
			int BulletCounts = BulletTimes.Count - MaxBullet;
			for(int i = 0;i <= BulletCounts; i++){
				Destroy((GameObject)BulletTimes[0]);
				BulletTimes.RemoveAt(0);
			}
		}
		//print("add" + BulletTimes.Count);
		BulletTimes.Add(Bullet);
	}
	
	public void RemoveBullet(GameObject Bullet){
		BulletTimes.Remove(Bullet);
	}
}
