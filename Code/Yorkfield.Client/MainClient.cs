using System;
using System.Diagnostics;
using System.Xml;
using NUnit.Engine;
using NUnit.Engine.Services;
using Yorkfield.Core;

namespace Yorkfield.Client
{
	public class MainClient : IClient
	{
		public int TimeOutMilliseconds { get; set; }

		public MainClient()
		{
			TimeOutMilliseconds = 250000;
		}

		public void Start(IServer server)
		{
			var instructions = server.GetBuildInstructions(Environment.MachineName);
			BuildProject(server, instructions);
			ExecuteNUnitTestRunner(server, instructions);
		}

		private static void ExecuteNUnitTestRunner(IServer server, BuildInstructions instructions)
		{
			Trace.WriteLine("Executing NUnit Test Runner");
			XmlNode result2;
			using (var r = new TestEngine())
			{
				r.Initialize();
				var pkg = new TestPackage(instructions.TargetUnitTestingAssembly);
				using (var runner = r.GetRunner(pkg))
				{
					var filter = TestFilter.Empty;
					var tests = runner.CountTestCases(filter);
					Trace.WriteLine($"Found {tests} tests");
					result2 = runner.Run(null, filter);
				}
			}

			var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Successful,
				new TestResult[0]);
			server.UpdateClientStatus(status);
			Trace.WriteLine("Test fixtures execution complete");
		}

		private void BuildProject(IServer server, BuildInstructions instructions)
		{
			Trace.WriteLine("Building project");
			var startOptions = new ProcessStartInfo(instructions.BuildCommand);
			startOptions.UseShellExecute = false;
			var process = Process.Start(startOptions);
			var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.InProgress,
				new TestResult[0]);
			server.UpdateClientStatus(status);
			if (!process.WaitForExit(TimeOutMilliseconds))
			{
				status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Failed, new TestResult[0]);
				server.UpdateClientStatus(status);
				Trace.WriteLine("Build project is timed out");
				throw new TimeoutException();
			}

			var result = process.ExitCode == 0 ? BuildStatus.InProgress : BuildStatus.Failed;
			status = new ClientInformation(Environment.MachineName, instructions.Session, result, new TestResult[0]);
			server.UpdateClientStatus(status);
			if (result == BuildStatus.Failed)
			{
				Trace.WriteLine("Build project failed");
				throw new ApplicationException();
			}
		}
	}
}