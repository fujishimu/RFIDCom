using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FurnitureManager : MonoBehaviour {
	public bool isDebugMode;         // Trueなら矢印オブジェクトが表示される
	public GameObject antenaBase;    // AntenaBase. オブジェクトの配置場所を全て持っている

	private GameObject matrixParent; // 配置する親(1,1 1,2 1,3 など）
	private Vector3 matrixPosition;  // 配置する場所

	/// <summary>
	/// 家具の生成，更新，削除を行っている
	/// </summary>
	/// <param name="tagData">Tag data.</param>
	public void ArrangeFurnitureIfNeeded (TagData tagData) {
		SetParent (tagData);
		if (matrixParent.transform.childCount == 0) {
			DeleteFurniture (tagData.UID);
			matrixPosition = matrixParent.transform.position;
			CreateFurniture (tagData);
		} else {
			Transform child = matrixParent.transform.GetChild (0);
			bool isSameName = child.gameObject.name == tagData.UID;
			bool isSameSurface = tagData.IsSurface == child.GetComponent<Furniture> ().IsReverse;
			if (isSameName && isSameSurface) {
				// 回転のみ更新する
				UpdateFurniture (child.gameObject, (float)tagData.Rotate);
			} else {
				Destroy (child.gameObject);
				matrixPosition = child.position;
				CreateFurniture (tagData);
			}
		}
	}

	/// <summary>
	/// 行と列の値が送られてくるので，それに対応した親を見つける
	/// 0,5 0,6 0,7とか
	/// </summary>
	private void SetParent (TagData tagData) {
		if (tagData.Row > tagData.Column) {
			matrixParent = antenaBase.transform.Find (tagData.Column.ToString () + "," + tagData.Row.ToString ()).gameObject;
		} else {
			matrixParent = antenaBase.transform.Find (tagData.Row.ToString () + "," + tagData.Column.ToString ()).gameObject;
		}
	}

	/// <summary>
	/// オブジェクトを生成して配置する
	/// </summary>
	private void CreateFurniture (TagData tagData) {
		GameObject _f;

		if (tagData.IsSurface) { 
			if (isDebugMode) {
				_f = Instantiate (ObjectManager.Instance.DebugObjectR, matrixPosition, Quaternion.identity) as GameObject;
			} else {
				_f = Instantiate (ObjectManager.Instance.GetFurnitureByKey (tagData.UID + "R"), matrixPosition, Quaternion.identity) as GameObject;	
			}
		} else {
			if (isDebugMode) {
				_f = Instantiate (ObjectManager.Instance.DebugObject, matrixPosition, Quaternion.identity) as GameObject;
			} else {
				_f = Instantiate (ObjectManager.Instance.GetFurnitureByKey (tagData.UID), matrixPosition, Quaternion.identity) as GameObject;
			
			}
		}
		_f.GetComponent<Furniture> ().Rotation = (float)tagData.Rotate;
		_f.GetComponent<Furniture> ().UpdateData ();
		_f.GetComponent<Furniture> ().IsReverse = tagData.IsSurface;
		_f.GetComponent<Furniture> ().UID = tagData.UID;
		_f.name = tagData.UID;
		_f.transform.parent = matrixParent.transform;
	}

	/// <summary>
	/// 配置したオブジェクトを更新する
	/// </summary>
	void UpdateFurniture (GameObject matrix, float _rotate) {
		matrix.GetComponent<Furniture> ().Rotation = _rotate;
		matrix.GetComponent<Furniture> ().UpdateData ();
	}

	/// <summary>
	/// 配置されているオブジェクトを削除
	/// </summary>
	public void DeleteFurniture (string _uID) {
		for (int i = 0; i < antenaBase.transform.childCount; i++) {
			Transform matrix = antenaBase.transform.GetChild (i);
			if (matrix.childCount == 0) {
				continue;
			}
			if (matrix.GetChild (0).name == _uID) {
				Destroy (matrix.GetChild (0).gameObject);
			}
		}
	}

	public void RemoveAll () {
		for (int i = 0; i < antenaBase.transform.childCount; i++) {
			Transform matrix = antenaBase.transform.GetChild (i);
			if (matrix.childCount == 0) {
				continue;
			}
			Destroy (matrix.GetChild (0).gameObject);
		}
	}

	public IList<string> GetPreData () {
		IList<string> _list = new List<string> ();
		for (int i = 0; i < antenaBase.transform.childCount; i++) {
			Transform matrix = antenaBase.transform.GetChild (i);
			if (matrix.childCount == 0) {
				continue;
			}
			_list.Add (matrix.GetChild (0).name);
		}
		return _list;
	}
}
