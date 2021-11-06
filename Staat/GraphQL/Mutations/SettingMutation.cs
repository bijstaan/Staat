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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Extensions;
using Staat.GraphQL.Mutations.Inputs.Setting;
using Staat.GraphQL.Mutations.Payloads.Setting;
using Staat.Helpers;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    [Authorize]
    public class SettingMutation
    {
        [UseApplicationContext]
        public async Task<SettingBasePayload> AddSetting(AddSettingInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var setting = new Models.Settings()
            {
                Key = input.Key,
                Value = input.Value
            };
            await context.Settings.AddAsync(setting, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Settings>();
            return new SettingBasePayload(setting);
        }

        [UseApplicationContext]
        public async Task<SettingBasePayload> UpdateSetting(AddSettingInput input,
            [ScopedService] ApplicationDbContext context, CancellationToken cancellationToken)
        {
            Settings? setting = await context.Settings.DeferredFirst(x => x.Key == input.Key).FromCacheAsync(cancellationToken);
            if (setting is null)
            {
                return new SettingBasePayload(
                    new UserError("Setting with key not found.", "SETTING_NOT_FOUND"));
            }
            setting.Value = input.Value;
            await context.SaveChangesAsync(cancellationToken);
            QueryCacheManager.ExpireType<Settings>();
            return new SettingBasePayload(setting);
        }
    }
}