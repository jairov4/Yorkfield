using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Yorkfield.Server
{
	/// <summary>
	/// Microsoft SQL Server connection factory implementation
	/// </summary>
	public class SqlConnectionFactory : IDbConnectionFactory
	{
		/// <summary>
		/// Opens the connection to database.
		/// </summary>
		/// <returns></returns>
		public async Task<IDbConnection> OpenConnection()
		{
			var connectionString = ConfigurationManager.ConnectionStrings[0];
			var providerName = connectionString.ProviderName;
			var provider = DbProviderFactories.GetFactory(providerName);
			var connection = provider.CreateConnection();
			connection.ConnectionString = connectionString.ConnectionString;
			await connection.OpenAsync();
			return connection;
		}
	}
}