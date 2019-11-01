using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopItem : MoveItem {

    public enum InOutLoopEnum {
        In,
        Out,
    }
    [SerializeField]
    InOutLoopEnum inOut;
    public InOutLoopEnum InOut { get { return inOut; } }

    bool isActive = false;
    public bool IsActive { get { return isActive; } }


    override protected void Start() {
        base.Start();
        if(inOut == InOutLoopEnum.In) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255);
        }
    }

    /// <summary>
    /// プレイヤが触れると実行される
    /// </summary>
    public void Action() {
        if (inOut == InOutLoopEnum.Out) {
            if (isActive) {
                Delete();
            } else {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255);
                bc.enabled = false;
                isActive = true;
            }
        }
    }
}
