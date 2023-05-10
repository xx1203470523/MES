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
        /// <exception cref="BusinessException"></exception>
        Task<ManuFacePlateProductionPackageDto> GetManuFacePlateProductionPackageInfo(ManuFacePlateProductionPackageQueryDto param);

        /*
        /// <summary>
        /// 组装
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="BusinessException"></exception>
        Task<string> AddPackageCom(ManuFacePlateProductionPackageAddDto addDto);
        */

    }
}
