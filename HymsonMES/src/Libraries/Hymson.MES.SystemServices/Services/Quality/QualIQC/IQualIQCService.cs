using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Quality
{
    /// <summary>
    /// 服务接口（来料检验）
    /// </summary>
    public interface IQualIQCService
    {
        /// <summary>
        /// 提交（来料检验）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> SubmitIncomingAsync(IQCRequestDto dto);

    }
}
