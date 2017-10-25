using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.RightArrow))
		{
			this.transform.GetChild(5).gameObject.transform.Rotate(new Vector3(0,0.3f,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			this.transform.GetChild(5).gameObject.transform.Rotate(new Vector3(0,-0.3f,0));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			this.transform.GetChild(5).GetChild(0).gameObject.transform.Rotate(new Vector3(-0.3f,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			this.transform.GetChild(5).GetChild(0).gameObject.transform.Rotate(new Vector3(0.3f,0,0));
		}
		if(Input.GetKey(KeyCode.R))
		{
			this.transform.GetChild(5).gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
			this.transform.GetChild(5).GetChild(0).gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
		}
	}

	public void ChangeCamera()
	{
		if(this.transform.GetChild(0).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).gameObject.SetActive(true);
			this.transform.GetChild(2).gameObject.SetActive(false);
			this.transform.GetChild(3).gameObject.SetActive(false);
			this.transform.GetChild(4).gameObject.SetActive(false);
			this.transform.GetChild(5).gameObject.SetActive(false);
		} else if(this.transform.GetChild(1).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).gameObject.SetActive(false);
			this.transform.GetChild(2).gameObject.SetActive(true);
			this.transform.GetChild(3).gameObject.SetActive(false);
			this.transform.GetChild(4).gameObject.SetActive(false);
			this.transform.GetChild(5).gameObject.SetActive(false);
		} else if(this.transform.GetChild(2).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).gameObject.SetActive(false);
			this.transform.GetChild(2).gameObject.SetActive(false);
			this.transform.GetChild(3).gameObject.SetActive(true);
			this.transform.GetChild(4).gameObject.SetActive(false);
			this.transform.GetChild(5).gameObject.SetActive(false);
		} else if(this.transform.GetChild(3).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).gameObject.SetActive(false);
			this.transform.GetChild(2).gameObject.SetActive(false);
			this.transform.GetChild(3).gameObject.SetActive(false);
			this.transform.GetChild(4).gameObject.SetActive(true);
			this.transform.GetChild(5).gameObject.SetActive(false);
		} else if(this.transform.GetChild(4).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).gameObject.SetActive(false);
			this.transform.GetChild(2).gameObject.SetActive(false);
			this.transform.GetChild(3).gameObject.SetActive(false);
			this.transform.GetChild(4).gameObject.SetActive(false);
			this.transform.GetChild(5).gameObject.SetActive(true);
		}
		else if(this.transform.GetChild(5).gameObject.activeSelf)
		{
			this.transform.GetChild(0).gameObject.SetActive(true);
			this.transform.GetChild(1).gameObject.SetActive(false);
			this.transform.GetChild(2).gameObject.SetActive(false);
			this.transform.GetChild(3).gameObject.SetActive(false);
			this.transform.GetChild(4).gameObject.SetActive(false);
			this.transform.GetChild(5).gameObject.SetActive(false);
		}
	}
}
