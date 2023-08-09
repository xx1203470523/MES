using Hymson.MES.EquipmentServices.Dtos.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Parameter.ProductProcessCollection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductProcessCollectionService
    {
        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Collection(ProductProcessParameterDto param);
    }
}
