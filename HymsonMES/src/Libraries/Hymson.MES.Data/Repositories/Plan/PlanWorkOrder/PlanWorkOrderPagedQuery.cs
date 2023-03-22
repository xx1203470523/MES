/*
 *creator: Karl
 *
 *describe: 工单信息表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表 分页参数
    /// </summary>
    public class PlanWorkOrderPagedQuery : PagerInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public YesOrNoEnum? IsLocked { get; set; }

        /// <summary>
        /// 计划开始时间  开始
        /// </summary>
        public DateTime? PlanStartTimeS { get; set; }

        /// <summary>
        /// 计划开始时间  结束
        /// </summary>
        public DateTime? PlanStartTimeE { get; set; }
    }
}
