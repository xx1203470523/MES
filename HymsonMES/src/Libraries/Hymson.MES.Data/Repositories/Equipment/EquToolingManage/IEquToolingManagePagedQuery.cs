using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 工具管理表 分页参数
    /// </summary>
    public class IEquToolingManagePagedQuery : PagerInfo
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long? SiteId { get; set; }


        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 工具类型编码
        /// 空值 : false  
        /// </summary>
        public string ToolsTypeCode { get; set; }

        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 更新时间  时间范围  数组
        /// </summary>
        public DateTime[]? UpdatedOn { get; set; }
    }
}
