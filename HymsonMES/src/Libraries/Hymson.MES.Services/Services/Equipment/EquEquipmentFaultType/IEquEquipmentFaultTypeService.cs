using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Equipment.EquEquipmentFaultType
{
    /// <summary>
    /// 设备故障类型服务接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IEquEquipmentFaultTypeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<EquipmentFaultTypeDto>> GetPageListAsync(EquipmentFaultTypePagedQueryDto param);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> CreateQualUnqualifiedGroupAsync(EQualUnqualifiedGroupCreateDto param);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ModifyQualUnqualifiedGroupAsync(EQualUnqualifiedGroupModifyDto param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualUnqualifiedGroupAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquipmentFaultTypeDto> QueryQualUnqualifiedGroupByIdAsync(long id);

        /// <summary>
        /// 获取设备故障类型关联故障现象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<EquipmentFaultTypePhenomenonRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id);

        /// <summary>
        /// 获取设备故障类型关联设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<EquipmentFaultTypeEquipmentGroupRelationDto>> GetQualUnqualifiedCodeProcedureRelationByIdAsync(long id);
    }
}
