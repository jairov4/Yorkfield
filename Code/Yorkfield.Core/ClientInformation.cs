using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	/// <summary>
	/// Represent the information of a client connected
	/// </summary>
	[DataContract]
	[KnownType(typeof (TestResult[]))]
	public class ClientInformation
	{
		[DataMember] private string name;

		[DataMember] private Guid session;

		[DataMember] private BuildStatus status;

		[DataMember] private IReadOnlyCollection<TestResult> testResults;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClientInformation"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="session">The session identifier.</param>
		/// <param name="status">The status of the build.</param>
		/// <param name="testResults">The test results collection.</param>
		public ClientInformation(string name, Guid session, BuildStatus status, IReadOnlyCollection<TestResult> testResults)
		{
			RequiresNotNull(name);
			RequiresNotNull(status);
			RequiresNotNull(testResults);
			this.name = name;
			this.status = status;
			this.testResults = testResults;
			this.session = session;
		}

		/// <summary>
		/// The name
		/// </summary>
		public string Name => name;

		/// <summary>
		/// Gets the session identifier.
		/// </summary>
		/// <value>
		/// The session.
		/// </value>
		public Guid Session => session;

		/// <summary>
		/// Gets the status of the build.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public BuildStatus Status => status;

		/// <summary>
		/// Gets the test results collection.
		/// </summary>
		/// <value>
		/// The test results.
		/// </value>
		public IReadOnlyCollection<TestResult> TestResults => testResults;
	}
}