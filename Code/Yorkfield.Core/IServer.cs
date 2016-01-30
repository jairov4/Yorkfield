using System.ServiceModel;

namespace Yorkfield.Core
{
	[ServiceContract]
	public interface IServer
	{
		[OperationContract]
		ServerStatus PollCurrentStatus();

		[OperationContract]
		BuildInstructions GetBuildInstructions(string clientName);

		[OperationContract]
		void UpdateClientStatus(ClientInformation info);
	}
}