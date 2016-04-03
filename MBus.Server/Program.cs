using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using NiceConsole;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

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
			app.UseWebApi(ConfigureWebApi());

			var staticFileOptions = new StaticFileOptions
			{
				RequestPath = new PathString("/web"),
				FileSystem = new PhysicalFileSystem(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "webdir"))
			};
			app.UseDefaultFiles(new DefaultFilesOptions
			{
				RequestPath = staticFileOptions.RequestPath,
				FileSystem = staticFileOptions.FileSystem,
			});
			app.UseStaticFiles(staticFileOptions);
		}

		private HttpConfiguration ConfigureWebApi()
		{
			var config = new HttpConfiguration();

			config.Routes.MapHttpRoute("DefaultApi",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional });

			return config;
		}
	}


	public class RelayController : ApiController
	{
		// GET /api/relay
		public IEnumerable<string> Get()
		{
			var relayServerUri = ConfigurationManager.AppSettings["relayserver"];
			return new[] { relayServerUri };
		}
	}


	public class MyHub : Hub
	{
		public void Send(string userName, string message)
		{
			Log.Content(prefix: "Message from " + userName + ": ", text: message);
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
