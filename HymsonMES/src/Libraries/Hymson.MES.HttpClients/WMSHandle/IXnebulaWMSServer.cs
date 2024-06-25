

using Hymson.MES.HttpClients.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 仓库服务
    /// </summary>
    public interface IXnebulaWMSServer
    {
        /// <summary>
        /// 领料申请，包含工单借料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialPickingRequestAsync(MaterialPickingRequestDto request);
        /// <summary>
        /// 领料取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialPickingCancelAsync(MaterialPickingCancelDto request);

        /// <summary>
        /// 退料申请，请求发送成功之后，库存即刻扣除，后续WMS反馈有问题时候再加回来
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialReturnRequestAsync(MaterialReturnRequestDto request);
        /// <summary>
        /// 取消退料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialReturnCancelAsync(MaterialReturnCancelDto request);

    }
}
