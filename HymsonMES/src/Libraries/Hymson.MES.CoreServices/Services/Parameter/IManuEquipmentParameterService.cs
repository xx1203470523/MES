using Hymson.MES.CoreServices.Dtos.Parameter;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public interface IManuEquipmentParameterService
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<EquipmentParameterDto> param);
    }
}
