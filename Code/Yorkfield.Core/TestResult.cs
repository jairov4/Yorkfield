using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public class TestResult
	{
		[DataMember] private string name;

		[DataMember] private BuildStatus status;

		public TestResult(string name, BuildStatus status)
		{
			this.name = name;
			this.status = status;
		}

		public string Name => name;

		public BuildStatus Status => status;
	}
}