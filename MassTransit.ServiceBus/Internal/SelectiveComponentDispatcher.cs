/// Copyright 2007-2008 The Apache Software Foundation.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
/// this file except in compliance with the License. You may obtain a copy of the 
/// License at 
/// 
///   http://www.apache.org/licenses/LICENSE-2.0 
/// 
/// Unless required by applicable law or agreed to in writing, software distributed 
/// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
/// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
/// specific language governing permissions and limitations under the License.
namespace MassTransit.ServiceBus.Internal
{
	public class SelectiveComponentDispatcher<TComponent, TMessage> :
		Consumes<TMessage>.Selected
		where TMessage : class
		where TComponent : class
	{
		private readonly IObjectBuilder _builder;

		public SelectiveComponentDispatcher(IObjectBuilder builder)
		{
			_builder = builder;
		}

		public bool Accept(TMessage message)
		{
			Consumes<TMessage>.Selected consumer = _builder.Build<Consumes<TMessage>.Selected>(typeof (TComponent));

			bool result = consumer.Accept(message);

			_builder.Release(consumer);

			return result;
		}

		public void Consume(TMessage message)
		{
			Consumes<TMessage>.Selected consumer = _builder.Build<Consumes<TMessage>.Selected>(typeof (TComponent));

			consumer.Consume(message);

			_builder.Release(consumer);
		}
	}
}