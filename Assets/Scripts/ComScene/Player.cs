using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour {

	Dictionary<string, GameObject> fukidashiDic;
    GameObject playerSpine;
	GameObject delightedEffect;
	GameObject dEffctRs;
	GameObject ball;
	SkeletonAnimation playerAnimation;
	public float speed = 2f;
	public float arrowWidth = 2f;
	public float arrowHeight = 2f;

	[SerializeField]
	int placeRow;
	[SerializeField]
	int placeColumn;
	int state; //0: 何もしない. 1: 向かい合う
    int arrowSpeed = 0;

	public int PlaceRow { get{ return placeRow; } set{ placeRow = value; }}
	public int PlaceColumn { get{ return placeColumn; } set{ placeColumn = value; }}
	public int GetState {get { return state;} }


	void Start () {
		//生成時に透明→FadeIn
        playerSpine = gameObject.transform.Find("PlayerSpine").gameObject;
		dEffctRs = Resources.Load ("Prefabs/ComScene/DelightedEffect") as GameObject;
		ball = GameObject.FindGameObjectWithTag ("Ball");
		playerAnimation = playerSpine.GetComponent<SkeletonAnimation> ();
		playerAnimation.skeleton.a = 0f;
		playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Idle", true);
		state = 0; 

		//吹き出し取得
		fukidashiDic = new Dictionary<string, GameObject> ();
		Transform[] objs = GetComponentsInChildren<Transform>();
		foreach (Transform obj in objs) {
			fukidashiDic.Add (obj.name, obj.gameObject);
			if (obj.name == "fukidasi-eating" || obj.name == "fukidasi-talking" || obj.name == "fukidasi-enjoying") {
				fukidashiDic [obj.name].SetActive (false);
			}
		}

		//裏だったら横を向く
		if (playerSpine != null && GetComponent<Furniture> ().IsReverse) {
			playerSpine.transform.rotation = Quaternion.Euler(90f, 0.0f, 0);
			playerSpine.transform.localScale = new Vector3(-0.5f, playerSpine.transform.localScale.y, playerSpine.transform.localScale.z);
		}

    }


	void FixedUpdate () {
		if (playerAnimation.skeleton.a < 1.0f) {
			playerAnimation.skeleton.a += 0.05f;
		}
	}


    //アニメーションのstate
    //0: Idle - 立ち
    //1: Chat - 吹き出し
	//2: Delighted - 回転
	//3: 飯を食べる
	//4: テレビをみる
	//5: ボールで遊ぶ
	public bool SpineState(int st) {
		if (playerSpine != null && state != st) {
			if (st == 0) {
				state = 0;
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Idle", true);
				DestroyDelEffect ();
				DeleteFukidashi ();
				return true;
			}
			if (st == 1) {   
				state = 1;
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Chat", true);
				DestroyDelEffect ();
				ShowFukidashi ("fukidasi-talking");
				return true;
			}
			if (st == 2) {	 
				state = 2;
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Delighted", true);
				CreateDelEffect ();
				return true;
			}
			if (st == 3) {
				state = 3;
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Eat", true);	//Eatモーションがない為Delightedで代用
				//playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Delighted", true);
				CreateDelEffect ();
				ShowFukidashi ("fukidasi-eating");
				return true;
			}
			if (st == 4) {
				state = 4;
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Watch", true);		//Watchモーションがない為Delightedで代用
				//playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Delighted", true);
				CreateDelEffect ();
				return true;
			}
			if (st == 5) {
				state = 5;
				//playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Enjoying", true);
				playerSpine.GetComponent<SkeletonAnimation> ().state.SetAnimation (0, "Delighted",true);
				CreateDelEffect ();
				ShowFukidashi ("fukidasi-enjoying");
				ball.GetComponent<Ball> ().Move ();
				return true;
			}
		} 
		return false;
    }


	//振り向き(多分使わない)
	/*
	public bool TurnLook() {
		if (playerSpine != null && playerSpine.transform.rotation.y == 0) {
			float _angle = Mathf.LerpAngle(0, 180, Time.time / 2);
			playerSpine.transform.eulerAngles = new Vector3(0, _angle, 0);
			return true;
		}
		if (playerSpine != null && playerSpine.transform.rotation.y == 180) {
			float _angle = Mathf.LerpAngle(180, 0, Time.time / 2);
			playerSpine.transform.eulerAngles = new Vector3(0, _angle, 0);
			return true;
		}
		return false;
	}
	*/


	//キラキラエフェクト発生
	void CreateDelEffect() {
		delightedEffect = Instantiate (dEffctRs, playerSpine.transform.position, Quaternion.Euler(90, 0, 0));
		delightedEffect.transform.parent = this.transform;
	}


	//キラキラエフェクト削除
	void DestroyDelEffect() {
		if (delightedEffect != null) {
			Destroy (delightedEffect);
			delightedEffect = null;
		}	
	}


	//吹き出しの管理
	void ShowFukidashi(string name) {
		foreach (KeyValuePair<string, GameObject> pair in fukidashiDic) {
			if (pair.Key == "fukidasi-eating" || pair.Key == "fukidasi-talking" || pair.Key == "fukidasi-enjoying") {
				pair.Value.SetActive (false);
			}
		}

		if (fukidashiDic.ContainsKey (name))
			fukidashiDic [name].SetActive (true);
	}


	//吹き出し消す
	void DeleteFukidashi() {
		foreach (KeyValuePair<string, GameObject> pair in fukidashiDic) {
			if (pair.Key == "fukidasi-eating" || pair.Key == "fukidasi-talking" || pair.Key == "fukidasi-enjoying") {
				pair.Value.SetActive (false);
			}
		}
	}
		
}
