using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 成品条码产出记录(FQC生成使用)新增/更新Dto
    /// </summary>
    public record QualFinallyOutputRecordSaveDto : BaseEntityDto
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
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public bool CodeType { get; set; }

        /// <summary>
        /// 是否已生成过检验单(0-否 1-是)
        /// </summary>
        public bool IsGenerated { get; set; }

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

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    public record QualFinallyOutputRecordView : BaseEntityDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? MaterialUnit { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? MaterialVersion { get; set; }
        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }
        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 是否同一工单
        /// </summary>
        public TrueOrFalseEnum? IsSameWorkOrder { get; set; }

        /// <summary>
        /// 是否同产线
        /// </summary>
        public TrueOrFalseEnum? IsSameWorkCenter { get; set; }

        /// <summary>
        /// FQC检测单状态
        /// </summary>
        public InspectionStatusEnum? FQCStatus { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 条码状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
        /// </summary>
        public SfcStatusEnum BarcodeStatus { get; set; }


        /// <summary>
        /// FQC检验单号
        /// </summary>
        public string FQCInspectionOrder { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }

        /// <summary>
        /// 是否已生成过检验单(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsGenerated { get; set; }

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

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }
    }

    /// <summary>
    /// 成品条码产出记录(FQC生成使用)Dto
    /// </summary>
    public record QualFinallyOutputRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? MaterialUnit { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? MaterialVersion { get; set; }
        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }
        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 是否同一工单
        /// </summary>
        public TrueOrFalseEnum? IsSameWorkOrder { get; set; }

        /// <summary>
        /// 是否同产线
        /// </summary>
        public TrueOrFalseEnum? IsSameWorkCenter { get; set; }



        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }

        /// <summary>
        /// 是否已生成过检验单(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsGenerated { get; set; }

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

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 成品条码产出记录(FQC生成使用)分页Dto
    /// </summary>
    public class QualFinallyOutputRecordPagedQueryDto : PagerInfo { }


    /// <summary>
    /// FQC检验-生成前条码查询
    /// </summary>
    public class FQCInspectionSFCQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }
    }

}
