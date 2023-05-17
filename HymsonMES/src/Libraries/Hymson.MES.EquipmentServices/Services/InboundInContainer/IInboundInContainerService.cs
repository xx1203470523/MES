using Hymson.MES.EquipmentServices.Dtos.InboundInContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InboundInContainer
{
    /// <summary>
    /// 进站-容器
    /// </summary>
    public interface IInboundInContainerService
    {
        /// <summary>
        /// 进站-容器
        /// </summary>
        /// <param name="inboundInContainerDto"></param>
        /// <returns></returns>
        Task InboundInContainerAsync(InboundInContainerDto inboundInContainerDto);
    }
}
