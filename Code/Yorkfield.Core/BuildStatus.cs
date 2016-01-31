using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	/// <summary>
	/// The build status
	/// </summary>
	[DataContract]
	public enum BuildStatus
	{
		[EnumMember] InProgress,
		[EnumMember] Successful,
		[EnumMember] Failed
	}
}