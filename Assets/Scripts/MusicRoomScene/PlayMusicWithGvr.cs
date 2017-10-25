using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// シーン名によって音楽を再生するかどうか決める
/// </summary>
public class PlayMusicWithGvr : MonoBehaviour {

	void Start () {
		string sceneName = SceneManager.GetActiveScene ().name;

		if (sceneName != "MusicRoomScene") {
			return;
		}

		AudioClip bgm = (AudioClip)Resources.Load("piano");
		Debug.Log (bgm);
		gameObject.GetComponent<GvrAudioSource> ().clip = bgm;
	}
}
