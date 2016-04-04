using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(MBus.Relay.Startup))]

namespace MBus.Relay
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
			app.UseWebApi(ConfigureWebApi());
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

	public class MBusController : ApiController
	{
		// POST /api/mbus
		public void Post([FromBody]Message message)
		{
			if (message != null && !string.IsNullOrWhiteSpace(message.Sender) && !string.IsNullOrWhiteSpace(message.Content))
			{
				var myHub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
				myHub.Clients.All.addMessage(message.Sender, message.Content);
			}
		}
		public class Message
		{
			public string Sender { get; set; }
			public string Content { get; set; }
		}
	}

	public class MyHub : Hub
	{
		public void Send(string userName, string message)
		{
			Clients.All.addMessage(userName, message);
		}
		public override Task OnConnected()
		{
			return base.OnConnected();
		}
		public override Task OnDisconnected(bool stopCalled)
		{
			return base.OnDisconnected(stopCalled);
		}
		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}
	}
}
