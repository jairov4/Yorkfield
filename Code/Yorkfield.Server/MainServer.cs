using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	/// <summary>
	/// The main server implementation
	/// </summary>
	public class MainServer : IServer
	{
		private const string buildCommand = "build.cmd";
		private const string targetUnitTestingAssembly = "Yorkfield.Test\\bin\\Debug\\Yorkfield.Test.dll";
		private readonly ConcurrentDictionary<Guid, ClientInformation> statusBySession;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainServer"/> class.
		/// </summary>
		public MainServer()
		{
			statusBySession = new ConcurrentDictionary<Guid, ClientInformation>();
		}

		/// <summary>
		/// Polls the current status of this server.
		/// </summary>
		/// <returns>The current server state</returns>
		public ServerStatus PollCurrentStatus()
		{
			return new ServerStatus(DateTimeOffset.Now, statusBySession.Values.ToArray());
		}

		/// <summary>
		/// Gets the build instructions.
		/// </summary>
		/// <param name="clientName">Name of the client.</param>
		/// <returns>The build instructions for this session</returns>
		public BuildInstructions GetBuildInstructions(string clientName)
		{
			RequiresNotNull(clientName);
			var session = new BuildInstructions(Guid.NewGuid(), buildCommand, targetUnitTestingAssembly);
			return session;
		}

		/// <summary>
		/// Updates the client status.
		/// </summary>
		/// <param name="info">The information.</param>
		public void UpdateClientStatus(ClientInformation info)
		{
			RequiresNotNull(info);
			statusBySession[info.Session] = info;
		}

		/// <summary>
		/// Clears the finished sessions.
		/// </summary>
		public void ClearFinishedSessions()
		{
			var sessionsToRemove =
				statusBySession.Values.Where(x => x.Status == BuildStatus.Successful || x.Status == BuildStatus.Failed);
			foreach (var clientInformation in sessionsToRemove)
			{
				ClientInformation rem;
				statusBySession.TryRemove(clientInformation.Session, out rem);
			}
		}
	}
}