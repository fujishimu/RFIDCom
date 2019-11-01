using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine;
using Spine.Unity;
using System;

namespace ProStudy {
    public class Player : MonoBehaviour {

        Vector3 playerInitPos;  //プレイヤーの初期位置
        float spd = 0.03f;  //横移動速度
        float downSpd = 0.06f;   //落下速度
        float climbSpd = 0.02f; //右斜め前移動速度
        float rayLength = 4.0f; //rayの長さ
        bool isAllowClimb = true;    //右斜め前にジャンプできるか
        bool isAllowJump = true;    //上にジャンプできるか
        bool isGoal = false;    //ゴール条件を満たしているか
        StageObjects stageObjects;

        SkeletonAnimation playerAnim = null;
        Tween climbTween, jumpTween;

        PLAYER_STATE beforeState;

        int layer;  
        public enum PLAYER_STATE {
            Walk,
            Climb,
            Jump,
            Idle,
            Down,
            Goal
        }
        PLAYER_STATE state;
        public PLAYER_STATE State {
            get { return state; }
        }


        // Use this for initialization
        void Start() {
            //rayがぶつかるlayerを設定
            layer = LayerMask.GetMask(new string[] { "Wall", "Goal" });

            playerAnim = transform.Find("SpineObject").GetComponent<SkeletonAnimation>();
            playerAnim.AnimationState.Event += HandleEvent;

            state = PLAYER_STATE.Idle;
            beforeState = state;

            playerInitPos = transform.position;
        }

        // Update is called once per frame
        void Update() {
            //rayを飛ばして壁を検知
            RaycastHit2D rHit = Physics2D.Raycast(transform.position, Vector2.right, rayLength / 2.5f, layer); //右方向
            RaycastHit2D uHit = Physics2D.Raycast(transform.position + new Vector3(0, 1.0f), Vector2.up, rayLength, layer); //上方向
            RaycastHit2D ruHit = Physics2D.Raycast(transform.position + new Vector3(1.0f, 1.0f), new Vector2(1.0f, 1.0f), rayLength, layer); //右上方向 
            //デバッグ用表示
            Debug.DrawRay(transform.position, Vector2.right * rayLength / 2.5f);
            Debug.DrawRay(transform.position + new Vector3(0, 2.0f), Vector2.up * rayLength);
            Debug.DrawRay(transform.position + new Vector3(1.0f, 1.0f), new Vector2(1.0f, 1.0f) * rayLength);

            if (GameManager.instance.State == GameManager.STATE.Play) {
                //壁にぶつかったら止まる
                if (rHit.collider != null && rHit.collider.tag == "Wall") {
                    state = PLAYER_STATE.Idle;
                }
                //ゴールできないとき
                if(rHit.collider != null && rHit.collider.tag == "Goal" && GameManager.instance.CurrentPoint != stageObjects.CoinList.Count) {
                    state = PLAYER_STATE.Idle;
                }
                //右上にブロックがあったら飛べない
                if (ruHit.collider != null && ruHit.collider.tag == "Wall") {
                    isAllowClimb = false;
                } else {
                    isAllowClimb = true;
                }
                //上にブロックがあったら飛べない
                if (uHit.collider != null && uHit.collider.tag == "Wall") {
                    isAllowJump = false;
                } else {
                    isAllowJump = true;
                }
            }

            switch (state) {
                case PLAYER_STATE.Idle:
                    Idle();
                    break;
                case PLAYER_STATE.Walk:
                    Walk();
                    break;
                case PLAYER_STATE.Climb:
                    Climb();
                    break;
                case PLAYER_STATE.Jump:
                    Jump();
                    break;
                case PLAYER_STATE.Down:
                    Down();
                    break;
                case PLAYER_STATE.Goal:
                    transform.position += new Vector3(spd, 0, 0);
                    break;
                default:
                    break;
            }
            
            ///デバッグメッセージ
            if(beforeState != state) {
                Debug.Log("NowState:" + state);
                beforeState = state;
            }
        }

