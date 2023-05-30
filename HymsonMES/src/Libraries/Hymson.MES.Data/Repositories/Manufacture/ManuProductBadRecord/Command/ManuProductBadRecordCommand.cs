using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command
{
    public class ManuProductBadRecordCommand
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 处置结果
        /// </summary>
        public ProductBadDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    public class ManuProductBadRecordUpdateCommand
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; } 

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 处置结果
        /// </summary>
        public ProductBadDisposalResultEnum? DisposalResult { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
