using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Core.Enums.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.NIO.NioPushActualDelivery.View
{
    /// <summary>
    /// 状态
    /// </summary>
    public class NioPushActualDeliveryView : NioPushActualDeliveryEntity
    {
        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }
    }
}
