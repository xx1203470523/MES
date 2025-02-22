using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板-生产过站 service接口
    /// </summary>
    public interface IManuFacePlateProductionService
    {
        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ManuFacePlateProductionPackageDto> GetManuFacePlateProductionPackageInfoAsync(ManuFacePlateProductionPackageQueryDto param);

    }
}
