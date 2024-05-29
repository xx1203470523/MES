using Hymson.MES.CoreServices.Services.Quality;

namespace Hymson.MES.BackgroundServices.Tasks.Quality.EnvOrderCreate
{
    /// <summary>
    /// 
    /// </summary>
    public class EnvOrderAutoCreateService : IEnvOrderAutoCreateService
    {
        private readonly IEnvOrderCreateService _service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service"></param>
        public EnvOrderAutoCreateService(IEnvOrderCreateService service)
        {
            _service = service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await _service.AutoCreateAsync();
        }
    }
}
