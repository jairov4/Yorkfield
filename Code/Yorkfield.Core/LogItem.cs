using System;
using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	/// <summary>
	/// Represent a log entry
	/// </summary>
	[DataContract]
	public struct LogItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogItem"/> struct.
		/// </summary>
		/// <param name="timestamp">The timestamp.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="message">The message.</param>
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

		/// <summary>
		/// Gets the timestamp
		/// </summary>
		public DateTimeOffset? Timestamp => timestamp;

		/// <summary>
		/// Gets the severity of this log entry.
		/// </summary>
		/// <value>
		/// The severity.
		/// </value>
		public LogSeverity Severity => severity;

		/// <summary>
		/// Gets the log message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message => message;
	}
}