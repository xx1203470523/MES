using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;

namespace Hymson.MES.Services.Services.EquToolLifeRecord
{
    /// <summary>
    /// 服务接口（设备夹具寿命）
    /// </summary>
    public interface IEquToolLifeRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquToolLifeRecordSaveDto saveDto);
    }
}