using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using MiniJSON;

public class TagData {
	public string UID{ set; get; }    // タグのID
	public int Row{ set; get; }       // ボード上の列
	public int Column{ set; get; }    // ボード上の行
	public bool IsSurface{ set; get; }  // タグが表か裏か
	public double Rotate{ set; get; } // 角度

	public void parseJsonData (string jsonStr) {
		IDictionary<string, object> tagDict = Json.Deserialize (jsonStr) as IDictionary<string,object>;
		UID    = (string)tagDict ["UID"];
		Column = (int)((long)tagDict ["Column"]);
		Row    = (int)((long)tagDict ["Row"]);
		Rotate = (double)tagDict ["Rotate"];
		if ((int)((long)tagDict ["Surface"]) == 1) {
			IsSurface = true;
		} else {
			IsSurface = false;
		}
	}
}
