using System.Data;
using System.Threading.Tasks;

namespace Yorkfield.Server
{
	/// <summary>
	/// Factory that allows to get connections to a data base
	/// </summary>
	public interface IDbConnectionFactory
	{
		/// <summary>
		/// Opens the connection to database.
		/// </summary>
		/// <returns></returns>
		Task<IDbConnection> OpenConnection();
	}
}