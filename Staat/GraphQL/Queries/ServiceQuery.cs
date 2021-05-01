using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.Models;

namespace Staat.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class ServiceQuery
    {
        [UseApplicationContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Service> GetServices([ScopedService] ApplicationDbContext context) => context.Service;
    }
}