using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	[KnownType(typeof (TestResult[]))]
	public class ClientInformation
	{
		[DataMember] private string name;

		[DataMember] private Guid session;

		[DataMember] private BuildStatus status;

		[DataMember] private IReadOnlyCollection<TestResult> testResults;

		public ClientInformation(string name, Guid session, BuildStatus status, IReadOnlyCollection<TestResult> testResults)
		{
			this.name = name;
			this.status = status;
			this.testResults = testResults;
			this.session = session;
		}

		public string Name => name;

		public Guid Session => session;

		public BuildStatus Status => status;

		public IReadOnlyCollection<TestResult> TestResults => testResults;
	}
}