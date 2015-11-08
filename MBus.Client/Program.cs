using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;
using System.Net.Http;

namespace MBus.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var serverUri = ConfigurationManager.AppSettings["server"] + "/signalr";
			var clientname = ConfigurationManager.AppSettings["clientname"];

			using (var connection = new HubConnection(serverUri))
			{
				connection.Closed += () => { Log.Info("You have been disconnected."); };
				var hubProxy = connection.CreateHubProxy("MyHub");
				hubProxy.On<string, string>("AddMessage", (userName, message) =>
				{
					Log.Content(userName + ": " + message);
				});

				try
				{
					connection.Start().Wait();
				}
				catch (AggregateException e)
				{
					Log.Error("Unable to connect to server: " + e.InnerException.Message);
					return;
				}

				Log.Suucess("Connected to server at " + serverUri);
				Log.Info("Connected as " + clientname + " with id " + connection.ConnectionId);

				var line = Console.ReadLine();
				while (string.IsNullOrWhiteSpace(line) == false)
				{
					hubProxy.Invoke("Send", clientname, line);
					line = Console.ReadLine();
				}
			}
		}
	}
}
