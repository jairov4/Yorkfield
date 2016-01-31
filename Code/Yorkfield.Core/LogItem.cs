using System;
using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	[DataContract]
	public struct LogItem
	{
		public LogItem(DateTimeOffset? timestamp, LogSeverity severity, string message)
		{
			RequiresNotNull(message);
			this.timestamp = timestamp;
			this.severity = severity;
			this.message = message;
		}

		[DataMember] private DateTimeOffset? timestamp;

		[DataMember] private LogSeverity severity;

		[DataMember] private string message;

		public DateTimeOffset? Timestamp => timestamp;

		public LogSeverity Severity => severity;

		public string Message => message;
	}
}