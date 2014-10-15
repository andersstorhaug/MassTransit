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
namespace MassTransit.Tests.Distributor
{
	using System.Diagnostics;
	using Load;
	using Load.Messages;
	using Load.Sagas;
	using MassTransit.Distributor.Messages;
	using MassTransit.Pipeline.Inspectors;
	using NUnit.Framework;
	using TestFramework;
	using MassTransit.Distributor;

	[TestFixture]
	public class Using_the_distributor_for_a_saga :
		LoopbackDistributorSagaTestFixture
	{
		[Test]
		public void Should_have_a_subscription_for_the_first_command()
		{
			LocalBus.ShouldHaveSubscriptionFor<FirstCommand>();
		}

		[Test]
		public void Should_have_a_subscription_for_the_pending_command()
		{
			LocalBus.ShouldHaveSubscriptionFor<FirstPending>();
		}
	}

	[TestFixture]
	public class Using_the_distributor_saga_worker_for_a_saga :
		LoopbackDistributorSagaTestFixture
	{
		protected override void EstablishContext()
		{
			base.EstablishContext();

			AddInstance("A", "loopback://localhost/a", 
				builder => builder.Add(FirstSagaRepository),
				bus => bus.ImplementSagaDistributorWorker(FirstSagaRepository));
			
			AddInstance("B", "loopback://localhost/b", 
				builder => builder.Add(FirstSagaRepository),
				bus => bus.ImplementSagaDistributorWorker(FirstSagaRepository));
			
			AddInstance("C", "loopback://localhost/c", 
				builder => builder.Add(FirstSagaRepository),
				bus => bus.ImplementSagaDistributorWorker(FirstSagaRepository));
		}

		[Test]
		public void Should_register_the_message_consumers()
		{
			Instances["A"].DataBus.ShouldHaveSubscriptionFor<Distributed<FirstCommand>>();
			Instances["B"].DataBus.ShouldHaveSubscriptionFor<Distributed<FirstCommand>>();
			Instances["C"].DataBus.ShouldHaveSubscriptionFor<Distributed<FirstCommand>>();
		}

		[Test, Explicit]
		public void Using_the_load_generator_should_share_the_load()
		{
			var generator = new LoadGenerator<FirstCommand, FirstResponse>();

			generator.Run(RemoteBus, 100, x => new FirstCommand(x));
		}

		[Test, Explicit]
		public void The_pipeline_viewer_should_show_the_distributor()
		{
			PipelineViewer.Trace(LocalBus.InboundPipeline);
			PipelineViewer.Trace(LocalBus.ControlBus.InboundPipeline);
			PipelineViewer.Trace(RemoteBus.InboundPipeline);
			PipelineViewer.Trace(Instances["A"].DataBus.InboundPipeline);
		}
	}
}