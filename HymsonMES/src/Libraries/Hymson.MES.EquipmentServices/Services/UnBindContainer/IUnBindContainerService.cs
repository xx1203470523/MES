using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.UnBindContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.UnBindContainer
{
    /// <summary>
    /// 容器解绑
    /// </summary>
    public interface IUnBindContainerService
    {
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindContainerRequest"></param>
        /// <returns></returns>
        Task UnBindContainerAsync(UnBindContainerRequest unBindContainerRequest);
    }
}
