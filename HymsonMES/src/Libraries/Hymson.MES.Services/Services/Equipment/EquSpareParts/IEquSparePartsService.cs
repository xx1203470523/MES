using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（备件注册表）
    /// </summary>
    public interface IEquSparePartsService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateEquSparePartsAsync(EquSparePartsSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyEquSparePartsAsync(EquSparePartsSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteEquSparePartsAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquSparePartsAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartsDto?> QueryEquSparePartsByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartsDto>> GetPagedListAsync(EquSparePartsPagedQueryDto pagedQueryDto);

    }
}