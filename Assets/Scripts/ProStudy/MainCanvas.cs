using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace ProStudy {
    public class MainCanvas : MonoBehaviour {

        Button startBtn;
        Text startBtnText, clearText, coinText;
        RectTransform clearRect;

        // Use this for initialization
        void Start() {
            startBtn = transform.Find("StartBtn").GetComponent<Button>();
            startBtnText = startBtn.gameObject.transform.Find("Text").GetComponent<Text>();
            clearText = transform.Find("ClearText").GetComponent<Text>();
            clearRect = clearText.gameObject.GetComponent<RectTransform>();
            coinText = transform.Find("CoinText").GetComponent<Text>();
            
            startBtnText.text = "スタート";
            clearText.enabled = false;
            clearRect.localPosition = new Vector3(0, 350);
        }


        //初期化, ステージクリアに必要なコイン
        public void Init(int num) {
            coinText.text = "0/" + num;
        }

        //Start前
        public void OnIdle() {
            startBtnText.text = "スタート";
        }
        //Start後
        public void OnPlay() {
            startBtnText.text = "リトライ";
        }
        //Retry
        public void OnRetry() {
            OnIdle();
        }
        //タイトルへ
        public void OnTitle() {
            LoadScene();
        }
        //Goal後
        public void OnEnd() {
            clearText.enabled = true;
            if (clearRect.localPosition == new Vector3(0, 350, 0))
                clearRect.DOLocalMoveY(0, 1.0f).SetEase(Ease.OutBounce);
            //startBtnText.text = "タイトルへ";
            //ToggleBtnListener(LoadScene);
            
        }

        //現在のポイントと必要ポイント
        public void changePoint(int point, int num) {
            coinText.text = point + "/" + num;
        }

        //OnClickの関数を変更する
        public void ToggleBtnListener(UnityEngine.Events.UnityAction call) {
            startBtn.onClick.AddListener(call);
        }
        //シーンをロードする
        void LoadScene() {
            SceneManager.LoadScene("Title");
        }
    }
}
