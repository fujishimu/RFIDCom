using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Prefabのディクショナリ
/// 予めロードしておく
/// タグとオブジェクトの関連はここに定義している
/// </summary>
public class ObjectManager {
	private static ObjectManager _instance = null;
	private IDictionary<string,GameObject> _objectDic;
	private GameObject _debugObj;
	private GameObject _debugObjR;

	public GameObject DebugObject{
		get {
			return _debugObj;
		}
	}
	public GameObject DebugObjectR{
		get {
			return _debugObjR;
		}
	}

	public static ObjectManager Instance {
		get {
			if(_instance == null) {
				_instance = new ObjectManager();
				_instance.init();
				Debug.Log ("Object Manager Instantiated");
			}
			return _instance;
		}
	}

	public void ClearTagPrefabRelation () {
		_objectDic.Clear ();
	}

	public static void Release () {
		_instance = null;
	}


	private void init() {
		_objectDic = new Dictionary<string, GameObject> ();

		// シーン名によって表示するオブジェクトを変更する
		string sceneName = SceneManager.GetActiveScene ().name;
		if (sceneName == "MainScene") {
			InitForFurnitureScene ();
		}
		if (sceneName == "MusicRoomScene") {
			InitForMusicRoomScene ();
		}
		if (sceneName == "ComScene") {
			InitForComScene ();
		}
	}

	private void InitForFurnitureScene () {
		// TODO: 存在しないタグ
		// _objectDic.Add ("E007A20000001065",  Resources.Load ("Prefabs/E007A20000001065") as GameObject);
		// _objectDic.Add ("E007A20000001065R", Resources.Load ("Prefabs/E007A20000001065R") as GameObject);
		// _objectDic.Add ("E007A200000017B2",  Resources.Load ("Prefabs/E007A200000017B2") as GameObject);
		// _objectDic.Add ("E007A200000017B2R", Resources.Load ("Prefabs/E007A200000017B2R") as GameObject);
		// _objectDic.Add ("E007A200000017B8",  Resources.Load ("Prefabs/E007A200000017B8") as GameObject);
		// _objectDic.Add ("E007A200000017B8R", Resources.Load ("Prefabs/E007A200000017B8R") as GameObject);
		_objectDic.Add ("E007A200000017B9",  Resources.Load ("Prefabs/E007A200000017B9") as GameObject);
		_objectDic.Add ("E007A200000017B9R", Resources.Load ("Prefabs/E007A200000017B9R") as GameObject);
		_objectDic.Add ("E007A200000017BC",  Resources.Load ("Prefabs/E007A200000017BC") as GameObject);
		_objectDic.Add ("E007A200000017BCR", Resources.Load ("Prefabs/E007A200000017BCR") as GameObject);
		_objectDic.Add ("E007A200000017C5",  Resources.Load ("Prefabs/E007A200000017C5") as GameObject);
		_objectDic.Add ("E007A200000017C5R", Resources.Load ("Prefabs/E007A200000017C5R") as GameObject);
		_debugObj = Resources.Load("Prefabs/Cube") as GameObject;
		_debugObjR = Resources.Load("Prefabs/CubeR") as GameObject;

	}

	private void InitForMusicRoomScene () {
		Debug.Log ("InitForMusicRoomScene()");
		_objectDic.Add ("E007A200000017B9",  Resources.Load ("Prefabs/PianoWithChair") as GameObject);
		_objectDic.Add ("E007A200000017B9R", Resources.Load ("Prefabs/PianoWithChair") as GameObject);
		_objectDic.Add ("E007A200000017BC",  Resources.Load ("Prefabs/GuitarWithAmp") as GameObject);
		_objectDic.Add ("E007A200000017BCR", Resources.Load ("Prefabs/GuitarWithAmp") as GameObject);
		_objectDic.Add ("E007A200000017C5",  Resources.Load ("Prefabs/DrumSet") as GameObject);
		_objectDic.Add ("E007A200000017C5R", Resources.Load ("Prefabs/DrumSet") as GameObject);
		_debugObj = Resources.Load("Prefabs/Cube") as GameObject;
		_debugObjR = Resources.Load("Prefabs/CubeR") as GameObject;
	}

	private void InitForComScene () {
		Debug.Log ("InitForComScene()");
		_objectDic.Add ("E007A200000017B9", Resources.Load ("Prefabs/ComScene/Player1") as GameObject);
		_objectDic.Add ("E007A200000017C5", Resources.Load ("Prefabs/ComScene/Player2") as GameObject);
	}

	public GameObject GetFurnitureByKey(string _key) {
		return _objectDic [_key];
	}
}
