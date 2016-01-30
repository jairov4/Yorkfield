using Yorkfield.Core;

namespace Yorkfield.Client
{
	public interface IClient
	{
		int TimeOutMilliseconds { get; set; }

		void Start(IServer server);
	}
}