using System;
using System.Collections.Generic;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	/// <summary>
	/// The Log server
	/// </summary>
	public class LogServer : ILog
	{
		private readonly IDbConnectionFactory factory;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogServer"/> class.
		/// </summary>
		/// <param name="factory">The database connection factory.</param>
		public LogServer(IDbConnectionFactory factory)
		{
			RequiresNotNull(factory);
			this.factory = factory;
		}

		/// <summary>
		/// Logs the specified severity.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <param name="message">The message.</param>
		public void Log(LogSeverity severity, string message)
		{
			RequiresNotNull(message);
			var con = factory.OpenConnection();
			con.ContinueWith(pt =>
			{
				var connection = pt.Result;
				try
				{
					var cmd = connection.CreateCommand();
					cmd.CommandText = "INSERT INTO Log (Id, Timestamp, Severity, Message) VALUES (NEWID(), @time, @sev, @msg)";

					var timestampParameter = cmd.CreateParameter();
					timestampParameter.ParameterName = "@time";
					timestampParameter.Value = DateTime.Now;
					cmd.Parameters.Add(timestampParameter);

					var severityParameter = cmd.CreateParameter();
					severityParameter.ParameterName = "@sev";
					severityParameter.Value = severity.ToString();
					cmd.Parameters.Add(severityParameter);

					var messageParameter = cmd.CreateParameter();
					messageParameter.ParameterName = "@msg";
					messageParameter.Value = message;
					cmd.Parameters.Add(messageParameter);

					cmd.ExecuteNonQuery();
				}
				finally
				{
					connection.Dispose();
				}
			});
		}

		/// <summary>
		/// Reads the log data.
		/// </summary>
		/// <param name="from">Initial timestamp to search from.</param>
		/// <param name="to">Final timestamp up search to.</param>
		/// <returns>Collection of log items found</returns>
		public IReadOnlyCollection<LogItem> ReadLogData(DateTimeOffset @from, DateTimeOffset to)
		{
			var list = new List<LogItem>();
			var task = factory.OpenConnection();
			task.Wait();
			using (var connection = task.Result)
			{
				var cmd = connection.CreateCommand();
				cmd.CommandText = "SELECT cast(Timestamp as datetime), Severity, Message FROM Log WHERE Timestamp >= @from AND Timestamp <= @to ORDER BY Timestamp DESC";

				var paramFrom = cmd.CreateParameter();
				paramFrom.ParameterName = "@from";
				paramFrom.Value = @from;
				cmd.Parameters.Add(paramFrom);

				var paramTo = cmd.CreateParameter();
				paramTo.ParameterName = "@to";
				paramTo.Value = to;
				cmd.Parameters.Add(paramTo);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var timestamp = reader.GetDateTime(0);
						LogSeverity severity;
						Enum.TryParse(reader.GetString(1), out severity);
						var message = reader.GetString(2);
						var item = new LogItem(timestamp, severity, message);
						list.Add(item);
					}
				}
				return list;
			}
		}
	}
}