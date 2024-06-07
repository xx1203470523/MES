using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（设备状态时间）
    /// </summary>
    public interface IManuEquipmentStatusTimeService
    {
        /// <summary>
        /// 设备状态监控报表分页查询
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuEquipmentStatusReportViewDto>> GetPageListAsync(ManuEquipmentStatusTimePagedQueryDto pagedQueryDto);
    }
}