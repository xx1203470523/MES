/*
 *creator: Karl
 *
 *describe: 设备台账信息    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;

namespace Hymson.MES.Services.Services.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息 service接口
    /// </summary>
    public interface IEquEquipmentRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equEquipmentRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentRecordPagedViewDto>> GetPagedListAsync(EquEquipmentRecordPagedQueryDto equEquipmentRecordPagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentRecordDto> QueryEquEquipmentRecordByIdAsync(long id);
    }
}
