using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

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
