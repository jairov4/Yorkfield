using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public enum TestStatus
	{
		[EnumMember] InProgress,
		[EnumMember] Passed,
		[EnumMember] Failed,
		[EnumMember] Inconclusive,
		[EnumMember] Waiting
	}
}