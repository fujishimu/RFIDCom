﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Furniture : MonoBehaviour {

    float _afterRote;
	public bool IsReverse{ set; get;}
	public string UID {set; get;}
    public float Rotation { 
		get { 
			return _afterRote;
		}
		set { 
			_afterRote = value; 
		}
	}

	public void UpdateData() {
		//Debug.Log("myRotate is "+Rotation.ToString());
		if(GameObject.Find("FurnitureManager").GetComponent<FurnitureManager>().isDebugMode) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0, Rotation, 0));
		} else {
            string sceneName = SceneManager.GetActiveScene().name;
            if ( sceneName== "ComScene2") {
                //this.transform.rotation = Quaternion.Euler (new Vector3 (0, Rotation, 0));
            }else if(sceneName == "PSStage1" || sceneName == "PSStage2" || sceneName == "PSStage3") {
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Rotation));
            } else {
				this.transform.rotation = Quaternion.Euler(new Vector3(-90, Rotation, 0));
			}
		}
	}
}
