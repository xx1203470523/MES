using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 附件保存
    /// </summary>
    public record QualIqcOrderSaveAttachmentDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// IQC检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    /// <summary>
    /// 检验单状态Dto
    /// </summary>
    public record QualOrderOperationStatusDto
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
    public record QualIqcOrderSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 检验类型ID
        /// </summary>
        public long IQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<QualIqcOrderParameterSaveDto> Details { get; set; }

    }

    /// <summary>
    /// iqc检验单新增/更新Dto
    /// </summary>
    public record QualIqcOrderParameterSaveDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 参数附件
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto>? Attachments { get; set; }

    }

    /// <summary>
    /// iqc检验单Dto
    /// </summary>
    public record QualIqcOrderDto : BaseEntityDto
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
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

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
        /// qual_iqc_inspection_item_copy 的Id
        /// </summary>
        public string IqcInspectionItemId { get; set; }

        /// <summary>
        /// 收货单详情Id
        /// </summary>
        public long? MaterialReceiptDetailId { get; set; }

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public decimal? PlanQty { get; set; }

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public string IsQualifiedText { get; set; }

        /// <summary>
        /// 报检人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

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
    /// iqc检验单分页Dto
    /// </summary>
    public class QualIqcOrderPagedQueryDto : PagerInfo { }

    /// <summary>
    /// 生成检验单Dto
    /// </summary>
    public record GenerateInspectionDto
    {
        /// <summary>
        /// 收货单
        /// </summary>
        public long ReceiptId { get; set; }

        /// <summary>
        /// 收货单
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 收货单明细ID集合
        /// </summary>
        public IEnumerable<long> Details { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record OrderParameterDetailQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 检验类型ID
        /// </summary>
        public long? IQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record OrderParameterDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 样本条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 参数Id proc_parameter 的id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数数据类型
        /// </summary>
        public DataTypeEnum ParameterDataType { get; set; }

        /// <summary>
        /// 检验器具
        /// </summary>
        public IQCUtensilTypeEnum Utensil { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        public decimal Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

}
