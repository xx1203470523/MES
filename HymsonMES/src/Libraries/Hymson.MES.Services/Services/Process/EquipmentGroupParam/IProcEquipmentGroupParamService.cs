using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Microsoft.AspNetCore.Http;

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
        Task<PagedInfo<ProcEquipmentGroupParamViewDto>> GetPagedListAsync(ProcEquipmentGroupParamPagedQueryDto procEquipmentGroupParamPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProcEquipmentGroupParamCreateDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task ModifyAsync(ProcEquipmentGroupParamModifyDto saveDto);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamViewDto> QueryProcEquipmentGroupParamByIdAsync(long id);

        /// <summary>
        /// 获取载具类型验证根据vehicleTypeId查询
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamDetailDto>> QueryProcEquipmentGroupParamDetailByRecipeIdAsync(long recipeId);

        /// <summary>
        /// 分页查询详情的参数
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterDto>> GetDetailParamByProductIdAndProcedureIdPagedAsync(ProcEquipmentGroupParamDetailParamPagedQueryDto queryDto);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);


        /// <summary>
        /// 导入信息表格
        /// </summary>
        /// <returns></returns>
        Task ImportInteEquipmentGroupParamAsync(IFormFile formFile);
    }
}
