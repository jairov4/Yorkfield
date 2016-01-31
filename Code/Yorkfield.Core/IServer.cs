using System.ServiceModel;

namespace Yorkfield.Core
{
	/// <summary>
	/// Represent the server and its functionaly exposed
	/// </summary>
	[ServiceContract]
	public interface IServer
	{
		/// <summary>
		/// Polls the current status of this server.
		/// </summary>
		/// <returns>The current server state</returns>
		[OperationContract]
		ServerStatus PollCurrentStatus();

		/// <summary>
		/// Gets the build instructions.
		/// </summary>
		/// <param name="clientName">Name of the client.</param>
		/// <returns>The current build instructions</returns>
		[OperationContract]
		BuildInstructions GetBuildInstructions(string clientName);

		/// <summary>
		/// Updates the client status.
		/// </summary>
		/// <param name="info">The information.</param>
		[OperationContract]
		void UpdateClientStatus(ClientInformation info);
	}
}