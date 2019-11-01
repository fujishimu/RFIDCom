using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

    GameObject canvas;
    Button stage1Btn;

	// Use this for initialization
	void Start () {
        //canvas = GameObject.Find("Canvas").gameObject;
        //stage1Btn = canvas.transform.Find("Buttons/Stage1Btn").GetComponent<Button>();
    }

    //シーンのロード
    public void LoadScene(int stageNum) {
        SceneManager.LoadScene("PSStage" + stageNum.ToString());
    }
}
