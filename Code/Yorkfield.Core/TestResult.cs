using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public class TestResult
	{
		[DataMember] private string name;

		[DataMember] private TestStatus status;

		public TestResult(string name, TestStatus status)
		{
			this.name = name;
			this.status = status;
		}

		public string Name => name;

		public TestStatus Status => status;
	}
}