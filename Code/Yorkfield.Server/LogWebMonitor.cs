using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	/// <summary>
	/// Log web monitor
	/// </summary>
	public class LogWebMonitor : NancyModule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogWebMonitor"/> class.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="baseUri">The base URI.</param>
		public LogWebMonitor(ILog log, string baseUri) : base(baseUri)
		{
			RequiresNotNull(log);
			Get["/"] = _ => View["LogWebMonitor", GetModel(log)];
		}

		private Model GetModel(ILog log)
		{
			var data = log.ReadLogData(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
			return new Model(data);
		}

		/// <summary>
		/// Represent the view model of the monitor
		/// </summary>
		public class Model
		{
			public Model(IReadOnlyCollection<LogItem> items)
			{
				RequiresNotNull(items);
				Items = items.Select(x => new LogItemModel(x));
			}

			/// <summary>
			/// Gets the log entries.
			/// </summary>
			/// <value>
			/// The items.
			/// </value>
			public IEnumerable<LogItemModel> Items { get; }
		}

		/// <summary>
		/// Log entry view model
		/// </summary>
		public class LogItemModel
		{
			public LogItemModel(LogItem logItem)
			{
				RequiresNotNull(logItem);
				LogItem = logItem;
			}

			/// <summary>
			/// Gets the log item.
			/// </summary>
			/// <value>
			/// The log item.
			/// </value>
			public LogItem LogItem { get; }

			/// <summary>
			/// Gets a value indicating whether this instance is an error entry.
			/// </summary>
			public bool IsError => LogItem.Severity == LogSeverity.Error;

			/// <summary>
			/// Gets a value indicating whether this instance is an information entry.
			/// </summary>
			/// <value>
			///   <c>true</c> if this instance is information; otherwise, <c>false</c>.
			/// </value>
			public bool IsInformation => LogItem.Severity == LogSeverity.Information;

			/// <summary>
			/// Gets a value indicating whether this instance is a warning entry.
			/// </summary>
			/// <value>
			///   <c>true</c> if this instance is warning; otherwise, <c>false</c>.
			/// </value>
			public bool IsWarning => LogItem.Severity == LogSeverity.Warning;
		}
	}
}