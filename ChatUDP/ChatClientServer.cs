using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatUDP
{
	public delegate void ShowMessage(string message);
    class ChatClientServer
    {
		private int localPort = 4000;
		private int remotePort = 4000;
		private string listernIP = "13.0.0.6";
		private Socket listeningSocket;
		public ShowMessage showMessage;
		public void Start()
		{		
			try
			{				
				listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				listeningSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
				Task listeningTask = new Task(Listen);
				listeningTask.Start();
				Thread.Sleep(1000);
				byte[] data = Encoding.Unicode.GetBytes("Start server...");
				//EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("10.10.6.6"), remotePort);
				EndPoint remotePoint = new IPEndPoint(IPAddress.Broadcast, remotePort);
				listeningSocket.SendTo(data, remotePoint);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			//finally
			//{
			//	CloseSocket();
			//}
		}

		public void Send(string message)
		{			
			byte[] data = Encoding.Unicode.GetBytes(message);
			//EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("10.10.6.6"), remotePort);
			EndPoint remotePoint = new IPEndPoint(IPAddress.Broadcast, remotePort);
			listeningSocket.SendTo(data, remotePoint);			
		}

		

		private void Listen()
		{
			try
			{
				//прослушиваем по адресу
				IPEndPoint localIp = new IPEndPoint(IPAddress.Parse(listernIP), localPort);
				listeningSocket.Bind(localIp);
				while (true)
				{
					// полученеие сообщения 
					StringBuilder builder = new StringBuilder();
					int bytes = 0;
					byte[] data = new byte[256];
					EndPoint remoteIp = new IPEndPoint(IPAddress.Any, remotePort);
					do
					{
						bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
						builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
					} while (listeningSocket.Available > 0);
					IPEndPoint remoteFullIp = remoteIp as IPEndPoint;		
				
					showMessage($"{remoteFullIp.Address}: {remoteFullIp.Port}");
					showMessage($" >>> {builder.ToString()}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				CloseSocket();
			}
		}

		private void CloseSocket()
		{
			listeningSocket?.Shutdown(SocketShutdown.Both);
			listeningSocket?.Close();
			listeningSocket = null;
		}
	}
}
