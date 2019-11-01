using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour {

    Vector3 pos;    //初期位置
    BoxCollider2D col;
    Tween yoyo;

	// Use this for initialization
	void Start () {
        col = GetComponent<BoxCollider2D>();
        Init();
	}

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init() {
        yoyo.Kill();
        pos = this.transform.position;
        col.enabled = true;
        this.gameObject.SetActive(true);
        yoyo = this.transform.DOMoveY(-0.5f, 1.2f).SetRelative(true).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// プレイヤーが取得したときの処理
    /// </summary>
    public void GetCoin() {
        SoundManager.Instance.PlaySE("getCoin");
        col.enabled = false;
        this.transform.DOMoveY(3, 0.8f).SetRelative(true).SetEase(Ease.OutQuad).OnComplete(()=> {
            this.gameObject.SetActive(false);

        });
    }
}
