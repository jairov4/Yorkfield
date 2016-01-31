using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.Responses;
using Yorkfield.Core;

namespace Yorkfield.Server
{
	public class LogWebMonitor : NancyModule
	{
		public LogWebMonitor(ILog log, string baseUri) : base(baseUri)
		{
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
				Items = items.Select(x => new LogItemModel(x));
			}

			public IEnumerable<LogItemModel> Items { get; }
		}

		public class LogItemModel
		{
			public LogItemModel(LogItem logItem)
			{
				LogItem = logItem;
			}

			public LogItem LogItem { get; }

			public bool IsError => LogItem.Severity == LogSeverity.Error;
			public bool IsInformation => LogItem.Severity == LogSeverity.Information;
			public bool IsWarning => LogItem.Severity == LogSeverity.Warning;
		}
	}
}