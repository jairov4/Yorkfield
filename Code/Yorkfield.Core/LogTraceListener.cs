using System.Diagnostics;

namespace Yorkfield.Core
{
	public class LogTraceListener : TraceListener
	{
		private readonly ILog log;

		public LogTraceListener(ILog log)
		{
			this.log = log;
		}

		public override void Write(string message)
		{
			log.Log(LogSeverity.Information, message);
		}

		public override void WriteLine(string message)
		{
			log.Log(LogSeverity.Information, message);
		}
	}
}