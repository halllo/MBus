using System;
using System.Configuration;
using NiceConsole;

namespace MBus.Client.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var serverUri = ConfigurationManager.AppSettings["server"] + "/signalr";
			var clientname = ConfigurationManager.AppSettings["clientname"];

			using (var mbus = new MBusClient(clientname))
			{

				//setup
				{
					mbus.OnDisconnect += () => { Log.Info("You have been disconnected."); };
					mbus.On += (userName, message) => { Log.Content(prefix: userName + ": ", text: message); };

					try
					{
						mbus.Connect(serverUri).Wait();
					}
					catch (Exception e)
					{
						Log.Error("Unable to connect to server: " + e.Message);
						return;
					}

					Log.Suucess("Connected to server at " + serverUri);
					Log.Info("Connected as " + clientname + " with id " + mbus.ConnectionId);
				}


				//emit
				{
					var message = System.Console.ReadLine();
					while (string.IsNullOrWhiteSpace(message) == false)
					{
						mbus.Emit(message: message);
						message = System.Console.ReadLine();
					}
				}

			}
		}
	}


}
