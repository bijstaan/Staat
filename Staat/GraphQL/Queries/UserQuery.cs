using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Staat.Data;
using Staat.Extensions;
using Staat.Models.Users;

namespace Staat.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class UserQuery
    {
        [UseApplicationContext]
        [UsePaging(MaxPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers([ScopedService] ApplicationDbContext context) => context.User;
    }
}