using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcParameterLinkTypeView :BaseEntity 
    {
        ///// <summary>
        ///// 所属站点代码
        ///// </summary>
        //public string SiteCode { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// ID（标准参数）
        /// </summary>
        public long ParameterID { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