        /// <summary>
        /// stateをMoveへ変更
        /// </summary>
        public void StartGame() {
            state = PLAYER_STATE.Walk;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(StageObjects s) {
            this.stageObjects = s;
            climbTween.Kill();
            jumpTween.Kill();
            transform.position = playerInitPos;
            state = PLAYER_STATE.Idle;
        }
        void InitTween(string id) {
            if (DOTween.TweensById(id) != null) {
                DOTween.TweensById(id).ForEach((tween) => {
                    Debug.Log(tween);
                    tween.Kill();
                });
            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        void Idle() {
            if (playerAnim.state.GetCurrent(0).Animation.Name != "Idle") {
                playerAnim.timeScale = 1.0f;
                playerAnim.state.SetAnimation(0, "Idle", true);
            }
        }

        /// <summary>
        /// 横移動
        /// </summary>
        void Walk() {
            transform.position += new Vector3(spd, 0, 0);
            WalkAnim();
        }
        void WalkAnim() {
            if(playerAnim.state.GetCurrent(0).Animation.Name == "Idle" || 
                (playerAnim.timeScale != 1.0f)) {
                playerAnim.timeScale = 1.0f;
                playerAnim.state.SetAnimation(0, "Walk", true);
            }
        }

        /// <summary>
        /// 右斜め前に飛ぶ
        /// </summary>
        void Climb() {
            if (!isAllowClimb) {
                state = PLAYER_STATE.Idle;
                return;
            }
            if (playerAnim.state.GetCurrent(0).Animation.Name != "Climb") {
                playerAnim.timeScale = 2.0f;
                playerAnim.state.SetAnimation(0, "Climb", false);
            }
        }
        /// <summary>
        /// JumpアニメーションのJumpEventに合わせて飛ぶ
        /// </summary>
        void ClimbCallBack() {
            climbTween = transform.DOJump(new Vector3(5.0f, 4.0f), 1.0f, 1, 1.0f).SetRelative(true).OnComplete(() => {
                state = PLAYER_STATE.Down;
            });
        }

        /// <summary>
        /// 上にジャンプ
        /// </summary>
        void Jump() {
            if (!isAllowJump) {
                state = PLAYER_STATE.Idle;
                return;
            }
            if (playerAnim.state.GetCurrent(0).Animation.Name != "Jump") {
                playerAnim.timeScale = 2.0f;
                playerAnim.state.SetAnimation(0, "Jump", false);
            }
        }

        /// <summary>
        /// JumpアニメーションのJumpEventに合わせて飛ぶ
        /// </summary>
        void JumpCallBack() {
            jumpTween = transform.DOMove(new Vector3(0.1f, 4.0f), 0.4f).SetRelative(true).SetId("Jump").OnComplete(() => {
                state = PLAYER_STATE.Down;
            });
        }

        /// <summary>
        /// 落下
        /// </summary>
        void Down() {
            transform.position -= new Vector3(0, downSpd, 0);
            if (playerAnim.state.GetCurrent(0).Animation.Name != "Walk" && state == PLAYER_STATE.Down) {
                playerAnim.timeScale = 3.0f;
                playerAnim.state.SetAnimation(0, "Walk", true);
            }
        }

        /// <summary>
        /// ゴール
        /// </summary>
        public void Goal() {
            WalkAnim();
        }

        //アイテム,ゴール,床(Wall)を検知
        void OnTriggerEnter2D(Collider2D collision) {
            if (GameManager.instance.State == GameManager.STATE.Play && state != PLAYER_STATE.Goal) {
                switch(collision.tag) {
                    case "Coin":
                        collision.gameObject.GetComponent<Coin>().GetCoin();
                        GameManager.instance.AddPoint();
                        break;
                    case "Wall":
                        state = PLAYER_STATE.Walk;
                        break;
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision) {
            //落下判定
            if (GameManager.instance.State == GameManager.STATE.Play && state !=PLAYER_STATE.Climb && state != PLAYER_STATE.Jump && state != PLAYER_STATE.Goal) {
                if(collision.tag == "Wall") {
                    state = PLAYER_STATE.Down;
                    playerAnim.timeScale = 3.0f;
                    playerAnim.state.SetAnimation(0, "Walk", true);
                    //SoundManager.Instance.PlaySE("falling");
                }
            }
        }

        void OnTriggerStay2D(Collider2D collision) {
            //ゴール後の状態を固定
            if (collision.tag == "Goal") {
                state = PLAYER_STATE.Goal;
                return;
            }
            if (GameManager.instance.State == GameManager.STATE.Play && state != PLAYER_STATE.Jump && state != PLAYER_STATE.Climb && state != PLAYER_STATE.Goal) {
                //ジャンプまたはクライム中に別行動取らない
                if (state != PLAYER_STATE.Climb && state != PLAYER_STATE.Jump) {
                    if (collision.tag == "JumpItem") {
                        collision.GetComponent<MoveItem>().Delete();
                        state = PLAYER_STATE.Jump;
                        return;
                    }
                    if (collision.tag == "ClimbItem") {
                        collision.GetComponent<MoveItem>().Delete();
                        state = PLAYER_STATE.Climb;
                        return;
                    }
                    if (collision.tag == "LoopItem") {
                        LoopItem li = collision.GetComponent<LoopItem>();
                        li.Action();
                        Warp(li);
                        return;
                    }
                }

                //落下後に床に触れる
                if(collision.tag == "Wall") {
                    state = PLAYER_STATE.Walk;
                    return;
                }
            }
        }


        void HandleEvent(TrackEntry trackEntry, Spine.Event e) {
            if(e.Data.Name == "Jump") {
                if (state == PLAYER_STATE.Climb) {
                    ClimbCallBack();
                } else if (state == PLAYER_STATE.Jump) {
                    JumpCallBack();
                }
                SoundManager.Instance.PlaySE("robotFootstep");
            }
            if (e.Data.Name == "WalkOnGround" && state == PLAYER_STATE.Walk) {
                SoundManager.Instance.SetSeVol(0.2f);
                SoundManager.Instance.PlaySE("robotFootstep");
            }
        }

        /// <summary>
        /// LoopOutItemへ移動
        /// </summary>
        /// <param name="p">プレイヤが触れたloopItem</param>
        /// <returns>移動先の座標</returns>
        public void Warp(LoopItem p) {
            foreach (MoveItem i in stageObjects.MoveItemList) {
                if (i.GetComponent<LoopItem>() != null && p.InOut == LoopItem.InOutLoopEnum.In && !i.gameObject.Equals(p.gameObject) && i.GetComponent<LoopItem>().IsActive) {
                    i.Delete();
                    p.Delete();
                    climbTween.Kill();
                    jumpTween.Kill();
                    this.transform.position = i.gameObject.transform.position;
                }
            }
        }
    }
}
