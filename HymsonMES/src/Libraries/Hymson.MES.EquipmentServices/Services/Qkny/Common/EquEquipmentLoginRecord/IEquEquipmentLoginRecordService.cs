using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;

namespace Hymson.MES.Services.Services.EquEquipmentLoginRecord
{
    /// <summary>
    /// 服务接口（操作员登录记录）
    /// </summary>
    public interface IEquEquipmentLoginRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquEquipmentLoginRecordSaveDto saveDto);
    }
}