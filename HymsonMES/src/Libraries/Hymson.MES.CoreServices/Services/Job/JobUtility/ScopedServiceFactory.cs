using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class ScopedServiceFactory : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IServiceScope _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public ScopedServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IJobContextProxy GetScopedService()
        {
            if (_scope == null)
            {
                // 创建新的服务作用域
                _scope = _serviceScopeFactory.CreateScope();
            }

            // 从作用域中获取 ScopedService 实例
            return _scope.ServiceProvider.GetRequiredService<IJobContextProxy>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // 释放服务作用域
            _scope?.Dispose();
            GC.SuppressFinalize(this);
            _scope = null;
        }
    }
}
