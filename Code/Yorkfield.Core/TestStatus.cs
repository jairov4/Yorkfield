using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	/// <summary>
	/// Indicates the test case status
	/// </summary>
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