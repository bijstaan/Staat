using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.Models;
using Z.EntityFramework.Plus;

namespace Staat.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class MonitorQuery
    {
        [UseApplicationContext]
        [UsePaging(MaxPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Monitor> GetMonitors([ScopedService] ApplicationDbContext context) => context.Monitor.FromCache().AsQueryable();
    }
}