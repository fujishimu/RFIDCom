using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GoalGate : MonoBehaviour {

    SkeletonAnimation goalAnimLeft = null;
    SkeletonAnimation goalAnimRight = null;

    // Use this for initialization
    void Start () {
        goalAnimLeft = transform.Find("SpineObjLeft").gameObject.GetComponent<SkeletonAnimation>();
        goalAnimRight = transform.Find("SpineObjRight").gameObject.GetComponent<SkeletonAnimation>();
        goalAnimLeft.timeScale = 0.5f;
        goalAnimRight.timeScale = 0.5f;
        goalAnimLeft.state.SetAnimation(0, "Idle", false);
        goalAnimRight.state.SetAnimation(0, "Idle", false);
    }

    /// <summary>
    /// プレイヤーがゴールしたときに呼ぶ
    /// </summary>
    public void Goal() {
        if (goalAnimLeft.state.GetCurrent(0).Animation.name != "LeftOpen" && goalAnimRight.state.GetCurrent(0).Animation.name != "RightOpen") {
            goalAnimLeft.state.SetAnimation(0, "LeftOpen", false);
            goalAnimRight.state.SetAnimation(0, "RightOpen", false);
            SoundManager.Instance.PlaySE("door");
        }
    }
}
