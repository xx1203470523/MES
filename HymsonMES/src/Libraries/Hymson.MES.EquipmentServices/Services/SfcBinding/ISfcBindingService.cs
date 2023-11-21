using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.ManuCommonDto;

namespace Hymson.MES.EquipmentServices.Services.SfcBinding
{
    /// <summary>
    /// 条码绑定
    /// </summary>
    public interface ISfcBindingService
    {
        /// <summary>
        /// 条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param>
        /// <returns></returns>
        Task SfcCirculationBindAsync(SfcBindingDto sfcBindingDto);

    }
}
