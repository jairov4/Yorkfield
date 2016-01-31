using System.Collections.Generic;
using System.Linq;
using Nancy;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	/// <summary>
	/// The main server web monitor
	/// </summary>
	public class MainServerWebMonitor : NancyModule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MainServerWebMonitor"/> class.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <param name="path">The path.</param>
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

		/// <summary>
		/// Represents the monitor view model
		/// </summary>
		public class Model
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Model"/> class.
			/// </summary>
			/// <param name="state">The state.</param>
			public Model(ServerStatus state)
			{
				RequiresNotNull(state);
				State = state;
				Items = state.Clients.Select(x => new Item(x)).ToList();
			}

			/// <summary>
			/// Gets the state.
			/// </summary>
			/// <value>
			/// The state.
			/// </value>
			public ServerStatus State { get; }

			/// <summary>
			/// Gets the session entries collection.
			/// </summary>
			/// <value>
			/// The items.
			/// </value>
			public IReadOnlyCollection<Item> Items { get; }
		}

		/// <summary>
		/// Session entry information
		/// </summary>
		public class Item
		{
			/// <summary>
			/// Gets the associated client.
			/// </summary>
			/// <value>
			/// The client.
			/// </value>
			public ClientInformation Client { get; }

			/// <summary>
			/// Initializes a new instance of the <see cref="Item"/> class.
			/// </summary>
			/// <param name="client">The client.</param>
			public Item(ClientInformation client)
			{
				RequiresNotNull(client);
				this.Client = client;
				var r = client.TestResults.Select(x => x.Name + ": " + x.Status).Take(10);
				var tip = string.Join(" | ", r);
				this.Tip = tip + (client.TestResults.Count > 10 ? " | ..." : string.Empty);
				Passed = client.TestResults.Count(x => x.Status == TestStatus.Passed);
			}

			/// <summary>
			/// Gets the tooltip.
			/// </summary>
			/// <value>
			/// The tip.
			/// </value>
			public string Tip { get; }

			/// <summary>
			/// Gets the number of tests that have Passed status.
			/// </summary>
			/// <value>
			/// The passed.
			/// </value>
			public int Passed { get; }

			/// <summary>
			/// The total test results gathered
			/// </summary>
			public int Total => Client.TestResults.Count;
		}

	}
}