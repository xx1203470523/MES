using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param
{
    /// <summary>
    /// 马威产品参数
    /// </summary>
    public interface IProcProductParameterGroupMavelRepository
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcedureParamMavelView>> GetParamListAsync(MavelParamQuery query);
    }
}
