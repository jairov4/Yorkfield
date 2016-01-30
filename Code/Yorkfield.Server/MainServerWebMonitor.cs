using Nancy;
using Yorkfield.Core;

namespace Yorkfield.Server
{
	public class MainServerWebMonitor : NancyModule
	{
		public MainServerWebMonitor(IServer server, string path) : base(path)
		{
			Get["/"] = _ => View["MainServerWebMonitor", server.PollCurrentStatus()];
		}
	}
}