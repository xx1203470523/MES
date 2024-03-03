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
