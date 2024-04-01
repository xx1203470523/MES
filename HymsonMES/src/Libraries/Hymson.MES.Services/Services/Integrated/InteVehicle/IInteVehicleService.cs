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
    /// 载具 service接口
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

        /// <summary>
        /// 获取载具验证
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        Task<InteVehicleVerifyDto> QueryVehicleVerifyByVehicleIdAsync(long vehicleId);

        /// <summary>
        /// 获取载具装载
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightDto>> QueryVehicleFreightByVehicleIdAsync(long vehicleId);
        /// <summary>
        /// 获取载具装载
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        Task<InteVehicleStackView> QueryVehicleFreightByPalletNoAsync(string palletNo);
        /// <summary>
        /// 载具操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task VehicleOperationAsync(InteVehicleOperationDto dto);
        Task VehicleBindOperationAsync(InteVehicleOperationDto dto);
        Task VehicleUnBindOperationAsync(InteVehicleOperationDto dto);
        Task VehicleClearOperationAsync(InteVehicleOperationDto dto);
    }
}
