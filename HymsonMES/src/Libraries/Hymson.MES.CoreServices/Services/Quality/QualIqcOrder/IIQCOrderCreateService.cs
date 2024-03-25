using Hymson.MES.CoreServices.Bos.Quality;

namespace Hymson.MES.CoreServices.Services.Quality
{
    /// <summary>
    /// IQC检验单创建服务接口
    /// </summary>
    public interface IIQCOrderCreateService
    {
        /// <summary>
        /// IQC检验单创建
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> CreateAsync(IQCOrderCreateBo bo);
    }
}
