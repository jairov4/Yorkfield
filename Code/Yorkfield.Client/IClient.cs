using Yorkfield.Core;

namespace Yorkfield.Client
{
	/// <summary>
	/// Abstract view of the client features
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Gets or sets the time out milliseconds.
		/// </summary>
		/// <value>
		/// The time out milliseconds.
		/// </value>
		int TimeOutMilliseconds { get; set; }

		/// <summary>
		/// Starts the specified server.
		/// </summary>
		/// <param name="server">The server.</param>
		void Start(IServer server);
	}
}