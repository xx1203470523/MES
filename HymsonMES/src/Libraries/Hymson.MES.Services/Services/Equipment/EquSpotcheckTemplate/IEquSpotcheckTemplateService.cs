/*
 *creator: Karl
 *
 *describe: 设备点检模板    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;

namespace Hymson.MES.Services.Services.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板 service接口
    /// </summary>
    public interface IEquSpotcheckTemplateService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equSpotcheckTemplatePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckTemplateDto>> GetPagedListAsync(EquSpotcheckTemplatePagedQueryDto equSpotcheckTemplatePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateCreateDto"></param>
        /// <returns></returns>
        Task CreateEquSpotcheckTemplateAsync(EquSpotcheckTemplateCreateDto equSpotcheckTemplateCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSpotcheckTemplateModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquSpotcheckTemplateAsync(EquSpotcheckTemplateModifyDto equSpotcheckTemplateModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquSpotcheckTemplateAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquSpotcheckTemplateAsync(EquSpotcheckTemplateDeleteDto param); 

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckTemplateDto> QueryEquSpotcheckTemplateByIdAsync(long id);

        #region 关联信息

        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<GetItemRelationListDto>> QueryItemRelationListAsync(GetEquSpotcheckTemplateItemRelationDto param);


        /// <summary>
        /// 获取模板关联信息（设备组）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<QueryEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync(GetEquSpotcheckTemplateItemRelationDto param);

        #endregion
    }
}
