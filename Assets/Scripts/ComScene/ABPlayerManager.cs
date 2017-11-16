using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABPlayerManager : MonoBehaviour {

	public List<GameObject> matList;	//マス目のリスト
	//Dictionary<string ,GameObject> existDic;	//タグが置かれて生成されたPrefabのDic
	List<GameObject> existList;

	void Start() {
		existList = new List<GameObject> ();
	}


	void Update() {

		for (int i = 0; i < matList.Count; i++) {
			// マス目に生成されていたらexitListに追加。
			if (matList[i].transform.childCount > 0 && !existList.Contains(matList[i].transform.GetChild (0).gameObject)) {
				GameObject _tmp = matList[i].transform.GetChild (0).gameObject;
				_tmp.GetComponent<Player> ().placeNum = matList [i].name;
				existList.Add(_tmp);
				Debug.Log ("Add " + matList[i].name);
			}
			//タグが取り除かれたらexistDicから削除
			if (matList[i].transform.childCount == 0) {
				for(int j = 0; j < existList.Count; j++) {
					Debug.Log (existList[j].name);
					if (existList[j].name == matList [i].name) {
						existList.RemoveAt (j);
						Debug.Log ("Remove " + matList [i].name);
						break;
					}
				}
			}
		}
			
		//2つ生成されていたら向かい合わせる
		if(existList.Count == 2) {
	}
	}

}
