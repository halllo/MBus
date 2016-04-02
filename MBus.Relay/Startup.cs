using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(MBus.Relay.Startup))]

namespace MBus.Relay
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
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
