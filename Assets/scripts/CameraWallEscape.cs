using UnityEngine;
using System.Collections;

public class CameraWallEscape : MonoBehaviour {

	public Transform tg;//対象となるオブジェクト
	
	public LayerMask layerMask;
	
	public float dist = 3.0f; //カメラからオブジェクトの距離
	public float fastness = 1.0f; //カメラの追従速度
	public float cameraHeight = 2.0f; //カメラの高さ
	
	Vector3 nextLoc; //次にカメラが移動すべき目的点
	Vector3 FinalPosition;
	
	void Start(){
		if (GameObject.Find("Player")){
			tg = GameObject.Find("PlayerHead").GetComponent<Transform>();
		}
	}
	
	//カメラの更新はLateUpdateを用いる
	void LateUpdate() {
		if (tg == null){
			if (GameObject.Find("Player")){
				tg = GameObject.Find("PlayerHead").GetComponent<Transform>();
			}
		}else{
			//オブジェクトの位置を次の仮カメラ位置とする
			nextLoc = tg.transform.position;
			
			//distで指定された距離(オブジェクトの背面方向へ)離れる
			Vector3 dir = tg.TransformDirection(Vector3.forward);
			dir = dir.normalized * dist;
			nextLoc -= dir;
			
		
			//オブジェクトから後方に離れた位置を候補とする
			Vector3 candidate = nextLoc;
			//レイキャストを用いてオブジェクトとカメラの間にある壁を判定
			//壁は"Wall"レイヤに属しているものとする
			float dd = Vector3.Distance(tg.transform.position, candidate);
			Vector3 dir2 = candidate -tg.transform.position;
			RaycastHit Hit;
			if (Physics.Raycast(tg.transform.position , dir2,out Hit, dd, layerMask)){
				Debug.DrawLine (tg.transform.position, Hit.point, Color.red); //対象物と壁に衝突した位置の間に線を引く
				nextLoc = Hit.point; //衝突位置を次の位置とする
			}
			nextLoc.y += cameraHeight; //カメラ高さを設定
			
			//Vector3 d = nextLoc - transform.position; //現カメラ位置から次位置へのカメラ移動量を算出
			
			//d *= Time.deltaTime*fastness; //速度と経過時間でカメラ移動量を決定
			
			Vector3 looks = tg.transform.position - transform.position; //対象と現在のカメラの距離を算出.
			looks.y = 0f;
			float StartDistance = looks.magnitude; //距離を算出.
			Vector3 lastlooks = tg.transform.position - nextLoc; //対象と目的地のカメラの距離を算出.
			lastlooks.y = 0f;
			float LastDistance = lastlooks.magnitude; //距離を算出.
			float Times = Time.deltaTime*fastness;
			
			float FinalDistance = Mathf.Lerp(StartDistance,LastDistance,Times); //Times分の間の距離を補間.
	
			//print ("DST:"+FinalDistance);
			
			float Angles = Mathf.LerpAngle(transform.eulerAngles.y,tg.transform.eulerAngles.y,Times); //Times分の間の向きを補完.
			
			//print (":"+transform.rotation.y +"/"+tg.transform.rotation.y+" / "+Angles);
			
			Quaternion FinalAngle = Quaternion.Euler(0f,Angles,0f); //Quaternionを算出.
			
			Vector3 FinalPosition1 = tg.transform.position - FinalAngle * Vector3.forward * Mathf.Clamp(FinalDistance,0f,dist); //最終的な位置を算出.
			//print (":"+transform.position.x +"/"+tg.transform.position.x+" / "+FinalPosition.x);
			
			FinalPosition1.y = cameraHeight + tg.transform.position.y;
			
			FinalPosition = FinalPosition1;
			
			dd = Vector3.Distance(transform.position, nextLoc);
			Vector3 Normaling = FinalPosition1 - transform.position;
			Normaling.Normalize();
			if (Physics.Raycast(transform.position , Normaling , out Hit, dd, layerMask)){
				//Vector3 StartPoint = FinalPosition1 - Hit.point;
				//Vector3 RefrectPoint = Vector3.Reflect(StartPoint,Hit.normal);
				//Vector3 LastPoint = ((StartPoint + RefrectPoint) / 2f);
				//FinalPosition = Hit.point + LastPoint;
				//FinalPosition.y = cameraHeight + tg.transform.position.y;
				
				Vector3 d = nextLoc - transform.position; //現カメラ位置から次位置へのカメラ移動量を算出
				
				d *= Time.deltaTime*fastness; //速度と経過時間でカメラ移動量を決定
				
				FinalPosition = d + transform.position;
				
			}
	
			transform.position = FinalPosition; //カメラ位置を変更
			transform.LookAt(tg); //オブジェクトの方向にカメラを向ける
		}
	}
}
