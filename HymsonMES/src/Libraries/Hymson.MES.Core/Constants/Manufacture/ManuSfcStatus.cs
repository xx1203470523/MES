using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Constants.Manufacture
{

    /// <summary>
    /// 条码常量
    /// </summary>
    public static class ManuSfcStatus
    {
        /// <summary>
        /// 条码在制品状态为 排队 活动 在制完成
        /// </summary>
        public static readonly IEnumerable<SfcStatusEnum> sfcStatusInProcess = new List<SfcStatusEnum>() {
                 SfcStatusEnum.lineUp, SfcStatusEnum.Activity, SfcStatusEnum.InProductionComplete
        };
    }
}
