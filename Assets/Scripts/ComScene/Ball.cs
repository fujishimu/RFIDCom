using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	bool isLeft;	//左側に生成されたらtrue
	public float spd = 0.2f;	//ボールの移動速度
	public float rSpd = 60;	//回転スピード
	GameObject player1;
	GameObject player2;

	public bool IsLeft { get{ return isLeft; } set{ isLeft = value; }}


	void Start () {
		isLeft = false;
	}

	public void Move() {
		GameObject[] _players = GameObject.FindGameObjectsWithTag ("Player");
		if (_players.Length == 2) {
			player1 = _players [0];
			player2 = _players [1];
		}

		if (transform.position.x > 6.5f)  {
			spd *= -1;
			rSpd *= -1;
			if (player1.GetComponent<Player> ().PlaceColumn == 4) {
				player1.GetComponent<Player> ().SpineState (5);
			}
			if (player2.GetComponent<Player> ().PlaceColumn == 4) {
				player2.GetComponent<Player> ().SpineState (5);
			}
		}
		if (transform.position.x < -7.0f) {
			spd *= -1;
			rSpd *= -1;
			if (player1.GetComponent<Player> ().PlaceColumn == 7) {
				player1.GetComponent<Player> ().SpineState (5);
			}
			if (player2.GetComponent<Player> ().PlaceColumn == 7) {
				player2.GetComponent<Player> ().SpineState (5);
			}
		}

		
		transform.position += new Vector3 (spd, 0, 0);
		transform.Rotate(new Vector3(0, 0, rSpd));
	}
}
