using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABPlayerManager : MonoBehaviour {

	//public List<GameObject> matList;	//マス目のリスト
	Dictionary<string ,GameObject> existDic;	//タグが置かれて生成されたPrefabのDic

	public GameObject ball;
	public Text clearText;
	public Text stageClearText;
	private const int MaxClearCnt = 3;
	Dictionary<string, bool> animFlgDic = new Dictionary<string, bool>() {	//アニメーションをしたかどうか
		{"eating", false},
		{"watching", false},
		{"talking", false},
		{"enjoying", false}
	};	


	void Start() {
		existDic = new Dictionary<string, GameObject> ();
		//clearText.text = "0/" + MaxClearCnt;
		//stageClearText.text = "";
	}


	void Update() {

        /*
		for (int i = 0; i < matList.Count; i++) {
			// マス目に生成されていたらexitListに追加。
			if (matList [i].transform.childCount > 0 && !existDic.ContainsKey(matList[i].name)) {
				GameObject _tmp = matList [i].transform.GetChild (0).gameObject;
				string[] _str = matList [i].name.Split (',');
				_tmp.GetComponent<Player> ().PlaceRow = int.Parse(_str [0]);
				_tmp.GetComponent<Player> ().PlaceColumn = int.Parse(_str[1]);
				existDic.Add (matList[i].name, _tmp);
				Debug.Log ("Add " + matList[i].name);
			}
			//タグが取り除かれたらexistDicから削除
			if (matList [i].transform.childCount == 0 && existDic.Count > 0 && existDic.ContainsKey(matList[i].name)) { 
				existDic.Remove (matList [i].name);
				Debug.Log ("Remove " + matList [i].name);
			}
		}
        */

		//タグが存在していない時
		if (existDic.Count == 0) {
		}

		//1つ存在している時
		if (existDic.Count == 1) {
			List<string> _keyList = new List<string> ();
			foreach (KeyValuePair<string, GameObject> dic in existDic) {
				_keyList.Add (dic.Key);
			}

			GameObject _dic0 = existDic [_keyList [0]];
			if(_dic0 != null)_dic0.GetComponent<Player>().SpineState (0);
		}

		//2つ存在している時
		if (existDic.Count == 2) {
			List<string> _keyList = new List<string> ();
			foreach (KeyValuePair<string, GameObject> dic in existDic) {
				_keyList.Add (dic.Key);
			}

			GameObject _dic0 = existDic [_keyList [0]];
			GameObject _dic1 = existDic [_keyList [1]];

			//距離に応じてモーションが変わる
			if (_dic0 != null && _dic1 != null) {
				float _d = Distance (_dic0, _dic1);

				//距離が1_
				if (_d == 1) {
					if (ChkPlace (_dic0, _dic1, 1, 5, 1, 6, false, true)) {	//机に向かって食べる
						_dic0.GetComponent<Player> ().SpineState (3);
						_dic1.GetComponent<Player> ().SpineState (3);
						if (!animFlgDic ["eating"])
							OnClear ("eating");	
						Debug.Log ("Eating!");

					} else if (ChkPlace (_dic0, _dic1, 0, 7, 1, 7, false, false, "E007A200000017C5", "E007A200000017B9")) {	//テレビをみる
						_dic0.GetComponent<Player> ().SpineState (4);
						_dic1.GetComponent<Player> ().SpineState (4);
						OnClear ("watching");
						Debug.Log ("Watching TV!");

					}else if ((_dic0.GetComponent<Furniture>().IsReverse && !_dic1.GetComponent<Furniture>().IsReverse ) ||
							!_dic0.GetComponent<Furniture>().IsReverse && _dic1.GetComponent<Furniture>().IsReverse){	//向かい合わせ
							if (_dic0.GetComponent<Player> ().PlaceRow == _dic1.GetComponent<Player> ().PlaceRow) {	//行が一緒のとき
								_dic0.GetComponent<Player> ().SpineState (1);
								_dic1.GetComponent<Player> ().SpineState (1);
								OnClear ("talking");
								Debug.Log ("Talking!");
							}
					} else {
						_dic0.GetComponent<Player> ().SpineState (0);
						_dic1.GetComponent<Player> ().SpineState (0);
					}
				
				//距離が3
				} else if (_d == 3) {
					if (ChkPlace (_dic0, _dic1, 3, 4, 3, 7, false, true)) {	//ボールで遊ぶ
						_dic0.GetComponent<Player> ().SpineState (5);
						_dic1.GetComponent<Player> ().SpineState (5);
						CatchBall ();
						OnClear ("enjoying");
						Debug.Log ("Enjoying with ball!");
					}

				//遠くて何もなし
				} else {
					_dic0.GetComponent<Player> ().SpineState (0);
					_dic1.GetComponent<Player> ().SpineState (0);
				}

				/*
				//振り向く
				if (existDic [_keyList [0]].GetComponent<Player> ().PlaceColumn < existDic [_keyList [1]].GetComponent<Player> ().PlaceColumn) {
					existDic [_keyList [1]].GetComponent<Player> ().TurnLook ();
				} else if (existDic [_keyList [0]].GetComponent<Player> ().PlaceColumn > existDic [_keyList [1]].GetComponent<Player> ().PlaceColumn) {
					existDic [_keyList [0]].GetComponent<Player> ().TurnLook ();
				}
				*/

			}

		}

	}


	// 2Player間の距離を測る
	float Distance(GameObject a, GameObject b) {
		int aX = a.GetComponent<Player> ().PlaceRow;
		int aY = a.GetComponent<Player> ().PlaceColumn;
		int bX = b.GetComponent<Player> ().PlaceRow;
		int bY = b.GetComponent<Player> ().PlaceColumn;

		float _a = 0f;
		if (aX != null && aY != null && bX != null && bY != null) {
			_a = (bX - aX) * (bX - aX) + (bY - aY) * (bY - aY);
		}

		return Mathf.Sqrt(_a);
	}


	//２つのタグがその場所にあるか
	//obj0: 一つ目
	//obj1: 二つ目
	//row0: 一つ目の行
	//column0: 一つ目の列
	//row1: 二つ目の行
	//column1:二つ目の列
	//rev0: 一つ目が裏かどうか
	//rev1: 二つ目が裏かどうか
	//name0: 一つ目のtag名
	//name1: 二つ目のtag名
	bool ChkPlace(GameObject obj0, GameObject obj1, int row0, int column0, int row1, int column1, bool rev0, bool rev1, string name0, string name1) {
		if ((obj0.GetComponent<Player> ().PlaceRow == row0 && obj0.GetComponent<Player> ().PlaceColumn == column0 &&
			obj1.GetComponent<Player> ().PlaceRow == row1 && obj1.GetComponent<Player> ().PlaceColumn == column1 &&
			obj0.GetComponent<Furniture> ().IsReverse == rev0 && obj1.GetComponent<Furniture> ().IsReverse == rev1) || 
			(obj0.GetComponent<Player> ().PlaceRow == row1 && obj0.GetComponent<Player> ().PlaceColumn == column1 &&
				obj1.GetComponent<Player> ().PlaceRow == row0 && obj1.GetComponent<Player> ().PlaceColumn == column0 &&
				obj0.GetComponent<Furniture> ().IsReverse == rev1 && obj1.GetComponent<Furniture> ().IsReverse == rev0))
		{
			if (obj0.name == name0 && obj1.name == name1) {
				return true;
			}
			return false;
		} else {
			return false;
		}

	}	

	//名前の区別をしない
	bool ChkPlace(GameObject obj0, GameObject obj1, int row0, int column0, int row1, int column1, bool rev0, bool rev1) {
		if ((obj0.GetComponent<Player> ().PlaceRow == row0 && obj0.GetComponent<Player> ().PlaceColumn == column0 &&
			obj1.GetComponent<Player> ().PlaceRow == row1 && obj1.GetComponent<Player> ().PlaceColumn == column1 &&
			obj0.GetComponent<Furniture> ().IsReverse == rev0 && obj1.GetComponent<Furniture> ().IsReverse == rev1) || 
			(obj0.GetComponent<Player> ().PlaceRow == row1 && obj0.GetComponent<Player> ().PlaceColumn == column1 &&
				obj1.GetComponent<Player> ().PlaceRow == row0 && obj1.GetComponent<Player> ().PlaceColumn == column0 &&
				obj0.GetComponent<Furniture> ().IsReverse == rev1 && obj1.GetComponent<Furniture> ().IsReverse == rev0))
		{
			return true;
		} else {
			return false;
		}
			
	}	

	//名前と向きの区別をしない
	bool ChkPlace(GameObject obj0, GameObject obj1, int row0, int column0, int row1, int column1) {
		if ((obj0.GetComponent<Player> ().PlaceRow == row0 && obj0.GetComponent<Player> ().PlaceColumn == column0 &&
			obj1.GetComponent<Player> ().PlaceRow == row1 && obj1.GetComponent<Player> ().PlaceColumn == column1) || 
			(obj0.GetComponent<Player> ().PlaceRow == row1 && obj0.GetComponent<Player> ().PlaceColumn == column1 &&
				obj1.GetComponent<Player> ().PlaceRow == row0 && obj1.GetComponent<Player> ().PlaceColumn == column0))
		{
			return true;
		} else {
			return false;
		}
	}


	//ボールを投げ合う
	void CatchBall() {
		if (ball != null) {
			ball.GetComponent<Ball> ().Move ();
		}	
	}


	//行動を管理
	void OnClear(string anim) {
		List<string> keys = new List<string> (animFlgDic.Keys);
		int clearCnt = 0;
		animFlgDic [anim] = true;
		foreach (string key in keys) {
			if (animFlgDic [key]) {
				clearCnt++;
			}
		}

		//clearText.text = clearCnt + "/" + MaxClearCnt;

		//ステージクリア
		if (clearCnt >= MaxClearCnt) {
			//stageClearText.text = "Stage Clear!!!!";
		}
	}
		
}