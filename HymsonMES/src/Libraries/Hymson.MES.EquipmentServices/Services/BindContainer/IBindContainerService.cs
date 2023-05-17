using Hymson.MES.EquipmentServices.Dtos.BindContainer;
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
        /// <param name="bindContainerDto"></param>
        /// <returns></returns>
        Task BindContainerAsync(BindContainerDto bindContainerDto);


        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindContainerDto"></param>
        /// <returns></returns>
        Task UnBindContainerAsync(UnBindContainerDto unBindContainerDto);
    }
}
