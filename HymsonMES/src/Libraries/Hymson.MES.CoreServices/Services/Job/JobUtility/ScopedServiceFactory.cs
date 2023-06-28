using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    public class ScopedServiceFactory : IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IServiceScope _scope;

        public ScopedServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

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


        public void Dispose()
        {
            // 释放服务作用域
            _scope?.Dispose();
            _scope = null;
        }
    }

}
