using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（工具类型）
    /// </summary>
    public interface IEquToolingTypeService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateEquSparePartsGroupAsync(EquToolingTypeSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyEquSparePartsGroupAsync(EquToolingTypeSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteEquSparePartsGroupAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquSparePartsGroupAsync(long[] ids);

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<EquToolingTypeGroupEquipmentGroupRelationSaveDto>> GetSparePartsEquipmentGroupRelationByIdAsync(long id);

        ///// <summary>
        ///// 获取物料
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<List<EquToolingTypeGroupEquipmentGroupRelationDto>> GetToolingTypeGroupMaterialIdRelationByIdAsync(long id);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolingTypeDto?> QueryEquSparePartsGroupByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolingTypeDto>> GetPagedListAsync(EquToolingTypeQueryDto pagedQueryDto);

    }
}