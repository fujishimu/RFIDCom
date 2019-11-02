using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakGround : MonoBehaviour {

    SpriteRenderer sp;
    BoxCollider2D bc;
	
    void Start () {
        sp = transform.Find("New Sprite").GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        this.gameObject.SetActive(true);
	}

    /// <summary>
    /// 乗った床は消える
    /// </summary>
    public void Break() {
        bc.enabled = false;
        DOTween.ToAlpha(
            () => sp.color,
            color => sp.color = color,
            0f, 1f).OnComplete(() => {
                this.gameObject.SetActive(false);
            });
    }

}
