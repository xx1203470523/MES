/*
 *creator: Karl
 *
 *describe: 设备维修记录    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquRepairOrder;

namespace Hymson.MES.Services.Services.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录 service接口
    /// </summary>
    public interface IEquRepairOrderService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equRepairOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquRepairOrderDto>> GetPagedListAsync(EquRepairOrderPagedQueryDto equRepairOrderPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderCreateDto"></param>
        /// <returns></returns>
        Task CreateEquRepairOrderAsync(EquRepairOrderCreateDto equRepairOrderCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equRepairOrderModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquRepairOrderAsync(EquRepairOrderModifyDto equRepairOrderModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquRepairOrderAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquRepairOrderAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquRepairOrderDto> QueryEquRepairOrderByIdAsync(long id);
    }
}
