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

#nullable enable
using System.Collections.Generic;
using Staat.Helpers;

namespace Staat.GraphQL.Mutations.Payloads.Service
{
    public class ServiceBasePayload : Payload
    {
        public Data.Models.Service? Service { get;}
        public ServiceBasePayload(Data.Models.Service service)
        {
            Service = service;
        }
        
        public ServiceBasePayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public ServiceBasePayload(UserError error) : base(new [] { error })
        {
        }
    }
}