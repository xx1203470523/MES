using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query
{
    /// <summary>
    /// 不合格代码查询参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long? UnqualifiedGroupId { get; set; }
    }
}
