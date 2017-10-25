using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class RayCastScript : MonoBehaviour {

	RaycastHit _hit;
	public GameObject _time;
	bool _isTimerStart;
	float _timeNum;

	int _taskNum;
	// Use this for initialization
	void Start () {
		_taskNum = 1;
		_isTimerStart = false;
		_timeNum = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		Ray _ray = new Ray (this.transform.position, this.transform.forward *100);

		if (Physics.Raycast (_ray, out _hit)) {
			Debug.Log (_hit.collider.gameObject.name);
			if(_hit.collider.gameObject.GetComponent<TextMesh>().text == _taskNum.ToString())
			{
				Destroy(_hit.collider.gameObject);
				_taskNum++;
				if(!_isTimerStart){
					_isTimerStart = true;
					StartCoroutine("Timer");
				}
			}
			if(_taskNum ==10)
			{
				_isTimerStart = false;
			}
		}
		Debug.DrawRay (transform.position, transform.forward*100, Color.green);
	}

	IEnumerator Timer()
	{
		while (_isTimerStart) {
			_timeNum+=1f;
			_time.GetComponent<TextMesh>().text = _timeNum.ToString();
			Debug.Log(_timeNum);
			yield return new WaitForSeconds(1f);
		}
	}
}
