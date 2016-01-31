using System.Collections.Generic;
using System.Linq;
using Nancy;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	public class MainServerWebMonitor : NancyModule
	{
		public MainServerWebMonitor(IServer server, string path) : base(path)
		{
			RequiresNotNull(server);
			RequiresNotNull(path);
			Get["/"] = _ => View["MainServerWebMonitor", GetModel(server)];
		}

		private Model GetModel(IServer server)
		{
			RequiresNotNull(server);
			var r = server.PollCurrentStatus();
			var model = new Model(r);
			return model;
		}

		public class Model
		{
			public Model(ServerStatus state)
			{
				RequiresNotNull(state);
				State = state;
				Items = state.Clients.Select(x => new Item(x)).ToList();
			}

			public ServerStatus State { get; }
			
			public IReadOnlyCollection<Item> Items { get; }
		}

		public class Item
		{
			public ClientInformation Client { get; }

			public Item(ClientInformation client)
			{
				RequiresNotNull(client);
				this.Client = client;
				var r = client.TestResults.Select(x => x.Name + ": " + x.Status).Take(10);
				var tip = string.Join(" | ", r);
				this.Tip = tip + (client.TestResults.Count > 10 ? " | ..." : string.Empty);
				Passed = client.TestResults.Count(x => x.Status == TestStatus.Passed);
			}

			public string Tip { get; }

			public int Passed { get; }

			public int Total => Client.TestResults.Count;
		}

	}
}