/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Staat.Data.Models;

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