using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquSparePart
{
    /// <summary>
    /// 业务接口层（备件注册） 
    /// </summary>
    public interface IEquSparePartService
    {
        /// <summary>
        /// 添加（备件注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquSparePartSaveDto createDto);

        /// <summary>
        /// 修改（备件注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquSparePartSaveDto modifyDto);

        /// <summary>
        /// 删除（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除（备件注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(IEnumerable<long>  idsArr);

        /// <summary>
        /// 分页查询列表（备件注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartDto>> GetPagedListAsync(EquSparePartPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartDto> GetDetailAsync(long id);
    }
}
