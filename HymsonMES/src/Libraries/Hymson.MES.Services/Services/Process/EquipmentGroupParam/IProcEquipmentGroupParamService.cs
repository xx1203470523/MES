/*
 *creator: Karl
 *
 *describe: 设备参数组    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 设备参数组 service接口
    /// </summary>
    public interface IProcEquipmentGroupParamService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procEquipmentGroupParamPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEquipmentGroupParamDto>> GetPagedListAsync(ProcEquipmentGroupParamPagedQueryDto procEquipmentGroupParamPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamCreateDto"></param>
        /// <returns></returns>
        Task CreateProcEquipmentGroupParamAsync(ProcEquipmentGroupParamCreateDto procEquipmentGroupParamCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEquipmentGroupParamModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcEquipmentGroupParamAsync(ProcEquipmentGroupParamModifyDto procEquipmentGroupParamModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcEquipmentGroupParamAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcEquipmentGroupParamAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamDto> QueryProcEquipmentGroupParamByIdAsync(long id);
    }
}
