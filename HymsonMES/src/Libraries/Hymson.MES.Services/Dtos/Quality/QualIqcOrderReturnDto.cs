using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 检验单状态Dto
    /// </summary>
    public record QualOrderReturnOperationStatusDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OrderOperateTypeEnum OperationType { get; set; }

    }

    /// <summary>
    /// iqc检验单新增/更新Dto
    /// </summary>
    public record QualIqcOrderReturnSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<QualIqcOrderReturnDetailSaveDto> Details { get; set; }

    }

    /// <summary>
    /// iqc检验单新增/更新Dto
    /// </summary>
    public record QualIqcOrderReturnDetailSaveDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 不合格处理方式;1-让步；2-挑选；3-返工；4-报废；
        /// </summary>
        public IQCHandMethodEnum? HandMethod { get; set; }

    }

    /// <summary>
    /// iqc检验单Dto
    /// </summary>
    public record QualIqcOrderReturnDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编码
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum Status { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public string IsQualifiedText { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public IQCHandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 报检人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public string InspectionBy { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionOn { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandledBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandledOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// iqc检验单Dto（详情）
    /// </summary>
    public record QualIqcOrderReturnBaseDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum Status { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 退料人
        /// </summary>
        public string ReturnUser { get; set; }

        /// <summary>
        /// 退料时间
        /// </summary>
        public string ReturnTime { get; set; }

        /// <summary>
        /// 送检人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 送检时间
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdatedOn { get; set; }
    }

    /// <summary>
    /// iqc检验单Dto（详情）
    /// </summary>
    public record QualIqcOrderReturnDetailDto : BaseEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long? MaterialId { get; set; }

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
        /// 供应商Id
        /// </summary>
        public long? SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public string IsQualifiedText { get; set; }

        /// <summary>
        /// 不合格处理方式;1-让步；2-挑选；3-返工；4-报废；
        /// </summary>
        public IQCHandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public string HandMethodText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// iqc检验单分页Dto
    /// </summary>
    public class QualIqcOrderReturnPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /*
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
        */

        /// <summary>
        /// 退料单号
        /// </summary>
        public string? ReqOrderCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum? Status { get; set; }

    }

    /// <summary>
    /// 生成检验单Dto
    /// </summary>
    public record GenerateOrderReturnDto
    {
        /// <summary>
        /// 退料单
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 退料单
        /// </summary>
        public string OrderNo { get; set; }

        /*
        /// <summary>
        /// 退料单明细ID集合
        /// </summary>
        public IEnumerable<long> Details { get; set; }
        */

    }

}
