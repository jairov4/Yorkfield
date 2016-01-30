using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Yorkfield.Server
{
	public class SqlConnectionFactory : IDbConnectionFactory
	{
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