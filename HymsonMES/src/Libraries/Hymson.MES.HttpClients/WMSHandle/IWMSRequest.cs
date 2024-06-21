
using Hymson.MES.HttpClients.Requests.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 仓库请求
    /// </summary>
    public interface IWMSRequest
    {
        /// <summary>
        /// 领料申请，包含工单借料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(string msg,bool result)> MaterialPickingRequestAsync(MaterialPickingRequest request);
        /// <summary>
        /// 领料取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task MaterialPickingCancelAsync(MaterialPickingRequest request);

        /// <summary>
        /// 退料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task MaterialReturnAsync(MaterialReturnRequest request);
       
    }
}
