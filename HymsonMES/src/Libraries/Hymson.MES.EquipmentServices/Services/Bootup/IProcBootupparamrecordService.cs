/*
 *creator: Karl
 *
 *describe: 开机参数采集表    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.EquipmentServices.Services.Process
{
    /// <summary>
    /// 开机参数采集表 service接口
    /// </summary>
    public interface IProcBootupparamrecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procBootupparamrecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBootupparamrecordDto>> GetPagedListAsync(ProcBootupparamrecordPagedQueryDto procBootupparamrecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootupparamrecordCreateDto"></param>
        /// <returns></returns>
        Task CreateProcBootupparamrecordAsync(ProcBootupparamrecordCreateDto procBootupparamrecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBootupparamrecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcBootupparamrecordAsync(ProcBootupparamrecordModifyDto procBootupparamrecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcBootupparamrecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcBootupparamrecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBootupparamrecordDto> QueryProcBootupparamrecordByIdAsync(long id);
    }
}
