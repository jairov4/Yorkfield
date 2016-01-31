using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using NUnit.Engine;
using Yorkfield.Core;

namespace Yorkfield.Client
{
	public class MainClient : IClient
	{
		public int TimeOutMilliseconds { get; set; }

		public ILog log;

		public MainClient(ILog log)
		{
			TimeOutMilliseconds = 250000;
			this.log = log;
		}

		public void Start(IServer server)
		{
			BuildInstructions instructions = new BuildInstructions(Guid.Empty, string.Empty, string.Empty);
			try
			{
				instructions = GetBuildInstructions(server);
				BuildProject(server, instructions);
				ExecuteNUnitTestRunner(server, instructions);
			}
			catch (FileNotFoundException e)
			{
				log.Log(LogSeverity.Error,  $"The file was not found: {e.FileName}");
				var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Failed,
					new TestResult[0]);
				server.UpdateClientStatus(status);
			}
			catch (Win32Exception e)
			{
				log.Log(LogSeverity.Error, $"Error launching the build process: {e.Message}");
				var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Failed,
					new TestResult[0]);
				server.UpdateClientStatus(status);
			}
			catch (ApplicationException e)
			{
				log.Log(LogSeverity.Error, $"Error building the project: {e.Message}");
				var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Failed,
					new TestResult[0]);
				server.UpdateClientStatus(status);
			}
		}

		private BuildInstructions GetBuildInstructions(IServer server)
		{
			log.Log(LogSeverity.Information, "Getting build instructions");
			var instructions = server.GetBuildInstructions(Environment.MachineName);
			log.Log(LogSeverity.Information, $"Building session {instructions.Session}");
			var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.InProgress,
				new TestResult[0]);
			server.UpdateClientStatus(status);
			return instructions;
		}

		private void ExecuteNUnitTestRunner(IServer server, BuildInstructions instructions)
		{
			log.Log(LogSeverity.Information, "Executing NUnit Test Runner");
			XmlNode result2;
			using (var r = new TestEngine())
			{
				r.Initialize();
				var pkg = new TestPackage(instructions.TargetUnitTestingAssembly);
				using (var runner = r.GetRunner(pkg))
				{
					var filter = TestFilter.Empty;
					var tests = runner.CountTestCases(filter);
					log.Log(LogSeverity.Information, $"Found {tests} tests");
					result2 = runner.Run(null, filter);
				}
			}

			var status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Successful,
				new TestResult[0]);
			server.UpdateClientStatus(status);
			log.Log(LogSeverity.Information, "Test fixtures execution complete");
		}

		private void BuildProject(IServer server, BuildInstructions instructions)
		{
			log.Log(LogSeverity.Information, "Building project");
			var startOptions = new ProcessStartInfo(instructions.BuildCommand);
			startOptions.UseShellExecute = false;
			var process = Process.Start(startOptions);
			ClientInformation status;
			if (!process.WaitForExit(TimeOutMilliseconds))
			{
				status = new ClientInformation(Environment.MachineName, instructions.Session, BuildStatus.Failed, new TestResult[0]);
				server.UpdateClientStatus(status);
				log.Log(LogSeverity.Warning, $"Build project is timed out, {process.ExitTime}");
				throw new TimeoutException();
			}

			var result = process.ExitCode == 0 ? BuildStatus.InProgress : BuildStatus.Failed;
			status = new ClientInformation(Environment.MachineName, instructions.Session, result, new TestResult[0]);
			server.UpdateClientStatus(status);
			if (result == BuildStatus.Failed)
			{
				log.Log(LogSeverity.Error, $"Build project failed: executing {instructions.BuildCommand} - Exit code {process.ExitCode}");
				throw new ApplicationException();
			}
		}
	}
}