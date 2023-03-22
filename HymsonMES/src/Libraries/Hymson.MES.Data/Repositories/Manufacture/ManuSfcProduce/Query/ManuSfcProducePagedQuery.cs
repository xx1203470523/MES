/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除） 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 分页参数
    /// </summary>
    public class ManuSfcProducePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcProduceStatusEnum? Status { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? Sfcs { get; set; }
    }
}
