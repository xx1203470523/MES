using Hymson.MES.EquipmentServices.Request.BindContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.BindContainer
{
    /// <summary>
    /// 容器绑定
    /// </summary>
    public interface IBindContainerService
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindContainerRequest"></param>
        /// <returns></returns>
        Task BindContainerAsync(BindContainerRequest bindContainerRequest);


        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindContainerRequest"></param>
        /// <returns></returns>
        Task UnBindContainerAsync(UnBindContainerRequest unBindContainerRequest);
    }
}
