using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	GameObject player;

	void Start () {
		player = gameObject.transform.root.gameObject;
	}

	void OnTriggerEnter(Collider col)
	{
        if (col.tag == "PlayerArrow")
        {
			if(player.GetComponent<Player>().getState() != 1)player.GetComponent<Player> ().SpineState(1);
        }
    }
	void OnTriggerExit(Collider col) 
	{
		if (col.tag == "PlayerArrow")
		{
			player.GetComponent<Player> ().SpineState(0);	
		}
	}

}
