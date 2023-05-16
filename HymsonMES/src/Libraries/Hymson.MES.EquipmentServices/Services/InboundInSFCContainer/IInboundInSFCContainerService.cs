using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InboundInSFCContainer
{
    /// <summary>
    /// 进站-电芯和托盘-装盘2
    /// </summary>
    public interface IInboundInSFCContainerService
    {
        /// <summary>
        /// 进站-电芯和托盘-装盘2
        /// </summary>
        /// <param name="inboundInSFCContainerRequest"></param>
        /// <returns></returns>
        Task InboundInSFCContainerAsync(InboundInSFCContainerRequest inboundInSFCContainerRequest);
    }
}
