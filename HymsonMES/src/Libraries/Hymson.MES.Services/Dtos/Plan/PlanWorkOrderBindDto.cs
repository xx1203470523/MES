/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 工单激活（物理删除）Dto
    /// </summary>
    public record PlanWorkOrderBindDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }


    /// <summary>
    /// 工单激活（物理删除）新增Dto
    /// </summary>
    public record PlanWorkOrderBindCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 工单激活（物理删除）更新Dto
    /// </summary>
    public record PlanWorkOrderBindModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }



    }

    /// <summary>
    /// 工单激活（物理删除）分页Dto
    /// </summary>
    public class PlanWorkOrderBindPagedQueryDto : PagerInfo
    {
    }

    /// <summary>
    /// 绑定激活工单 
    /// </summary>
    public class BindActivationWorkOrderDto{
        /// <summary>
        /// 工单
        /// </summary>
        public List<long> WorkOrderIds { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 已经绑定的工单信息
    /// </summary>
    public class HasBindWorkOrderInfoDto 
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

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
    }
}
