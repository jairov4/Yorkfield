using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Core
{
	/// <summary>
	/// Represent the server state
	/// </summary>
	[DataContract]
	public class ServerStatus
	{
		[DataMember] private IReadOnlyCollection<ClientInformation> clients;

		[DataMember] private DateTimeOffset timeStamp;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerStatus"/> class.
		/// </summary>
		/// <param name="timeStamp">The time stamp.</param>
		/// <param name="clients">The clients.</param>
		public ServerStatus(DateTimeOffset timeStamp, IReadOnlyCollection<ClientInformation> clients)
		{
			RequiresNotNull(clients);
			this.timeStamp = timeStamp;
			this.clients = clients;
		}

		/// <summary>
		/// Gets the timestamp
		/// </summary>
		public DateTimeOffset TimeStamp => timeStamp;

		/// <summary>
		/// Gets the clients collection.
		/// </summary>
		/// <value>
		/// The clients.
		/// </value>
		public IReadOnlyCollection<ClientInformation> Clients => clients;
	}
}