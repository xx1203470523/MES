using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（条码档位表）
    /// </summary>
    public interface IManuSfcGradeService
    {
        /// <summary>
        /// 根据SFC获取档位信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<ManuSfcGradeViewDto?> GetBySFCAsync(string sfc);
    }
}