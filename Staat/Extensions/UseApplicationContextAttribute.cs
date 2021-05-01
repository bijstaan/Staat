using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using Staat.Data;

namespace Staat.Extensions
{
    public class UseApplicationContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.UseDbContext<ApplicationDbContext>();
        }
    }
}