using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 检验单状态Dto
    /// </summary>
    public record QualOrderLiteOperationStatusDto
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
    public record QualIqcOrderLiteSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<QualIqcOrderLiteDetailSaveDto> Details { get; set; }

    }

    /// <summary>
    /// iqc检验单新增/更新Dto
    /// </summary>
    public record QualIqcOrderLiteDetailSaveDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal? UnQualifiedQty { get; set; }

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
    public record QualIqcOrderLiteDto : BaseEntityDto
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
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 通知单号
        /// </summary>
        public string? InformCode { get; set; }

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

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
    public record QualIqcOrderLiteBaseDto : BaseEntityDto
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
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum Status { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiptUser { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        public string ReceiptTime { get; set; }

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
    public record QualIqcOrderLiteDetailDto : BaseEntityDto
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
        /// 供应商批次
        /// </summary>
        public string SupplierBatch { get; set; }

        /// <summary>
        /// 内部
        /// </summary>
        public string InternalBatch { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long? SupplierId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal? UnQualifiedQty { get; set; }

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

        /// <summary>
        /// 是否免检
        /// </summary>
        public TrueOrFalseEnum IsFree { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否免检
        /// </summary>
        public string IsFreeText { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

    }


    /// <summary>
    /// iqc检验单物料Dto（详情）
    /// </summary>
    public record QualIqcOrderLiteMatDetailDto : BaseEntityDto
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
        /// 物料收货数量
        /// </summary>
        public string qty { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

    }

    /// <summary>
    /// iqc检验单分页Dto
    /// </summary>
    public class QualIqcOrderLitePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string? ReceiptNum { get; set; }

        /// <summary>
        /// 通知单号
        /// </summary>
        public string? InformCode { get; set; }

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /*
        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
        */

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string? SupplierName { get; set; }

        /*
        /// <summary>
        /// 供应商批次
        /// </summary>
        public string? SupplierBatch { get; set; }

        /// <summary>
        /// 内部
        /// </summary>
        public string? InternalBatch { get; set; }
        */

        /// <summary>
        /// 检验状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public IQCLiteStatusEnum? Status { get; set; }

    }

    /// <summary>
    /// 生成检验单Dto
    /// </summary>
    public record GenerateOrderLiteDto
    {
        /// <summary>
        /// 收货单
        /// </summary>
        public long ReceiptId { get; set; }

        /// <summary>
        /// 收货单
        /// </summary>
        public string ReceiptNum { get; set; }

        /*
        /// <summary>
        /// 收货单明细ID集合
        /// </summary>
        public IEnumerable<long> Details { get; set; }
        */

    }

}
