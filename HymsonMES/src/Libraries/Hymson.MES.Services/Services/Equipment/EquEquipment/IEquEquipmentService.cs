using Hymson.Infrastructure;
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
        Task<long> CreateAsync(EquEquipmentSaveDto parm);

        /// <summary>
        /// 更新（设备注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquEquipmentSaveDto parm);

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentListDto>> GetPagedListAsync(EquEquipmentPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentDictionaryDto>> GetEquEquipmentDictionaryAsync();

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<GetEquSpotcheckPlanEquipmentRelationListDto>> GetEquSpotcheckPlanEquipmentRelationListAsync(EquEquipmentSpotcheckRelationPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentDto> GetDetailAsync(long id);

        /// <summary>
        /// 查询设备关联硬件列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkHardwareBaseDto>> GetEquimentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto parm);

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLinkApiBaseDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto parm);


        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns></returns>
        Task<EquEquipmentDto> GetByEquipmentCodeAsync(string equipmentCode);

        /// <summary>
        /// 根据设备id+接口类型获取接口地址
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkApiDto> GetApiForEquipmentidAndType(long equipmentId, string apiType);

        /// <summary>
        /// 根据硬件编码硬件类型获取设备
        /// </summary>
        /// <param name="hardwareCode"></param>
        /// <param name="hardwareType"></param>
        /// <returns></returns>
        Task<EquEquipmentLinkHardwareDto> GetLinkHardwareForCodeAndTypeAsync(string hardwareCode, string hardwareType);


        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="EquipmentId"></param>
        /// <returns></returns>
        Task<string> CreateEquEquipmentTokenAsync(long EquipmentId);

        /// <summary>
        /// 查找Token
        /// </summary>
        /// <param name="EquipmentId"></param>
        /// <returns></returns>
        Task<string> GetEquEquipmentTokenAsync(long EquipmentId);

        /// <summary>
        /// 根据设备ID查询对应的验证
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentVerifyDto>> GetEquipmentVerifyByEquipmentIdAsync(long equipmentId);

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        Task<string> CreateEquTokenAsync(long SiteId);

    }
}
