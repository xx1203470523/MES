using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 质量锁定实体类
    /// </summary>
    public class QualityLockCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 锁
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// 将来锁，锁定的工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
