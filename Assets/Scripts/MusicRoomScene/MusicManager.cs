using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public GameObject PianoAudioObj;
	public GameObject BassAudioObj;
	public GameObject DrumAudioObj;
	private GvrAudioSource pianoAudio;
	private GvrAudioSource bassAudio;
	private GvrAudioSource drumAudio;

	void Start () {
		pianoAudio = PianoAudioObj.GetComponent<GvrAudioSource> ();
		bassAudio = BassAudioObj.GetComponent<GvrAudioSource> ();
		drumAudio = DrumAudioObj.GetComponent<GvrAudioSource> ();
	}

	/// <summary>
	/// ピアノ関係の処理
	/// </summary>
	public void UnMutePiano () {
		pianoAudio.mute = false;
	}
	public void MutePiano () {
		pianoAudio.mute = true;
	}
	public void SetPianoPosition (Vector3 position) {
		PianoAudioObj.transform.localPosition = position;
	}

	/// <summary>
	/// ベース関係の処理
	/// </summary>
	public void UnMuteBass () {
		bassAudio.mute = false;
	}
	public void MuteBass () {
		bassAudio.mute = true;
	}
	public void SetBassPosition (Vector3 position) {
		BassAudioObj.transform.localPosition = position;
	}

	/// <summary>
	/// ドラム関係の処理
	/// </summary>
	public void UnMuteDrum () {
		drumAudio.mute = false;
	}
	public void MuteDrum () {
		drumAudio.mute = true;
	}
	public void SetDrumPosition (Vector3 position) {
		DrumAudioObj.transform.localPosition = position;
	}
}
