using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public enum LogSeverity
	{
		[EnumMember] Error,
		[EnumMember] Warning,
		[EnumMember] Information
	}
}