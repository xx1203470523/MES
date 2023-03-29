/*
 *creator: Karl
 *
 *describe: 产品不良录入    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 产品不良录入，数据实体对象   
    /// manu_product_bad_record
    /// @author zhaoqing
    /// @date 2023-03-27 03:49:17
    /// </summary>
    public class ManuProductBadRecordEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public long? FoundBadOperationId { get; set; }

       /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutflowOperationId { get; set; }

       /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long? UnqualifiedId { get; set; }

       /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

       /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

       /// <summary>
        /// 不良来源;·1、设备复投不良  2、人工录入不良
        /// </summary>
        public ProductBadRecordSourceEnum? Source { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
    }
}
