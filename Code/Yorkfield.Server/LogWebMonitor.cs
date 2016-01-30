using System;
using System.Collections.Generic;
using System.Globalization;
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
				Items = items;
			}

			public IReadOnlyCollection<LogItem> Items { get; }
		}
	}
}