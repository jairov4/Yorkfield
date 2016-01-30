using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Yorkfield.Core
{
	[DataContract]
	public class ServerStatus
	{
		[DataMember] private IReadOnlyCollection<ClientInformation> clients;

		[DataMember] private DateTimeOffset timeStamp;

		public ServerStatus(DateTimeOffset timeStamp, IReadOnlyCollection<ClientInformation> clients)
		{
			this.timeStamp = timeStamp;
			this.clients = clients;
		}

		public DateTimeOffset TimeStamp => timeStamp;

		public IReadOnlyCollection<ClientInformation> Clients => clients;
	}
}