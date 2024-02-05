using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ProcessRoute.Query
{
    /// <summary>
    /// 工序查询
    /// </summary>
    public  class ProcessRouteProcedureQuery : PagerInfo
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// 工艺路线id
        /// </summary>
        public long? ProcessRouteId { get; set; }
    }
}
