using UnityEngine;
using System.Collections;

public class PianoAudioController : MonoBehaviour {
	void OnEnable () {
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		musicManager.UnMutePiano();
		musicManager.SetPianoPosition (gameObject.transform.localPosition);
	}

	void OnDisable () {
		if (GameObject.Find ("MusicManager") == null) {
			return;
		}
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		if (musicManager == null) {
			return;
		}
		musicManager.MutePiano();
	}
}
