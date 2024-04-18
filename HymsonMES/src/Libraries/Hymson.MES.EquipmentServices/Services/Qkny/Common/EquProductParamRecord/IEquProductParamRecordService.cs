using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquProductParamRecord;

namespace Hymson.MES.Services.Services.EquProductParamRecord
{
    /// <summary>
    /// 服务接口（产品参数记录表）
    /// </summary>
    public interface IEquProductParamRecordService
    {
        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<EquProductParamRecordSaveDto> saveDtoList);
    }
}