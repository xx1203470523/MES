using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 工具管理表 查询参数
    /// </summary>
    public class EquToolingManageQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        ///工具编码
        /// 空值 : false  
        /// </summary>
        public IEnumerable<string>?  Codes { get; set; }

        ///资源类型Id列表
        public IEnumerable<long> ToolTypeIds { get; set; }
    }
}
