using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（工具类型管理）
    /// </summary>
    public interface IEquToolsTypeService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquToolsTypeSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquToolsTypeSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

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
        Task<EquToolsTypeDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolsTypeCofigEquipmentGroupDto> GetEquipmentRelationAsync(long id);

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolsTypeCofigMaterialDto> GetMaterialRelationAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolsTypeDto>> GetPagedListAsync(EquToolsTypePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentGroupListDto>> GetEquipmentsAsync(long id);

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialDto>> GetMaterialsAsync(long id);

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        Task ImportAsync(IFormFile formFile);
    }
}