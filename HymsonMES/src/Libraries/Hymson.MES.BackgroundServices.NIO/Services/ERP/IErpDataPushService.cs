using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Services.ERP
{
    /// <summary>
    /// ERP相关数据推送
    /// </summary>
    public interface IErpDataPushService
    {
        /// <summary>
        /// NIO合作伙伴精益与库存信息
        /// </summary>
        /// <returns></returns>
        Task NioStockInfoAsync();

        /// <summary>
        /// 关键下级键
        /// </summary>
        /// <returns></returns>
        Task NioKeyItemInfoAsync();

        /// <summary>
        /// 实际交付情况
        /// </summary>
        /// <returns></returns>
        Task NioActualDeliveryAsync();
    }
}
