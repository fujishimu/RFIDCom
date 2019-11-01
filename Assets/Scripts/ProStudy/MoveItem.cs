using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProStudy;

public abstract class MoveItem : MonoBehaviour {

    protected SpriteRenderer sr;
    public SpriteRenderer Sr { get { return sr; } }
    protected BoxCollider2D bc;
    public BoxCollider2D Bc { get { return bc; } }
    bool isWall = false;

    protected virtual void Start() {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init() {
        if (!isWall && this.gameObject.activeSelf) {
            sr.enabled = true;
            bc.enabled = true;
        }
    }

    /// <summary>
    /// プレイヤーに触れたら消す
    /// </summary>
    public void Delete() {
        sr.enabled = false;
        bc.enabled = false;
    }

    void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Wall" || collision.tag == "JumpItem" || collision.tag == "ClimbItem" || collision.tag == "LoopItem") {
            isWall = true;
            //Delete();
        }
    }
}
