using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验单新增/更新Dto
    /// </summary>
    public record QualOqcOrderSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 出货单明细Ids
        /// </summary>
        public List<long> ShipmentDetailIds { get; set; }
        
    }

    /// <summary>
    /// OQC检验单Dto
    /// </summary>
    public record QualOqcOrderDto : BaseEntityDto
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
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// OQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentOrderId { get; set; }

        /// <summary>
        /// 出货数量
        /// </summary>
        public decimal ShipmentQty { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

        ///// <summary>
        ///// 删除标识
        ///// </summary>
        //public long IsDeleted { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string? CustomCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomName { get; set; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string? SupplierBatch { get; set; }

        /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public string? OperateBy { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? OperateOn { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-返工 4-报废）
        /// </summary>
        public OQCHandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 不合格处理人
        /// </summary>
        public string? ProcessedBy { get; set; }

        /// <summary>
        /// 不合格处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 不合格处理备注
        /// </summary>
        public string? UnqualifiedHandRemark { get; set; }

        /// <summary>
        /// 出货单单号
        /// </summary>
        public string? ShipmentNum { get; set; }

        /// <summary>
        /// 状态Str
        /// </summary>
        public string? StatusStr { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto>? Attachments { get; set; }
    }

    /// <summary>
    /// OQC检验单分页Dto
    /// </summary>
    public class QualOqcOrderPagedQueryDto : PagerInfo {

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 出货单号
        /// </summary>
        public string? ShipmentNum { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string? CustomCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomName { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-返工 4-报废）
        /// </summary>
        public OQCHandMethodEnum? HandMethodResult { get; set; }

    }

    /// <summary>
    /// 获取OQC单据类型Dto
    /// </summary>
    public record QualOqcOrderTypeOutDto {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }

        ///// <summary>
        ///// 样本数量（应检数量）
        ///// </summary>
        //public int SampleQty { get; set; }

        ///// <summary>
        ///// 已检数量
        ///// </summary>
        //public int CheckedQty { get; set; }
    }

    /// <summary>
    /// 校验样品条码QuqryDto
    /// </summary>
    public record CheckBarCodeQuqryDto { 

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 出货单Id
        /// </summary>
        public long? ShipmentId { get; set; }

        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long? InspectionOrderId { get; set;}

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }
    }

    /// <summary>
    /// 校验样品条码OutDto
    /// </summary>
    public record CheckBarCodeOutDto: BaseEntityDto
    {
        /// <summary>
        /// OQC检验参数组明细快照id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string? ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string? ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DataTypeEnum? ParameterDataType { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string? ReferenceValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }
    }


    /// <summary>
    /// OQC检验单样本新增/更新Dto
    /// </summary>
    public record QualOqcOrderExecSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<QualOqcOrderParameterSaveDto> Details { get; set; }
    }

    /// <summary>
    /// Oqc检验单样品参数
    /// </summary>
    public record QualOqcOrderParameterSaveDto
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
        public IEnumerable<OQCSampleInteAttachmentBaseDto>? Attachment { get; set; }

    }

    /// <summary>
    /// OQC样品附件Dto
    /// </summary>
    public record OQCSampleInteAttachmentBaseDto : BaseEntityDto {
        ///// <summary>
        ///// 文件名称
        ///// </summary>
        //public string? OriginalName { get; set; }

        ///// <summary>
        ///// 文件路径
        ///// </summary>
        //public string? FileUrl { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 完成Dto
    /// </summary>
    public record QualOqcOrderCompleteDto
    {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

    }

    /// <summary>
    /// 修改检验单状态Dto
    /// </summary>
    public record UpdateStatusDto {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }
    }

    public record OQCAnnexOutDto { 
        /// <summary>
        /// 附件Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 附件Path
        /// </summary>
        public string? Path { get; set; }
    }
}
