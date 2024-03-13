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
    }
}
