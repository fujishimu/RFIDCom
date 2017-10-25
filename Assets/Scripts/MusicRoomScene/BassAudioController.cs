using UnityEngine;
using System.Collections;

public class BassAudioController : MonoBehaviour {

	void OnEnable () {
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		musicManager.UnMuteBass();
		musicManager.SetBassPosition (gameObject.transform.localPosition);
	}

	void OnDisable () {
		if (GameObject.Find ("MusicManager") == null) {
			return;
		}
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		if (musicManager == null) {
			return;
		}
		musicManager.MuteBass();
	}
}