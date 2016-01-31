using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	public class LogWebMonitor : NancyModule
	{
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

		public class Model
		{
			public Model(IReadOnlyCollection<LogItem> items)
			{
				RequiresNotNull(items);
				Items = items.Select(x => new LogItemModel(x));
			}

			public IEnumerable<LogItemModel> Items { get; }
		}

		public class LogItemModel
		{
			public LogItemModel(LogItem logItem)
			{
				RequiresNotNull(logItem);
				LogItem = logItem;
			}

			public LogItem LogItem { get; }

			public bool IsError => LogItem.Severity == LogSeverity.Error;
			public bool IsInformation => LogItem.Severity == LogSeverity.Information;
			public bool IsWarning => LogItem.Severity == LogSeverity.Warning;
		}
	}
}