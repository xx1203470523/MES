/*
 *creator: Karl
 *
 *describe: 载具注册表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 载具注册表 service接口
    /// </summary>
    public interface IInteVehicleService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteVehiclePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleViewDto>> GetPagedListAsync(InteVehiclePagedQueryDto inteVehiclePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleCreateDto"></param>
        /// <returns></returns>
        Task CreateInteVehicleAsync(InteVehicleCreateDto inteVehicleCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteVehicleModifyDto"></param>
        /// <returns></returns>
        Task ModifyInteVehicleAsync(InteVehicleModifyDto inteVehicleModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteVehicleAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteVehicleAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteVehicleDto> QueryInteVehicleByIdAsync(long id);
    }
}
