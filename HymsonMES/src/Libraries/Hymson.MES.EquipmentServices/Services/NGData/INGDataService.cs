using Hymson.MES.EquipmentServices.Dtos.NGData;

namespace Hymson.MES.EquipmentServices.Services
{
    /// <summary>
    /// 获取条码NG数据服务
    /// </summary>
    public interface INGDataService
    {
        /// <summary>
        /// 获取条码NG数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NGDataDto> GetNGDataAsync(NGDataQueryDto param);
    }
}
