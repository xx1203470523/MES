using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 产品不良录入Dto
    /// </summary>
    public record ManuProductBadRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public long? FoundBadOperationId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutflowOperationId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long? UnqualifiedId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 不良来源;·1、设备复投不良  2、人工录入不良
        /// </summary>
        public bool? Source { get; set; }

        /// <summary>
        /// 说明
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
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }
    }

    /// <summary>
    /// 产品不良录入新增Dto
    /// </summary>
    public record ManuProductBadRecordCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public long FoundBadOperationId { get; set; }

        /// <summary>
        /// 发现不良资源
        /// </summary>
        public string? FoundBadResourceId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long OutflowOperationId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long[] UnqualifiedIds { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 不合格工艺路线id
        /// </summary>
        public long? BadProcessRouteId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 面板的产品不良录入新增Dto
    /// </summary>
    public record FacePlateManuProductBadRecordCreateDto: ManuProductBadRecordCreateDto
    {
        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum BarcodeType { get; set; } = ManuFacePlateBarcodeTypeEnum.Product;
    }

    /// <summary>
    /// 产品不良录入更新Dto
    /// </summary>
    public record ManuProductBadRecordModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 发现不良工序Id
        /// </summary>
        public long? FoundBadOperationId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutflowOperationId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long? UnqualifiedId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 不良来源;·1、设备复投不良  2、人工录入不良
        /// </summary>
        public bool? Source { get; set; }

        /// <summary>
        /// 说明
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
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }
    }

    /// <summary>
    /// 产品不良录入分页Dto
    /// </summary>
    public class ManuProductBadRecordPagedQueryDto : PagerInfo
    {
        
    }

    public class ManuProductBadRecordQueryDto
    {
        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 不合格代码类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }
    }

    /// <summary>
    /// 取消标识
    /// </summary>
    public class CancelSfcIdentificationDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 不合格代码信息
        /// </summary>
        public List<UnqualifiedList> UnqualifiedLists { get; set; }
    }

    public class UnqualifiedList
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }
        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 不良复判
    /// </summary>
    public class BadReJudgmentDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 不合格工艺路线id
        /// </summary>
        public long? BadProcessRouteId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 不合格代码信息
        /// </summary>
        public List<UnqualifiedDefectList> UnqualifiedLists { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UnqualifiedDefectList
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManuProductBadRecordViewDto
    {
        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long? UnqualifiedId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 不合格工艺路线Id
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}
