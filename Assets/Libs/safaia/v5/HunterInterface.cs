using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;  
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace hunter.UnityV5
{
	public class HunterInterface : MonoBehaviour
	{
		// 请修改一下常量为您所在项目组游戏代号，如g18、h46，字母都为小写
		public string GAME_ID = "ma68";

		// ip和port一般不用改
		public string HUNTER_DEVICE_AP = "192.168.40.111";
		public int HUNTER_DEFAULT_PORT = 29001;

		public const int DEVICE_CONNECT = 0x0010;
		public const int DEVICE_REJECT = 0x0011;
		public const int DEVICE_ACCEPT = 0x0012;
		public const int DEVICE_RUN_SCRIPT = 0x0015;
		public const int DEVICE_OUT = 0x0018;
		private string OS;
		private string uid;

		public struct PacketHeader
		{
			public int protocolId;
			public int payloadLen;

			public PacketHeader (int protocolId, int payloadLen)
			{
				this.protocolId = protocolId;
				this.payloadLen = payloadLen;
			}

			public PacketHeader (byte[] header)
			{
				this.protocolId = BitConverter.ToInt32 (header, 0);
				this.payloadLen = BitConverter.ToInt32 (header, 4);
			}

			public byte[] Serialize ()
			{
				byte[] pid = BitConverter.GetBytes (protocolId);
				byte[] len = BitConverter.GetBytes (payloadLen);
				if (!BitConverter.IsLittleEndian) {
					Array.Reverse (pid);
					Array.Reverse (len);
				}
				return pid.Concat (len).ToArray ();
			}
		}
		Socket clientSocket;
		List<byte> buf;

		// Use this for initialization
		void Start ()
		{
			OS = SystemInfo.operatingSystem.Split(' ')[0];
			uid = SystemInfo.deviceUniqueIdentifier;
			if (uid.Length > 32) {
				uid = uid.Substring (uid.Length - 32);
			}
			Debug.Log (uid);

			buf = new List<byte> ();
			buf.Capacity = 4096;

			// establish tcp socket
			var client = new TcpClient();
			try 
			{
				var result = client.BeginConnect(HUNTER_DEVICE_AP, HUNTER_DEFAULT_PORT, null, null);
	            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(0.5));
	            if (success)
	            {
	            	// we have connected
	                client.EndConnect(result);
	            }
			} catch {}
            clientSocket = client.Client;
			clientSocket.Blocking = false;
			if (clientSocket.Connected)
			{
				Debug.Log ("connect success.");
			}
			else 
			{
				Debug.LogError ("fail to connect to hunter");  
			}

			// connect protocol 
			var connBody = new Dictionary<string, string> ();
			connBody.Add ("name", "Unity3D");  // 起一个默认名字，在hunter中这个名字可以修改
			connBody.Add ("gameId", GAME_ID);  // 游戏代号
			connBody.Add ("owner", "");  // 无法确定owner时可用空字符串代替
			connBody.Add ("os", OS);
			connBody.Add ("engine_name", "Unity3d");
			connBody.Add ("script_lang", "csharp");
			connBody.Add ("uid", uid);
			Send (DEVICE_CONNECT, connBody);

			// set log callback
			Application.logMessageReceived += HandleLog;
		}

		void OnDisable ()
		{
			Application.logMessageReceived -= HandleLog;
		}

		// Update is called once per frame
		void Update ()
		{
			if (!clientSocket.Connected) {
				return;
			}

			try {
				byte[] rxbuf = new byte[4096];
				var bytesReceived = clientSocket.Receive (rxbuf, rxbuf.Length, SocketFlags.None);
				for (var i = 0; i < bytesReceived; ++i) {
					buf.Add (rxbuf [i]);
				}
			} catch (SocketException e) {
				//Debug.Log (e.ToString ());
			}
			if (buf.Count >= 8) {
				var headerBytes = buf.GetRange (0, 8).ToArray ();
				var pkgHeader = new PacketHeader (headerBytes);
				if (buf.Count >= 8 + pkgHeader.payloadLen) {
					var payloadBytes = buf.GetRange (8, pkgHeader.payloadLen).ToArray ();
					buf.RemoveRange (0, 8 + pkgHeader.payloadLen);
					var payload = (Hashtable)hunter.NGUIJson.jsonDecode (Encoding.UTF8.GetString (payloadBytes));
					if (pkgHeader.protocolId == DEVICE_RUN_SCRIPT) {
						// do sth to handle run script
					}
					Debug.Log (Encoding.UTF8.GetString (payloadBytes));
				}
			}
		}

		void Awake ()
		{
			DontDestroyOnLoad (this);
		}

		void OnDestory ()
		{
			clientSocket.Shutdown (SocketShutdown.Both);
			clientSocket.Close ();
		}

		void Send (int pid, Dictionary<string, string> content)
		{
			var payload = Encoding.UTF8.GetBytes (hunter.NGUIJson.jsonEncode (content));
			var pkg = new PacketHeader (pid, payload.Length);
			var data = pkg.Serialize ().Concat (payload).ToArray ();
			var bytesSent = 0;
			while (bytesSent < data.Length) {
				bytesSent += clientSocket.Send (data, bytesSent, data.Length - bytesSent, SocketFlags.None);
			}
		}

		void HandleLog (string condition, string stacktrace, LogType type)
		{
			var outBody = new Dictionary<string, string> ();
			outBody.Add ("type", type.ToString ());
			outBody.Add ("data", condition);
			Send (DEVICE_OUT, outBody);
			if (type == LogType.Error || type == LogType.Exception) {
				var tb = new Dictionary<string, string> ();
				tb.Add ("type", "Traceback");
				tb.Add ("data", stacktrace);
				Send (DEVICE_OUT, tb);
			}
		}

	}
}
