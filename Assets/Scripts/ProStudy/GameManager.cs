using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProStudy;
using Vr_rfid;
using UnityEngine.SceneManagement;

namespace ProStudy {
    public class GameManager : MonoBehaviour {

        public static GameManager instance;

        private void Awake() {
            if(instance == null) {
                instance = this;
            }
        }

        public int stageNum = 0;    //ステージ番号
        int currentPoint;   //現在のポイント
        public int CurrentPoint { get { return currentPoint; } }
        
        MainCanvas canvas;
        Player player;
        MainScript mainScript;
        StageObjects stageObjects;

        public enum STATE {
            Idle,
            Play,
            End,
            None
        }
        STATE state;
        public STATE State {
            get { return state;}
        }


        void Start() {
            canvas = GameObject.FindWithTag("MainCanvas").gameObject.GetComponent<MainCanvas>();
            player = transform.Find("ProPlayer").GetComponent<Player>();
            mainScript = GameObject.Find("MainObject").GetComponent<MainScript>();
            stageObjects = GameObject.Find("StageObjects").GetComponent<StageObjects>();

            stageObjects.Init();
            player.Init(stageObjects);
            currentPoint = 0;
            canvas.Init(stageObjects.CoinList.Count);
            state = STATE.Idle;

            SoundManager.Instance.SetBgmVol(0.15f);
            SoundManager.Instance.PlayBGM("bgm1");
        }

        void Update() {
            if (player.State == Player.PLAYER_STATE.Goal && currentPoint == stageObjects.CoinList.Count && state != STATE.End) {
                state = STATE.End;
                SoundManager.Instance.SetSeVol(0.4f);
                SoundManager.Instance.PlaySE("correct");
                SoundManager.Instance.PlaySE("trumpet");
                End();
            }

            if (currentPoint == stageObjects.CoinList.Count) {
                stageObjects.Goal();
            }
        }


        public void StartGame() {
            stageObjects.Init();
            canvas.OnPlay();
            canvas.ToggleBtnListener(Retry);
            player.StartGame();
            mainScript.IsUpdateScan = false;
            state = STATE.Play;
        }

        public void Retry() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            /*--リトライを連続してくとなぜか重くなるのでシーンを読み込み直している
            stageObjects.Init();
            canvas.OnIdle();
            canvas.ToggleBtnListener(StartGame);
            player.Init(stageObjects);
            mainScript.IsUpdateScan = true;
            currentPoint = 0;
            state = STATE.Idle;
            */
        }

        public void End() {
            SoundManager.Instance.StopBGM(0.1f);
            canvas.OnEnd();
            player.Goal();
            mainScript.IsUpdateScan = false;
        }

        public void Title() {
            canvas.OnTitle();
        }

        public void AddPoint() {
            currentPoint++;
            canvas.changePoint(currentPoint, stageObjects.CoinList.Count);
        }

        public void Click() {
            SoundManager.Instance.PlaySE("click");
        }
    }
}
