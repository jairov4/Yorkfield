using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	/// <summary>
	/// Represent the unit test result
	/// </summary>
	[DataContract]
	public class TestResult
	{
		[DataMember] private string name;

		[DataMember] private TestStatus status;

		/// <summary>
		/// Initializes a new instance of the <see cref="TestResult"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="status">The status.</param>
		public TestResult(string name, TestStatus status)
		{
			RequiresNotNull(status);
			this.name = name;
			this.status = status;
		}

		/// <summary>
		/// Gets the test case name
		/// </summary>
		public string Name => name;

		/// <summary>
		/// Gets the test case status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public TestStatus Status => status;
	}
}