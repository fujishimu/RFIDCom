using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    GameObject arrow;
    public float speed = 2f;
    public float arrowWidth = 2f;
    public float arrowHeight = 2f;
    public int arrowAngle = 0;
    int arrowSpeed;

	void Start () {
        arrow = gameObject.transform.Find("arrow").gameObject;
        arrowSpeed = (int)speed;
    }
	
	void Update () {
        float _arrAngF = arrow.transform.rotation.z * 360;  //矢印の現在の角度
        int _arrAng = (int)_arrAngF;
        Debug.Log("test: " +  _arrAng);
        //TODO:矢印の向き変更をどうするか
        //if ()
        //{
            arrow.transform.Rotate(new Vector3(0, 0, arrowSpeed));
        //}

        float _x = Mathf.Cos(Time.time * speed) * arrowWidth;
        float _z = Mathf.Sin(Time.time * speed) * arrowHeight;
        float _y = 0f;
        // arrow.transform.position = new Vector3(_x + transform.position.x, _y + transform.position.y, _z + transform.position.z);
    }
}
