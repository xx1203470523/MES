/*
 *creator: Karl
 *
 *describe: 条码信息表 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query
{
    /// <summary>
    /// 车间作业控制报告 分页参数
    /// </summary>
    public class WorkshopJobControlReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SFCStatus { get; set; }

        /// <summary>
        /// 条码在制状态
        /// </summary>
        public SfcProduceStatusEnum? SFCProduceStatus { get; set; }

        /// <summary>
        /// 是否锁
        /// </summary>
        public TrueOrFalseEnum? SFCIsLock { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }
    }
}
