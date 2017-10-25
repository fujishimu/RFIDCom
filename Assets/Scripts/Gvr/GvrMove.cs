using UnityEngine;
using System.Collections;

public class GvrMove : MonoBehaviour {
	public GameObject head;  // GvrMainの子要素

	public void Move () {
		gameObject.transform.position += head.transform.forward * 0.05f;
	}
}
