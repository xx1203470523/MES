/*
 *creator: Karl
 *
 *describe: 工单激活    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 工单激活Dto
    /// </summary>
    public record PlanWorkOrderActivationDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 线体Id
        /// </summary>
        public long LineId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 工单激活新增Dto
    /// </summary>
    public record PlanWorkOrderActivationCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 线体Id
        /// </summary>
        public long LineId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 工单激活更新Dto
    /// </summary>
    public record PlanWorkOrderActivationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 线体Id
        /// </summary>
        public long LineId { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 工单激活分页Dto
    /// </summary>
    public class PlanWorkOrderActivationPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 线体Id
        /// </summary>
        public long? LineId { get; set; }

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
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

    }

    /// <summary>
    /// 工单激活分页Dto--根据资源先找到线体
    /// </summary>
    public class PlanWorkOrderActivationAboutResPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

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
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

    }


    /// <summary>
    /// 激活/取消激活 工单
    /// </summary>
    public class ActivationWorkOrderDto 
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 是否需要激活   true 需要激活  false 需要取消激活
        /// </summary>
        public bool IsNeedActivation { get; set; }

        /// <summary>
        /// 线体ID
        /// </summary>
        public long LineId { get; set; }
    }

    /// <summary>
    /// 工单激活相关 视图
    /// </summary>
    public record PlanWorkOrderActivationListDetailViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

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
        public string InputQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public string FinishProductQuantity { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealEnd { get; set; }


        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivation { get; set; }
    }

}
