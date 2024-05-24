/*
 *creator: Karl
 *
 *describe: 设备点检模板    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquMaintenanceTemplate;

namespace Hymson.MES.Services.Services.EquMaintenanceTemplate
{
    /// <summary>
    /// 设备点检模板 service接口
    /// </summary>
    public interface IEquMaintenanceTemplateService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="EquMaintenanceTemplatePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenanceTemplateDto>> GetPagedListAsync(EquMaintenanceTemplatePagedQueryDto EquMaintenanceTemplatePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateCreateDto"></param>
        /// <returns></returns>
        Task CreateEquMaintenanceTemplateAsync(EquMaintenanceTemplateCreateDto EquMaintenanceTemplateCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquMaintenanceTemplateModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquMaintenanceTemplateAsync(EquMaintenanceTemplateModifyDto EquMaintenanceTemplateModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquMaintenanceTemplateAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquMaintenanceTemplateAsync(EquMaintenanceTemplateDeleteDto param); 

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenanceTemplateDto> QueryEquMaintenanceTemplateByIdAsync(long id);

        #region 关联信息

        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<GetItemRelationListDto>> QueryItemRelationListAsync(GetEquMaintenanceTemplateItemRelationDto param);


        /// <summary>
        /// 获取模板关联信息（设备组）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<QueryEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync(GetEquMaintenanceTemplateItemRelationDto param);

        #endregion
    }
}
