using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public enum TestStatus
	{
		[DataMember] InProgress,
		[DataMember] Passed,
		[DataMember] Failed,
		[DataMember] Inconclusive,
		[DataMember] Waiting
	}
}