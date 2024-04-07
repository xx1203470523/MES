using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;

namespace Hymson.MES.Services.Services.EquProcessParamRecord
{
    /// <summary>
    /// 服务接口（过程参数记录表）
    /// </summary>
    public interface IEquProcessParamRecordService
    {
        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<EquProcessParamRecordSaveDto> saveDtoList);
    }
}