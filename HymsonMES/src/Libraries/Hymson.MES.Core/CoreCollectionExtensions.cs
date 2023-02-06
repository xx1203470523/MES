using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddInfrastructure();
            return services;
        }
    }
}
