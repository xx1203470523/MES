/*
 *creator: Karl
 *
 *describe: 工单激活相关信息表 视图类 
 *builder:  Karl
 *build datetime: 2023-03-29 14:02:17
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活相关信息表 查询参数
    /// </summary>
    public class PlanWorkOrderActivationListDetailView : BaseEntity
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工作中心类型;和工作中心保持一致
        /// </summary>
        public WorkCenterTypeEnum? WorkCenterType { get; set; }

        /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// bom版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string WorkCenterCode { get; set; }


        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealEnd { get; set; }


        /// <summary>
        /// 工单激活Id
        /// </summary>
        public long WorkOrderActivationId { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivation 
        { 
            get 
            {
                return WorkOrderActivationId > 0;
            } 
        }
    }
}
