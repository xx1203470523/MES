using Hymson.MES.CoreServices.Bos.Quality;

namespace Hymson.MES.CoreServices.Services.Quality.QualFqcOrder
{
    /// <summary>
    /// FQC检验单创建服务接口
    /// </summary>
    public interface IFQCOrderCreateService
    {
        /// <summary>
        /// 检验单手动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ManualCreateAsync(FQCOrderManualCreateBo bo);

        /// <summary>
        /// 检验单自动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<bool> AutoCreateAsync(FQCOrderAutoCreateAutoBo bo);
    }
}
