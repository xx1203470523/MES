using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    public class InteContainerInfoEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 容器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 状态;0-新建 1-启用 2-保留3-废弃
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
