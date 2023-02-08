using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquEquipment
{
    /// <summary>
    /// 接口（设备注册）
    /// @author Czhipu
    /// @date 2022-11-08
    /// </summary>
    public interface IEquEquipmentService
    {
        /// <summary>
        /// 添加（设备注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> CreateEquEquipmentAsync(EquEquipmentCreateDto parm);

        /// <summary>
        /// 更新（设备注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyEquEquipmentAsync(EquEquipmentModifyDto parm);

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteEquEquipmentAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentDto>> GetPagedListAsync(EquEquipmentPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        Task<List<EquEquipmentDictionaryDto>> QueryEquEquipmentDictionaryAsync();

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomEquEquipmentDetailDto> GetCustomEquEquipmentAsync(long id);

        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="siteCode">站点</param>
        /// <returns></returns>
        Task<EquEquipmentEntity> QueryEquEquipmentAsync(string equipmentCode, string siteCode);

        /// <summary>
        /// 查询设备维护表列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentDto>> GetListAsync(EquEquipmentPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询设备关联硬件列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkHardwareDto>> GetEquimentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto parm);

        /// <summary>
        /// 根据硬件编码硬件类型获取设备
        /// </summary>
        /// <param name="HardwareCode"></param>
        /// <param name="HardwareType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkHardwareEntity> GetLinkHardwareForCodeAndTypeAsync(string HardwareCode, string HardwareType);

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkApiDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto parm);

        /// <summary>
        /// 根据设备id+接口类型获取接口地址
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkApiEntity> GetApiForEquipmentidAndType(long equipmentId, string apiType);
    }
}
