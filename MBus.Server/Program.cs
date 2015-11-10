using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;
using NiceConsole;

namespace MBus.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var serverUri = ConfigurationManager.AppSettings["server"];

			Log.Info("Starting server...");
			using (var signalR = WebApp.Start(serverUri))
			{
				Log.Suucess("Server started at " + serverUri);

				Console.ReadLine();
			}
		}
	}

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
			app.UseWelcomePage();
		}
	}

	public class MyHub : Hub
	{
		public void Send(string userName, string message)
		{
			Log.Content("Message from " + userName + ": " + message);
			Clients.All.addMessage(userName, message);
		}
		public override Task OnConnected()
		{
			Log.Info("Client connected: " + Context.ConnectionId);

			return base.OnConnected();
		}
		public override Task OnDisconnected()
		{
			Log.Info("Client disconnected: " + Context.ConnectionId);

			return base.OnDisconnected();
		}
	}
}
