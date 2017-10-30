using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerManager : MonoBehaviour {

    GameObject playerSpine;
    int state; //0: 何もしない. 1: 向かい合う
    GameObject arrow;
    public float speed = 2f;
    public float arrowWidth = 2f;
    public float arrowHeight = 2f;
    public int arrowAngle = 0;
    int arrowSpeed;


	void Start () {
        playerSpine = gameObject.transform.Find("PlayerSpine").gameObject;
        state = 0; 
        SpineState(0);
        arrow = gameObject.transform.Find("Arrow").gameObject;
        arrowSpeed = (int)speed;
    }

	void FixedUpdate () {
        /*
                float _arrAngF = arrow.transform.rotation.z * 360;  //矢印の現在の角度
                int _arrAng = (int)_arrAngF;
                Debug.Log("test: " +  _arrAng);
        */
        float _x = Mathf.Cos(Time.time * speed) * arrowWidth;
        float _z = Mathf.Sin(Time.time * speed) * arrowHeight;
        float _y = 0f;
        arrow.transform.position = new Vector3(_x + transform.position.x, _y + transform.position.y, _z + transform.position.z);

    }

	void Move(int n) {
		
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

	public int getState() {
		return state;
	}
}
