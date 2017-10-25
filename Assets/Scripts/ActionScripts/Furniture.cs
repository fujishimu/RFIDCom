using UnityEngine;
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
		Debug.Log("myRotate is "+Rotation.ToString());
		if(GameObject.Find("FurnitureManager").GetComponent<FurnitureManager>().isDebugMode) {
			this.transform.rotation =  Quaternion.Euler(new Vector3(0, Rotation, 0));
		} else {
			this.transform.rotation =  Quaternion.Euler(new Vector3(-90, Rotation, 0));
		}
	}
}
