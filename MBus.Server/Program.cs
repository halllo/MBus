using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;
using NiceConsole;
using System.IO;
using System.Reflection;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;

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

			var staticFileOptions = new StaticFileOptions
			{
				RequestPath =  new PathString("/web"),
				FileSystem = new PhysicalFileSystem(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "webdir"))
			};
			app.UseStaticFiles(staticFileOptions);
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
		public override Task OnDisconnected(bool stopCalled)
		{
			Log.Info("Client disconnected: " + Context.ConnectionId);

			return base.OnDisconnected(stopCalled);
		}
		public override Task OnReconnected()
		{
			Log.Info("Client reconnected: " + Context.ConnectionId);

			return base.OnReconnected();
		}
	}
}
