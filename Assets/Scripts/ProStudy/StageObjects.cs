using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjects : MonoBehaviour {

    List<Coin> coinList;
    public List<Coin> CoinList { get { return coinList; } }
    List<MoveItem> moveItemList;    //テスト用
    public List<MoveItem> MoveItemList { get { return moveItemList; } }
    GoalGate goal;
    GameObject antenaBase;
	
    void Start () {
        coinList = new List<Coin>();
        moveItemList = new List<MoveItem>();
        antenaBase = GameObject.Find("AntenaBase").gameObject;

        GetSearchObjectsToList();

        goal = transform.Find("Goal").GetComponent<GoalGate>();
    }

    public void Init() {
        coinList = new List<Coin>();
        moveItemList = new List<MoveItem>();
        GetChildrenFromAntenaBase(antenaBase, ref moveItemList);
        GetSearchObjectsToList();

        foreach(MoveItem i in moveItemList) {
            i.Init();
        }

        foreach(Coin c in coinList) {
            c.Init();
        }
    }

    /// <summary>
    /// SearchObject以下にあるオブジェクトをリストへ入れる
    /// </summary>
    void GetSearchObjectsToList() {
        foreach (Transform t in transform) {
            if (t.gameObject.GetComponent<Coin>() != null) {
                Coin c = t.gameObject.GetComponent<Coin>();
                coinList.Add(c);
            }

            //SearchObjects以下のMoveItem
            if (t.gameObject.GetComponent<MoveItem>() != null) {
                MoveItem i = t.gameObject.GetComponent<MoveItem>();
                moveItemList.Add(i);
            }
        }
    }

    public void Goal() {
        goal.Goal();
    }

    /// <summary>
    /// RFIDで生成されたprefabをリストへ追加
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="allChildren"></param>
    void GetChildrenFromAntenaBase(GameObject obj, ref List<MoveItem> allChildren) {
        Transform children = obj.GetComponentInChildren<Transform>();
        if(children.childCount == 0) {
            return;
        }
        foreach(Transform ob in children) {
            if (ob.GetComponent<MoveItem>() != null) {
                allChildren.Add(ob.gameObject.GetComponent<MoveItem>());
            }
            GetChildrenFromAntenaBase(ob.gameObject, ref allChildren);
        }
    }

    /// <summary>
    /// 別のLoopItemの場所へ移動
    /// </summary>
    /// <param name="obj">プレイヤが触れたloopItem</param>
    /// <returns>移動先の座標</returns>
    public Vector3 Warp(GameObject p) {
        foreach(MoveItem i in moveItemList) {
            if(i.GetComponent<LoopItem>() != null && p != i.gameObject) {
                return i.gameObject.transform.position; 
            }
        }
        return p.transform.position;
    }
}
