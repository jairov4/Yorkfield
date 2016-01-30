using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	[KnownType(typeof (string[]))]
	public class BuildInstructions
	{
		[DataMember] private string buildCommand;

		[DataMember] private Guid session;
		
		[DataMember] private string targetUnitTestingAssembly;

		public BuildInstructions(Guid session, string buildCommand, string targetUnitTestingAssembly)
		{
			this.buildCommand = buildCommand;
			this.targetUnitTestingAssembly = targetUnitTestingAssembly;
			this.session = session;
		}

		public Guid Session => session;

		public string BuildCommand => buildCommand;

		public string TargetUnitTestingAssembly => targetUnitTestingAssembly;
	}
}