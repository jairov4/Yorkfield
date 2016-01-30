using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Yorkfield.Core
{
	public class MyDataContractResolver : DataContractResolver
	{
		public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
		{
			//if (dataContractType == typeof(Customer))
			//{
			//	XmlDictionary dictionary = new XmlDictionary();
			//	typeName = dictionary.Add("SomeCustomer");
			//	typeNamespace = dictionary.Add("http://tempuri.com");
			//	return true;
			//}
			
			var resolved = knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);
			return resolved;
		}

		public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
		{
			var resolved = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
			return resolved;
		}
	}

	public static class ServiceExtensions
	{
		public static void AddDataContractResolver(this ServiceHost host, DataContractResolver dcr)
		{
			foreach (var endpoint in host.Description.Endpoints)
			{
				AddDataContractResolver(endpoint, dcr);
			}
		}

		public static void AddDataContractResolver<T>(this ChannelFactory<T> host, DataContractResolver dcr)
		{
			AddDataContractResolver(host.Endpoint, dcr);
		}

		public static void AddDataContractResolver(this ServiceEndpoint endpoint, DataContractResolver dcr)
		{
			foreach (var myOperationDescription in endpoint.Contract.Operations)
			{
				var serializerBehavior = myOperationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (serializerBehavior == null)
				{
					serializerBehavior = new DataContractSerializerOperationBehavior(myOperationDescription);
					myOperationDescription.Behaviors.Add(serializerBehavior);
				}

				serializerBehavior.DataContractResolver = dcr;
			}
		}
	}
}
