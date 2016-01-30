using System.Data;
using System.Threading.Tasks;

namespace Yorkfield.Server
{
	public interface IDbConnectionFactory
	{
		Task<IDbConnection> OpenConnection();
	}
}