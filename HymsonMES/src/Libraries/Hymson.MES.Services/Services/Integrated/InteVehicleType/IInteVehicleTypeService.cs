using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 载具类型维护 service接口
    /// </summary>
    public interface IInteVehicleTypeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteVehicleTypePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleTypeDto>> GetPagedListAsync(InteVehicleTypePagedQueryDto inteVehicleTypePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<long> CreateInteVehicleTypeAsync(InteVehicleTypeCreateDto dto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ModifyInteVehicleTypeAsync(InteVehicleTypeModifyDto dto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteVehicleTypeAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteVehicleTypeDto> QueryInteVehicleTypeByIdAsync(long id);

        /// <summary>
        /// 获取载具类型验证根据vehicleTypeId查询
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleTypeVerifyDto>> QueryInteVehicleTypeVerifyByVehicleTypeIdAsync(long vehicleTypeId);
    }
}
