using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 生产退料单新增/更新Dto
    /// </summary>
    public record ManuReturnOrderSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 退料单据号
        /// </summary>
        public string ReturnOrderCode { get; set; }

        /// <summary>
        /// 工单id，工单借料情况下，接收物料的工单
        /// </summary>
        public string TargetWorkOrderCode { get; set; }

        /// <summary>
        /// 退料单类型0:工单退料 1:工单借料
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 状态0:审批中，1：审批失败，2：审批成功3.已退料
        /// </summary>
        public WhWarehouseReturnStatusEnum Status { get; set; }



        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

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

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单id，待退料的工单
        /// </summary>
        public string SourceWorkOrderCode { get; set; }


    }

    /// <summary>
    /// 生产退料单Dto
    /// </summary>
    public record ManuReturnOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 退料单据号
        /// </summary>
        public string ReturnOrderCode { get; set; }

        /// <summary>
        /// 工单id，工单借料情况下，接收物料的工单
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 退料单类型0:工单退料 1:工单借料
        /// </summary>
        public ManuReturnTypeEnum Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseReturnStatusEnum Status { get; set; }



        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

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

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单id，待退料的工单
        /// </summary>
        public string SourceWorkOrderCode { get; set; }


    }

    /// <summary>
    /// 生产退料单分页Dto
    /// </summary>
    public class ManuReturnOrderPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 退料单号
        /// </summary>
        public string? ReturnOrderCode { get; set; }

        /// <summary>
        /// 编码（工单）
        /// </summary>
        public string? WorkOrderCode { get; set; }
    }

}
