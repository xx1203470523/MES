using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public  class QualUnqualifiedGroupByCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
