using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using MiniJSON;

namespace Vr_rfid
{
	public class MainScript : MonoBehaviour {
		public int receivePort = 18188;
		public int _portNumber = 18188;
		public GameObject furnitureManagerObj;
		public GameObject CameraManagerObj;

		private FurnitureManager furnitureManager;
		private CameraManager cameraManager;
		private UdpClient udpClient;
		private IPEndPoint endPoint;               // IPアドレスを表す
		private TagData tagData = new TagData ();  // 受信したタグの情報 
		private IList<TagData> preTagList = new List<TagData>();     // 前回のUIDリスト
		private IList<TagData> receiveTagList = new List<TagData>(); // UDP受診時のUIDリスト
		private bool isUdpReceived = false;

		// TODO: フラグが複数あるのイケてない気がする
		private bool isScanFinish;  // 全マスのスキャンが終了したか
		private bool _addFunction;

		void Start () {
			Debug.Log ("Start MainScript");
			furnitureManager = furnitureManagerObj.GetComponent<FurnitureManager> ();
			cameraManager = CameraManagerObj.GetComponent<CameraManager> ();
			var _oM = ObjectManager.Instance;

			/*Android用*/
//			preTagList = new List<TagData> ();
//			receiveTagList = new List<TagData> ();
//			AndroidJavaClass plugin = new AndroidJavaClass("cps.kalabu.homeapplicationandroid.MainActivity");
//			plugin.CallStatic("init", receivePort);
//			Debug.Log("Init Android");
			/*PC用*/
			endPoint = new IPEndPoint (IPAddress.Any, receivePort);
			udpClient = new UdpClient (endPoint);
			UdpState state = new UdpState (udpClient, endPoint);
			udpClient.BeginReceive (ReceiveCallback, state);
		}

		void OnDisable () {
			udpClient.Close ();
		}

		void Update () {
			if (isUdpReceived) {
				isUdpReceived = false;
				UdpState state = new UdpState (udpClient, endPoint);
				udpClient.BeginReceive (ReceiveCallback, state);
			}

			if(isScanFinish) {
				FinishScanFunction();
				isScanFinish = false;
			} else if(_addFunction) {
				AddTagData(tagData);
				_addFunction = false;
				tagData = null;
			}

			if (Input.GetKeyDown (KeyCode.Space)) {
				cameraManager.ChangeCamera ();
			}
		}

		/// <summary>
		/// UDPを受信した時に呼ばれる 
		/// </summary>
		private void ReceiveCallback (IAsyncResult AR) {
			UdpClient  u = (UdpClient)((UdpState)(AR.AsyncState)).UdpClient;
			IPEndPoint e = (IPEndPoint)((UdpState)(AR.AsyncState)).EndPoint;

			Byte[] dat = u.EndReceive (AR, ref e);
			String receiveData = System.Text.Encoding.ASCII.GetString (dat);
			Debug.Log (receiveData);

			/*全マス目のスキャン完了時*/
			if (receiveData == "Finish") {
				//FinishScanFunction ();
				isUdpReceived = true;
				isScanFinish = true;
				return;
			}
			SetUDPData (receiveData);
		}

		/// <summary>
		/// 全アンテナスキャン時のアクション 
		/// </summary>
		private void FinishScanFunction () {
			// 前回データの家具の削除
			foreach (TagData _target in preTagList) {
				Debug.Log("Delete");
				furnitureManager.DeleteFurniture (_target.UID);
			}
			// 前のタグリストを格納
			preTagList = new List<TagData> (receiveTagList);
			receiveTagList.Clear ();
		}

		/// <summary>
		/// 前回のデータとの比較を行いデータをリストへ格納 
		/// </summary>
		private void AddTagData (TagData addTagData) {
			TagData _target = null;
			foreach (TagData preTag in preTagList) {
				if (preTag.UID == addTagData.UID) {
					_target = preTag;
				}
			}
			if(_target != null) {
				preTagList.Remove (_target);
			}
			// 家具データの生成及び更新
			furnitureManager.ArrangeFurnitureIfNeeded (addTagData);
			receiveTagList.Add (addTagData);
		}

		/// <summary>
		/// タグ情報のセット
		/// </summary>
		public void SetUDPData(string receiveData) {
			Debug.Log(receiveData);
			tagData = new TagData ();
			tagData.parseJsonData (receiveData);
			_addFunction = true;
			isUdpReceived = true;
		}
	}
}