using System.Diagnostics;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	/// <summary>
	/// Adapter that redirects tracing information to log server
	/// </summary>
	public class LogTraceListener : TraceListener
	{
		private readonly ILog log;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogTraceListener"/> class.
		/// </summary>
		/// <param name="log">The log.</param>
		public LogTraceListener(ILog log)
		{
			RequiresNotNull(log);
			this.log = log;
		}

		/// <summary>
		/// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void Write(string message)
		{
			log.Log(LogSeverity.Information, message);
		}

		/// <summary>
		/// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void WriteLine(string message)
		{
			log.Log(LogSeverity.Information, message);
		}
	}
}