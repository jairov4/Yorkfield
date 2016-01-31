using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	/// <summary>
	/// Indicates the log entry severity
	/// </summary>
	[DataContract]
	public enum LogSeverity
	{
		[EnumMember] Error,
		[EnumMember] Warning,
		[EnumMember] Information
	}
}