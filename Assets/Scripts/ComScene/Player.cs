using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour {

    GameObject playerSpine;
	GameObject arrow;
	SkeletonAnimation playerAnimation;
	public float speed = 2f;
	public float arrowWidth = 2f;
	public float arrowHeight = 2f;
	public string placeNum;

    int state; //0: 何もしない. 1: 向かい合う
    int arrowSpeed = 0;
	float triggerTimeout; //onTriggerStayじゃない時は0


	void Start () {
		//生成時に透明→FadeIn
        playerSpine = gameObject.transform.Find("PlayerSpine").gameObject;
		playerAnimation = playerSpine.GetComponent<SkeletonAnimation> ();
		playerAnimation.skeleton.a = 0f;

        state = 0; 
        SpineState(0);
        arrow = gameObject.transform.Find("Arrow").gameObject;
        arrowSpeed = (int)speed;
    }

	void FixedUpdate () {
		if (playerAnimation.skeleton.a < 1.0f) {
			playerAnimation.skeleton.a += 0.05f;
		}

		StayTriggerChecker ();
    }

	void Move(int n) {
		
	}

	//<summary>
	//キャラクタのアングルを変える
	//現在は丸の位置が変わる
	//</summary>
	public void Angle(float arrowAngle) {
		float _ar = arrowAngle;
		if (_ar > 180 && _ar <= 360) {
			_ar = (_ar - 180) * -1;
		}
		float _x = Mathf.Cos(_ar * speed) * arrowWidth;
		float _z = Mathf.Sin(_ar * speed) * arrowHeight;
		float _y = 0f;
		if (arrow != null) {
			arrow.transform.position = new Vector3 (_x + transform.position.x, _y + transform.position.y, _z + transform.position.z);
		}
	}

    //アニメーションのstate
    //0: Idle
    //1: Chat
    public void SpineState(int n) {
        if(n == 0) {    //何もしていない
            state = 0;
            playerSpine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "Idle", true);
        }
        if(n == 1) {    //向いてる
            state = 1;
            playerSpine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "Chat", true);
        }
    }

	//<summary>
	//OnTriggerStayじゃない時にisTriggerStayをfalseにする
	//</summary>
	void StayTriggerChecker() {
		if (triggerTimeout > 0) {
			triggerTimeout -= Time.deltaTime;
		}
		if (triggerTimeout <= 0 && state != 0) {
			SpineState (0);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player") {
			//SpineState (1);
		}
	}

	void OnTriggerExit(Collider col) {
		Debug.Log ("teset");
		if (col.tag == "Player") {
			//SpineState (0);
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "Player") {
			triggerTimeout = 0.1f;
			if (state == 0) {
				SpineState (1);
			}
		}
	}

	public int getState() {
		return state;
	}
}
