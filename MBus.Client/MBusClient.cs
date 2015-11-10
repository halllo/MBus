using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace MBus.Client
{
	public class MBusClient : IDisposable
	{
		string _clientname;

		HubConnection _connection;
		IHubProxy _hubProxy;

		public MBusClient(string clientname)
		{
			_clientname = clientname;
		}

		public async Task Connect(string uri)
		{
			var connection = new HubConnection(uri);
			connection.Closed += () =>
			{
				var eh = OnDisconnect;
				if (eh != null) eh();
			};
			var hubProxy = connection.CreateHubProxy("MyHub");
			hubProxy.On<string, string>("AddMessage", (userName, message) =>
			{
				var eh = On;
				if (eh != null) eh(userName, message);
			});
			try
			{
				await connection.Start();
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}


			_connection = connection;
			_hubProxy = hubProxy;
		}

		public string ConnectionId
		{
			get { return _connection.ConnectionId; }
		}

		public void Dispose()
		{
			_connection.Dispose();
		}

		public void Emit(string message)
		{
			_hubProxy.Invoke("Send", _clientname, message);
		}

		public event Action<string, string> On;
		public event Action OnDisconnect;
	}
}
