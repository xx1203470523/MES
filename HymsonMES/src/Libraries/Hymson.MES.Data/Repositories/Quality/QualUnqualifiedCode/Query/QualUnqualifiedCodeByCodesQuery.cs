using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query
{
    /// <summary>
    /// 批量查询编码实体
    /// </summary>
    public class QualUnqualifiedCodeByCodesQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long? Site { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string[] Codes { get; set; }
    }
}
