// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Tests.Examples
{
	using TestFramework;
	using TestFramework.Examples.Messages;

	[Scenario]
	public class Given_a_pong_service :
		Given_two_service_buses_with_shared_subscriptions
	{
		[Given]
		public void A_pong_service()
		{
			PingService = new ConsumerOf<Ping>(ping =>
				{
					// Response with a pong to the ping
					CurrentMessage.Respond(new Pong(ping.CorrelationId));
				});
			RemoteBus.Subscribe(PingService);
		}

		protected ConsumerOf<Ping> PingService { get; private set; }
	}
}