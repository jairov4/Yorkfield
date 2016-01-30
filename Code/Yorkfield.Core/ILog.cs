using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Yorkfield.Core
{
	[ServiceContract]
	public interface ILog
	{
		[OperationContract]
		void Log(LogSeverity severity, string message);

		[OperationContract]
		IReadOnlyCollection<LogItem> ReadLogData(DateTimeOffset from, DateTimeOffset to);
	}
}