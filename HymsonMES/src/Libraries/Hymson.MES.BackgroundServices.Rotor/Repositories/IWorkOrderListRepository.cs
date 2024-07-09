using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 工单产品仓储
    /// </summary>
    public interface IWorkOrderListRepository
    {
        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<WorkOrderListDto>> GetList(string sql);
    }
}
