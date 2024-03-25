using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 在制品接口
    /// </summary>
    public interface IManuSfcProduceService
    {
        /// <summary>
        /// 根据条码更改条码状态
        /// 用于设备接口产出米数上报
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command);

        /// <summary>
        /// 获取设备最近条码
        /// </summary>
        /// <returns></returns>
        Task<ManuSfcProductMaterialView> GetEquipmentNewestSfc(ManuSfcEquipmentNewestQuery query);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcQuery"></param>
        /// <returns></returns>
        Task<ManuSfcProduceEntity> GetBySFCAsync(ManuSfcProduceBySfcQuery sfcQuery);
    }
}
