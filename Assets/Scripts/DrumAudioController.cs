using UnityEngine;
using System.Collections;

public class DrumAudioController : MonoBehaviour {

	void OnEnable () {
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		musicManager.UnMuteDrum();
		musicManager.SetDrumPosition (gameObject.transform.localPosition);
	}

	void OnDisable () {
		if (GameObject.Find ("MusicManager") == null) {
			return;
		}
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		if (musicManager == null) {
			return;
		}
		musicManager.MuteDrum();
	}
}
