using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Staat.Models;

namespace Staat.GraphQL.Subscriptions
{
    [ExtendObjectType(OperationTypeNames.Subscription)]
    public class ServiceSubscription
    {
        [Subscribe]
        public Service ServiceAdded([EventMessage] Service service) => service;

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<Service>> ServiceStatusChanged(int serviceId,
            [Service] ITopicEventReceiver receiver)
        {
            string topic = $"service_{serviceId.ToString()}_statusChanged";
            ValueTask<ISourceStream<Service>> stream = receiver.SubscribeAsync<string, Service>(topic);
            return stream;
        }
        
        [Subscribe]
        public Service ServiceRemoved([EventMessage] Service service) => service;
    }
}