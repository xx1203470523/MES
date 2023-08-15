using Hymson.MES.SystemServices.Dtos.Manufacture;

namespace Hymson.MES.SystemServices.Services.Manufacture
{
    /// <summary>
    /// 条码流转查询服务
    /// </summary>
    public interface IManuSfcCirculationService
    {
        /// <summary>
        /// 根据Pack条码获取关联记录
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcCirculationDto> GetRelationShipByPackAsync(string sfc);
    }
}
