using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 附件保存
    /// </summary>
    public record QualOqcOrderSaveAttachmentDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// OQC检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    /// <summary>
    /// 获取OQC检验单已经检验样品数据
    /// </summary>
    public class OqcOrderParameterDetailPagedQueryDto : PagerInfo {

        /// <summary>
        /// OQCOrderId
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 项目编号(参数编码)
        /// </summary>
        public string? ParameterCode { get; set;}
    }

    /// <summary>
    /// OQC检验单已经检验样品数据Dto
    /// </summary>
    public record OqcOrderParameterDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 已检验样本Id
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
        /// 规格下限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachment { get; set; }

    }

    /// <summary>
    /// 已检验数据修改样品明细Dto
    /// </summary>
    public record UpdateSampleDetailDto { 
        /// <summary>
        /// 样品明细Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto>? Attachment { get; set; }
    }

    /// <summary>
    /// OQC不合格处理Dto
    /// </summary>
    public record OQCOrderUnqualifiedHandleDto {
        /// <summary>
        /// OQCOrderId
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// 不合格处理方式;1-让步；2-挑选；3-返工；4-报废；
        /// </summary>
        public OQCHandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
