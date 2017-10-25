using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 仮想空間を移動するために壁などにAttachするスクリプト
/// プレイヤがタップしているかつ壁にEventTriggerが動作していた場合にその方向に向かって移動する 
/// </summary>
[RequireComponent(typeof(Collider))]
public class GvrEventDetector : MonoBehaviour, IGvrGazeResponder {

	public GameObject gvrMain;
	private GvrMove gvrMove;
	private Vector3 startingPosition;
	private bool isHold;  // 押し続けているか

	void Start() {
		gvrMain = GameObject.Find ("GvrMain");
		gvrMove = gvrMain.GetComponent<GvrMove> ();
		startingPosition = transform.localPosition;
	}

	void Update() {
		if (isHold) {
			gvrMove.Move ();
		}
	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
		if (GvrViewer.Instance.BackButtonPressed) {
			Application.Quit();
		}
	}

	public void SetGazedAt(bool gazedAt) {
		if (GetComponent<Renderer> ()) {
			GetComponent<Renderer> ().material.color = gazedAt ? Color.green : Color.white;
		}
	}

	public void Reset() {
		transform.localPosition = startingPosition;
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
	}

	public void ToggleDistortionCorrection() {
		switch(GvrViewer.Instance.DistortionCorrection) {
		case GvrViewer.DistortionCorrectionMethod.Unity:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Native;
			break;
		case GvrViewer.DistortionCorrectionMethod.Native:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.None;
			break;
		case GvrViewer.DistortionCorrectionMethod.None:
		default:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
			break;
		}
	}

	public void ToggleDirectRender() {
		GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
	}

	public void TeleportRandomly() {
		Vector3 direction = Random.onUnitSphere;
		direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
		float distance = 2 * Random.value + 1.5f;
		transform.localPosition = direction * distance;
	}

	// ボタンを押した 
	public void OnGazeDown() {
		isHold = true;
	}
	// ボタンを離した
	public void OnGazeUp() {
		isHold = false;
	}
	#region IGvrGazeResponder implementation
	public void OnGazeEnter() {
		SetGazedAt (true);
	}
	public void OnGazeExit() {
		SetGazedAt (false);
	}
	public void OnGazeTrigger() {
		ChangeScene ();
	}
	#endregion

	private void ChangeScene () {
		string sceneName = SceneManager.GetActiveScene ().name;
		if (sceneName == "MainScene") {
			ObjectManager.Instance.ClearTagPrefabRelation ();
			ObjectManager.Release ();
			SceneManager.LoadScene ("MusicRoomScene");
		} else {
			ObjectManager.Instance.ClearTagPrefabRelation ();
			ObjectManager.Release ();
			SceneManager.LoadScene ("MainScene");
		}
	}
}
