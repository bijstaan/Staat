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
    public class ServiceGroupQuery
    {
        [UseApplicationContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ServiceGroup> GetServiceGroups([ScopedService] ApplicationDbContext context) => context.ServiceGroup;
    }
    
}