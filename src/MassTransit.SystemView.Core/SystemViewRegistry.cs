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
namespace MassTransit.SystemView.Core
{
    using StructureMap;
    using StructureMapIntegration;

    public class SystemViewRegistry :
        MassTransitRegistryBase
    {
        public SystemViewRegistry(IConfiguration configuration, IContainer container)
        {
            RegisterControlBus(configuration.SystemViewControlUri, x => { });

            RegisterServiceBus(configuration.SystemViewDataUri, x =>
                {
                    x.SetConcurrentConsumerLimit(1);
                    x.UseControlBus(container.GetInstance<IControlBus>());

                    ConfigureSubscriptionClient(configuration.SubscriptionServiceUri, x);
                });
        }
    }
}