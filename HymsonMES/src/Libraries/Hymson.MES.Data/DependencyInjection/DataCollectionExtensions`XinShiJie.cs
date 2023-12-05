using Hymson.MES.Data.Repositories.Process;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入服务类（欣世界）
    /// </summary>
    public static partial class DataCollectionExtensions
    {
        /// <summary>
        /// 添加仓储依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddRepositoryForXinShiJie(this IServiceCollection services)
        {
            #region Process
            services.AddSingleton<IProcProductTimecontrolRepository, ProcProductTimecontrolRepository>();

            #endregion

            return services;
        }

    }
}
