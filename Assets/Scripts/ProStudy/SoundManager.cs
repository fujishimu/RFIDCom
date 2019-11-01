using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    GameObject seSoundPlayerObj, bgmSoundPlayerObj;
    AudioSource audioSource;
    AudioSource bgmSource;
    Dictionary<string, AudioClipInfo> seAudioClips = new Dictionary<string, AudioClipInfo>();
    Dictionary<string, AudioClipInfo> bgmAudioClips = new Dictionary<string, AudioClipInfo>();
    float seVol, bgmVol;
    List<string> onlySounds;
    List<string> onlySoundCounts;
    bool isPlaySE;   //falseでseの音が鳴らない

    //同時に一回しかならない音一覧
    string[] ONLY_SOUNDS = {
        /*"zeroEnemyCount",
        "playerTurn",
        "rangeAttack",
        "rangeDark",
        "rangeLight",
        "rangeBliz",
        "rangeStorm",
        "shot",
        "deathEnemy",
        "throwCard"*/
    };


    class AudioClipInfo {
		public string resourceName;
		public AudioClip clip;

		public AudioClipInfo(string resourceName) {
			this.resourceName = resourceName;
		}
	}


	void Awake() {
		Instance = this;

        Object[] _seClips = Resources.LoadAll("SE", typeof(AudioClip));
		Object[] _bgmClips = Resources.LoadAll("BGM",typeof(AudioClip));
        
        if (_seClips != null) {
			for(int i = 0; i < _seClips.Length; i++) {
				AudioClipInfo info = new AudioClipInfo(_seClips[i].name);
                info.clip = _seClips[i] as AudioClip;
				seAudioClips.Add(_seClips[i].name, info);
            }		
		}
		if(_bgmClips != null) {
			for(int i = 0; i < _bgmClips.Length; i++) {
				AudioClipInfo info = new AudioClipInfo(_seClips[i].name);
                info.clip = _bgmClips[i] as AudioClip;
				bgmAudioClips.Add(_bgmClips[i].name, info);
			}
		}

        seVol = PlayerPrefs.GetFloat("SeVolume", 0.7f);
        bgmVol = PlayerPrefs.GetFloat ("BgmVolume", 0.7f);

        onlySounds = new List<string>();
        for(int i = 0; i < ONLY_SOUNDS.Length; i++) {
            onlySounds.Add(ONLY_SOUNDS[i]);
        }

        onlySoundCounts = new List<string>();

        isPlaySE = true;
    }

    /// <summary>
    /// BGMの管理はここで
    /// </summary>
    private void Start() {
        //BGMを鳴らす
        /*
        if(SceneManager.GetActiveScene().name == "TestStage1") {
            switch(GameParameterManager.stageNum) {
                case 1:
                    PlayBGM("stage1");
                    break;
                case 2:
                    PlayBGM("stage2");
                    break;
                case 3:
                    PlayBGM("stage3");
                    break;
                case 100:
                    PlayBGM("stage1");
                    break;
            }
        }*/
    }

    private void FixedUpdate() {
        onlySoundCounts.Clear();
    }

    public bool PlaySE(string resourceName) {
        if (isPlaySE) {
            if (!seAudioClips.ContainsKey(resourceName)) {
                Debug.Log(resourceName + " not exist.");
                return false;
            }

            AudioClipInfo info = seAudioClips[resourceName];

            if (seSoundPlayerObj == null) {
                seSoundPlayerObj = new GameObject("SeSoundPlayer");
                audioSource = seSoundPlayerObj.AddComponent<AudioSource>();
            }

            for (int i = 0; i < onlySounds.Count; i++) {
                if (onlySounds[i] == resourceName) {

                    bool _is = false;
                    for (int j = 0; j < onlySoundCounts.Count; j++) {
                        if (onlySoundCounts[j] == resourceName) {
                            _is = true;
                        }
                    }
                    if (_is) {
                        return false;
                    } else if (_is == false) {
                        onlySoundCounts.Add(resourceName);
                        audioSource.volume = seVol;
                        audioSource.PlayOneShot(info.clip);
                        return true;
                    }

                }
            }

            audioSource.volume = seVol;
            audioSource.PlayOneShot(info.clip);

            return true;
        }
        return false;
	}
    
    public bool PlayBGM(string resourceName, bool loop) {
        if (!bgmAudioClips.ContainsKey(resourceName)) {
            Debug.Log(resourceName + " not exist.");
            return false;
        }

        AudioClipInfo info = bgmAudioClips[resourceName];

        if (bgmSoundPlayerObj == null) {
            bgmSoundPlayerObj = new GameObject("BgmSoundPlayer");
            bgmSource = bgmSoundPlayerObj.AddComponent<AudioSource>();
        }
        bgmSource.clip = info.clip;
        bgmSource.volume = bgmVol;
        bgmSource.loop = loop;
        bgmSource.Play();

        return true;
    }

    public bool PlayBGM(string resourceName) {
        return PlayBGM(resourceName, true);
    }
    
    public void StopBGM(float spd) {
        bgmSource.DOFade(0, spd);
    }

    public void StopBGM() {
        StopBGM(0);
    }

    /// <summary>
    /// SEの音を無音に
    /// </summary>
    public void StopSE() {
        isPlaySE = false;
    }

    /// <summary>
    /// SEの音を有音に
    /// </summary>
    public void StartSE() {
        isPlaySE = true;
    }

    public void SetSeVol(float vol) {
        PlayerPrefs.SetFloat("SeVolume", vol);
        this.seVol = vol;
    }

    public void SetBgmVol(float vol) {
        PlayerPrefs.SetFloat("BgmVolume", vol);
        if (bgmSource != null) {
            bgmSource.volume = vol;
        }
        this.bgmVol = vol;
    }

    public void LoopBGM(bool bl) {
        bgmSource = bgmSoundPlayerObj.AddComponent<AudioSource>();
        bgmSource.loop = bl;
    }

    /// <summary>
    /// BGMを切り替える
    /// </summary>
    /// <param name="spd">フェードアウト速度</param>
    /// <param name="delay">フェードアウト後の遅延</param>
    /// <param name="bgm">BGM名</param>
    public void FadeBGM(float spd, float delay, string bgm) {
        bgmSource.DOFade(0, spd).OnComplete(()=> {
            DOVirtual.DelayedCall(delay, ()=> {
                PlayBGM(bgm);
            });
        });
    }

    public float GetSeVol() {
        return seVol;
    }

    public float GetBgmVol() {
        return bgmVol;
    }
}
