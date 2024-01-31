using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query
{
    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSfcCirculationQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum[]? CirculationTypes { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 站点Id组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }

        /// <summary>
        /// 当前工序id组
        /// </summary>
        public IEnumerable<long>? ProcedureIds { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资源id组
        /// </summary>
        public IEnumerable<long>? ResourceIds { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备id组
        /// </summary>
        public IEnumerable<long>? EquipmentIds { get; set; }

        /// <summary>
        /// 扣料上料点id
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 扣料上料点id组
        /// </summary>
        public IEnumerable<long>? FeedingPointIds { get; set; }

        /// <summary>
        /// 流转前条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 流转前条码模糊条件
        /// </summary>
        public string? SFCLike { get; set; }

        /// <summary>
        /// 流转前工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 流转前工单id组
        /// </summary>
        public IEnumerable<long>? WorkOrderIds { get; set; }

        /// <summary>
        /// 流转前产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 流转前产品id组
        /// </summary>
        public IEnumerable<long>? ProductIds { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 位置号模糊条件
        /// </summary>
        public string? LocationLike { get; set; }

        /// <summary>
        /// 流转后条码信息
        /// </summary>
        public string? CirculationBarCode { get; set; }

        /// <summary>
        /// 流转后条码信息模糊条件
        /// </summary>
        public string? CirculationBarCodeLike { get; set; }

        /// <summary>
        /// 流转后工单id
        /// </summary>
        public long? CirculationWorkOrderId { get; set; }

        /// <summary>
        /// 流转后工单id组
        /// </summary>
        public IEnumerable<long>? CirculationWorkOrderIds { get; set; }

        /// <summary>
        /// 流转后产品id
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 流转后产品id组
        /// </summary>
        public IEnumerable<long>? CirculationProductIds { get; set; }

        /// <summary>
        /// 流转后主物料id组
        /// </summary>
        public IEnumerable<long>? CirculationMainProductIds { get; set; }

        /// <summary>
        /// 供应商id
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }

        /// <summary>
        /// 供应商id组
        /// </summary>
        public IEnumerable<long>? CirculationMainSupplierIds { get; set; }

        /// <summary>
        /// 流转条码数量最小值
        /// </summary>
        public decimal? CirculationQtyMin { get; set; }

        /// <summary>
        /// 流转条码数量最大值
        /// </summary>
        public decimal? CirculationQtyMax { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换
        /// </summary>
        public SfcCirculationTypeEnum? CirculationType { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)组
        /// </summary>
        public IEnumerable<TrueOrFalseEnum>? IsDisassembles { get; set; }

        /// <summary>
        /// 拆解人
        /// </summary>
        public string? DisassembledBy { get; set; }

        /// <summary>
        /// 拆解人模糊条件
        /// </summary>
        public string? DisassembledByLike { get; set; }

        /// <summary>
        /// 拆解时间开始日期
        /// </summary>
        public DateTime? DisassembledOnStart { get; set; }

        /// <summary>
        /// 拆解时间结束日期
        /// </summary>
        public DateTime? DisassembledOnEnd { get; set; }

        /// <summary>
        /// 换件id manu_sfc_circulation id
        /// </summary>
        public long? SubstituteId { get; set; }

        /// <summary>
        /// 换件id manu_sfc_circulation id组
        /// </summary>
        public IEnumerable<long>? SubstituteIds { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }

        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }

        /// <summary>
        /// 更新时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 更新时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }

        /// <summary>
        /// 记录绑定批次条码名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 记录绑定批次条码名称模糊条件
        /// </summary>
        public string? NameLike { get; set; }

        /// <summary>
        /// 绑定时条码的型号LA要求添加
        /// </summary>
        public string? ModelCode { get; set; }

        /// <summary>
        /// 绑定时条码的型号LA要求添加模糊条件
        /// </summary>
        public string? ModelCodeLike { get; set; }
    }

    /// <summary>
    /// 条码流转表 分页参数
    /// </summary>
    public class ManuSfcCirculationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? CirculationBarCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? CirculationBarCodes { get; set; }

        /// <summary>
        /// 条码流转类型
        /// </summary>
        public SfcCirculationTypeEnum? CirculationType { get; set; }

        /// <summary>
        /// 条码流转类型
        /// </summary>
        public IEnumerable<SfcCirculationTypeEnum>? CirculationTypes { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        new public string Sorting { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 站点Id组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }

        /// <summary>
        /// 当前工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 当前工序id组
        /// </summary>
        public IEnumerable<long>? ProcedureIds { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资源id组
        /// </summary>
        public IEnumerable<long>? ResourceIds { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备id组
        /// </summary>
        public IEnumerable<long>? EquipmentIds { get; set; }

        /// <summary>
        /// 扣料上料点id
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 扣料上料点id组
        /// </summary>
        public IEnumerable<long>? FeedingPointIds { get; set; }

        /// <summary>
        /// 流转前条码模糊条件
        /// </summary>
        public string? SFCLike { get; set; }

        /// <summary>
        /// 流转前工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 流转前工单id组
        /// </summary>
        public IEnumerable<long>? WorkOrderIds { get; set; }

        /// <summary>
        /// 流转前产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 流转前产品id组
        /// </summary>
        public IEnumerable<long>? ProductIds { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 位置号模糊条件
        /// </summary>
        public string? LocationLike { get; set; }

        /// <summary>
        /// 流转后条码信息模糊条件
        /// </summary>
        public string? CirculationBarCodeLike { get; set; }

        /// <summary>
        /// 流转后工单id
        /// </summary>
        public long? CirculationWorkOrderId { get; set; }

        /// <summary>
        /// 流转后工单id组
        /// </summary>
        public IEnumerable<long>? CirculationWorkOrderIds { get; set; }

        /// <summary>
        /// 流转后产品id
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 流转后产品id组
        /// </summary>
        public IEnumerable<long>? CirculationProductIds { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 流转后主物料id组
        /// </summary>
        public IEnumerable<long>? CirculationMainProductIds { get; set; }

        /// <summary>
        /// 供应商id
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }

        /// <summary>
        /// 供应商id组
        /// </summary>
        public IEnumerable<long>? CirculationMainSupplierIds { get; set; }

        /// <summary>
        /// 流转条码数量最小值
        /// </summary>
        public decimal? CirculationQtyMin { get; set; }

        /// <summary>
        /// 流转条码数量最大值
        /// </summary>
        public decimal? CirculationQtyMax { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换模糊条件
        /// </summary>
        public string? CirculationTypeLike { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)组
        /// </summary>
        public IEnumerable<TrueOrFalseEnum>? IsDisassembles { get; set; }

        /// <summary>
        /// 拆解人
        /// </summary>
        public string? DisassembledBy { get; set; }

        /// <summary>
        /// 拆解人模糊条件
        /// </summary>
        public string? DisassembledByLike { get; set; }

        /// <summary>
        /// 拆解时间开始日期
        /// </summary>
        public DateTime? DisassembledOnStart { get; set; }

        /// <summary>
        /// 拆解时间结束日期
        /// </summary>
        public DateTime? DisassembledOnEnd { get; set; }

        /// <summary>
        /// 换件id manu_sfc_circulation id
        /// </summary>
        public long? SubstituteId { get; set; }

        /// <summary>
        /// 换件id manu_sfc_circulation id组
        /// </summary>
        public IEnumerable<long>? SubstituteIds { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }

        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }

        /// <summary>
        /// 更新时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 更新时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }

        /// <summary>
        /// 记录绑定批次条码名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 记录绑定批次条码名称模糊条件
        /// </summary>
        public string? NameLike { get; set; }

        /// <summary>
        /// 绑定时条码的型号LA要求添加
        /// </summary>
        public string? ModelCode { get; set; }

        /// <summary>
        /// 绑定时条码的型号LA要求添加模糊条件
        /// </summary>
        public string? ModelCodeLike { get; set; }
    }

    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSfcCirculationBySfcsQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public IEnumerable<string> Sfc { get; set; }

        /// <summary>
        /// 转换后条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum[] CirculationTypes { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 流转后主物料id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }

    /// <summary>
    /// 组件使用报告 分页参数
    /// </summary>
    public class ComUsageReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 组件物料编码ID
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string? CirculationBarCode { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }

    }

    /// <summary>
    /// 追溯报表查询
    /// </summary>
    public class ProductTraceReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// true 正向，false 反向
        /// 默认正向追溯
        /// </summary>
        public bool TraceDirection { get; set; } = true;
    }
}
