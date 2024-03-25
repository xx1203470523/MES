/*
 *creator: Karl
 *
 *describe: 工单信息表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */

using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query
{
    /// <summary>
    /// 工单信息表 查询参数
    /// </summary>
    public class PlanWorkOrderQuery : QueryAbstraction
    {
        // <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public List<long>? ProductIds { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public List<PlanWorkOrderStatusEnum>? StatusList { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }


        /// <summary>
        /// 工单号模糊条件
        /// </summary>
        public string? OrderCodeLike { get; set; }


        /// <summary>
        /// 产品id
        /// </summary>
        public long? ProductId { get; set; }



        /// <summary>
        /// 工作中心类型;和工作中心保持一致
        /// </summary>
        public long? WorkCenterType { get; set; }

        /// <summary>
        /// 工作中心类型;和工作中心保持一致组
        /// </summary>
        public IEnumerable<long>? WorkCenterTypes { get; set; }


        /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心（车间或者线体）组
        /// </summary>
        public IEnumerable<long>? WorkCenterIds { get; set; }


        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 工艺路线组
        /// </summary>
        public IEnumerable<long>? ProcessRouteIds { get; set; }


        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 产品bom组
        /// </summary>
        public IEnumerable<long>? ProductBOMIds { get; set; }


        /// <summary>
        /// 工单类型
        /// </summary>
        public long? Type { get; set; }

        /// <summary>
        /// 工单类型组
        /// </summary>
        public IEnumerable<long>? Types { get; set; }


        /// <summary>
        /// 数量最小值
        /// </summary>
        public decimal? QtyMin { get; set; }

        /// <summary>
        /// 数量最大值
        /// </summary>
        public decimal? QtyMax { get; set; }


        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：已关闭；
        /// </summary>
        public long? Status { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：已关闭；组
        /// </summary>
        public IEnumerable<PlanWorkOrderStatusEnum>? Statuss { get; set; }


        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例最小值
        /// </summary>
        public decimal? OverScaleMin { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例最大值
        /// </summary>
        public decimal? OverScaleMax { get; set; }


        /// <summary>
        /// 是否锁定;1：是；2：否；   废弃使用
        /// </summary>
        public long? IsLocked { get; set; }

        /// <summary>
        /// 是否锁定;1：是；2：否；   废弃使用组
        /// </summary>
        public IEnumerable<long>? IsLockeds { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 备注模糊条件
        /// </summary>
        public string? RemarkLike { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }


        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }


        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }


        /// <summary>
        /// 更新时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 更新时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }


        /// <summary>
        /// 工厂组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }


        /// <summary>
        /// 锁定前的工单状态;1：未开始；2：下达；3：生产中；4：完成；5：已关闭；
        /// </summary>
        public long? LockedStatus { get; set; }

        /// <summary>
        /// 锁定前的工单状态;1：未开始；2：下达；3：生产中；4：完成；5：已关闭；组
        /// </summary>
        public IEnumerable<long>? LockedStatuss { get; set; }
    }
}
