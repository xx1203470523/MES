/*
 *creator: Karl
 *
 *describe: 工单激活 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
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
    /// 工单激活 分页参数
    /// </summary>
    public class PlanWorkOrderActivationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 工作中心Id (包含线体的和车间)
        /// </summary>
        public List<long> WorkCenterIds { get; set; } = new List<long>();

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool? IsActivation { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string? WorkCenterCode { get; set; }


        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public YesOrNoEnum? IsLocked { get; set; }

        /// <summary>
        /// 计划开始时间  字符串 ：时间范围，逗号分割
        /// </summary>
        public string? PlanStartTime { get; set; }

        public DateTime? PlanStartTimeS
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PlanStartTime))
                {
                    var dateArr = this.PlanStartTime.Split(',');
                    return dateArr.Length > 0 ? Convert.ToDateTime(dateArr[0]) : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? PlanStartTimeE
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PlanStartTime))
                {
                    var dateArr = this.PlanStartTime.Split(',');
                    return dateArr.Length > 1 ? Convert.ToDateTime(dateArr[1]) : null;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
