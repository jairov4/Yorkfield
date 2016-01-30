using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public enum BuildStatus
	{
		[EnumMember] InProgress,
		[EnumMember] Successful,
		[EnumMember] Failed
	}
}