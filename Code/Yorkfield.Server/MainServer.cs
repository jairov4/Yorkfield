using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	public class MainServer : IServer
	{
		private const string buildCommand = "build.cmd";
		private const string targetUnitTestingAssembly = "Yorkfield.Test\\bin\\Debug\\Yorkfield.Test.dll";
		private readonly ConcurrentDictionary<Guid, ClientInformation> statusBySession;

		public MainServer()
		{
			statusBySession = new ConcurrentDictionary<Guid, ClientInformation>();
		}

		public ServerStatus PollCurrentStatus()
		{
			return new ServerStatus(DateTimeOffset.Now, statusBySession.Values.ToArray());
		}

		public BuildInstructions GetBuildInstructions(string clientName)
		{
			RequiresNotNull(clientName);
			var session = new BuildInstructions(Guid.NewGuid(), buildCommand, targetUnitTestingAssembly);
			return session;
		}

		public void UpdateClientStatus(ClientInformation info)
		{
			RequiresNotNull(info);
			statusBySession[info.Session] = info;
		}

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