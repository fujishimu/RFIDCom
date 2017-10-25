using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

public class UdpState {
	public UdpClient UdpClient { get; set; }
	public IPEndPoint EndPoint { get; set; }
	public UdpState (UdpClient uc, IPEndPoint ipe) {
		UdpClient = uc;
		EndPoint = ipe;
	}
}
