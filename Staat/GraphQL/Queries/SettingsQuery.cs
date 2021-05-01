using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.Models;

namespace Staat.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class SettingsQuery
    {
        [UseApplicationContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Settings> GetSettings([ScopedService] ApplicationDbContext context) => context.Settings;
    }
}