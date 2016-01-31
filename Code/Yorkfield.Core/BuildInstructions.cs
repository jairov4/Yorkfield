using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	[DataContract]
	[KnownType(typeof (string[]))]
	public class BuildInstructions
	{
		[DataMember] private string buildCommand;

		[DataMember] private Guid session;
		
		[DataMember] private string targetUnitTestingAssembly;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildInstructions"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="buildCommand">The build command.</param>
		/// <param name="targetUnitTestingAssembly">The target unit testing assembly.</param>
		public BuildInstructions(Guid session, string buildCommand, string targetUnitTestingAssembly)
		{
			RequiresNotNull(buildCommand);
			RequiresNotNull(targetUnitTestingAssembly);
			this.buildCommand = buildCommand;
			this.targetUnitTestingAssembly = targetUnitTestingAssembly;
			this.session = session;
		}

		/// <summary>
		/// Gets the session identifier
		/// </summary>
		public Guid Session => session;

		/// <summary>
		/// Gets the build command.
		/// </summary>
		/// <value>
		/// The build command.
		/// </value>
		public string BuildCommand => buildCommand;

		/// <summary>
		/// Gets the target unit testing assembly.
		/// </summary>
		/// <value>
		/// The target unit testing assembly.
		/// </value>
		public string TargetUnitTestingAssembly => targetUnitTestingAssembly;
	}
}