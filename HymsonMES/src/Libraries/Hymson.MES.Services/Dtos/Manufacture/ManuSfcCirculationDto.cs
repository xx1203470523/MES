/*
 *creator: Karl
 *
 *describe: 条码流转表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-04-11 04:24:50
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture;

/// <summary>
/// 条码流转表Dto
/// </summary>
public record ManuSfcCirculationDto : BaseEntityDto
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
    /// 当前工序id
    /// </summary>
    public long ProcedureId { get; set; }

   /// <summary>
    /// 资源id
    /// </summary>
    public long? ResourceId { get; set; }

   /// <summary>
    /// 设备id
    /// </summary>
    public long? EquipmentId { get; set; }

   /// <summary>
    /// 扣料上料点id
    /// </summary>
    public long? FeedingPointId { get; set; }

   /// <summary>
    /// 流转前条码
    /// </summary>
    public string SFC { get; set; }

   /// <summary>
    /// 流转前工单id
    /// </summary>
    public long WorkOrderId { get; set; }

   /// <summary>
    /// 流转前产品id
    /// </summary>
    public long ProductId { get; set; }

   /// <summary>
    /// 流转后条码信息
    /// </summary>
    public long CirculationBarCode { get; set; }

   /// <summary>
    /// 流转后工单id
    /// </summary>
    public long? CirculationWorkOrderId { get; set; }

   /// <summary>
    /// 流转后产品id
    /// </summary>
    public long CirculationProductId { get; set; }

   /// <summary>
    /// 流转条码数量
    /// </summary>
    public decimal? CirculationQty { get; set; }

   /// <summary>
    /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
    /// </summary>
    public string CirculationType { get; set; }

   /// <summary>
    /// 是否拆解
    /// </summary>
    public bool? IsDisassemble { get; set; }

   /// <summary>
    /// 拆解人
    /// </summary>
    public string DisassembledBy { get; set; }

   /// <summary>
    /// 拆解时间
    /// </summary>
    public DateTime? DisassembledOn { get; set; }

   /// <summary>
    /// 换件id manu_sfc_circulation id
    /// </summary>
    public long? SubstituteId { get; set; }

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

   
}


/// <summary>
/// 条码流转表新增Dto
/// </summary>
public record ManuSfcCirculationCreateDto : BaseEntityDto
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
    /// 当前工序id
    /// </summary>
    public long ProcedureId { get; set; }

   /// <summary>
    /// 资源id
    /// </summary>
    public long? ResourceId { get; set; }

   /// <summary>
    /// 设备id
    /// </summary>
    public long? EquipmentId { get; set; }

   /// <summary>
    /// 扣料上料点id
    /// </summary>
    public long? FeedingPointId { get; set; }

   /// <summary>
    /// 流转前条码
    /// </summary>
    public string SFC { get; set; }

   /// <summary>
    /// 流转前工单id
    /// </summary>
    public long WorkOrderId { get; set; }

   /// <summary>
    /// 流转前产品id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 位置码
    /// </summary>
    public string Location { get; set; }

   /// <summary>
    /// 流转后条码信息
    /// </summary>
    public string CirculationBarCode { get; set; }

   /// <summary>
    /// 流转后工单id
    /// </summary>
    public long? CirculationWorkOrderId { get; set; }

   /// <summary>
    /// 流转后产品id
    /// </summary>
    public long CirculationProductId { get; set; }

   /// <summary>
    /// 流转条码数量
    /// </summary>
    public decimal? CirculationQty { get; set; }

   /// <summary>
    /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
    /// </summary>
    public SfcCirculationTypeEnum CirculationType { get; set; }

   /// <summary>
    /// 是否拆解
    /// </summary>
    public TrueOrFalseEnum? IsDisassemble { get; set; }

   /// <summary>
    /// 拆解人
    /// </summary>
    public string DisassembledBy { get; set; }

   /// <summary>
    /// 拆解时间
    /// </summary>
    public DateTime? DisassembledOn { get; set; }

   /// <summary>
    /// 换件id manu_sfc_circulation id
    /// </summary>
    public long? SubstituteId { get; set; }

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

   
}

/// <summary>
/// 条码流转表更新Dto
/// </summary>
public record ManuSfcCirculationModifyDto : BaseEntityDto
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
    /// 当前工序id
    /// </summary>
    public long ProcedureId { get; set; }

   /// <summary>
    /// 资源id
    /// </summary>
    public long? ResourceId { get; set; }

   /// <summary>
    /// 设备id
    /// </summary>
    public long? EquipmentId { get; set; }

   /// <summary>
    /// 扣料上料点id
    /// </summary>
    public long? FeedingPointId { get; set; }

   /// <summary>
    /// 流转前条码
    /// </summary>
    public string SFC { get; set; }

   /// <summary>
    /// 流转前工单id
    /// </summary>
    public long WorkOrderId { get; set; }

   /// <summary>
    /// 流转前产品id
    /// </summary>
    public long ProductId { get; set; }

   /// <summary>
    /// 流转后条码信息
    /// </summary>
    public string? CirculationBarCode { get; set; }

   /// <summary>
    /// 流转后工单id
    /// </summary>
    public long? CirculationWorkOrderId { get; set; }

   /// <summary>
    /// 流转后产品id
    /// </summary>
    public long CirculationProductId { get; set; }

   /// <summary>
    /// 流转条码数量
    /// </summary>
    public decimal? CirculationQty { get; set; }

   /// <summary>
    /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
    /// </summary>
    public SfcCirculationTypeEnum CirculationType { get; set; }

   /// <summary>
    /// 是否拆解
    /// </summary>
    public bool? IsDisassemble { get; set; }

   /// <summary>
    /// 拆解人
    /// </summary>
    public string DisassembledBy { get; set; }

   /// <summary>
    /// 拆解时间
    /// </summary>
    public DateTime? DisassembledOn { get; set; }

   /// <summary>
    /// 换件id manu_sfc_circulation id
    /// </summary>
    public long? SubstituteId { get; set; }

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

   

}

/// <summary>
/// 条码流转表分页Dto
/// </summary>
public class ManuSfcCirculationPagedQueryDto : PagerInfo
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
}

/// <summary>
/// 条码流转表分页Dto
/// </summary>
public class ManuSfcCirculationQueryDto : PagerInfo
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
}

public class ManuSfcCirculationBindDto
{
    /// <summary>
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 绑定类型
    /// </summary>
    public SfcCirculationTypeEnum? CirculationType { get; set; }

    /// <summary>
    /// 主条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? CirculationBarCode { get; set; }
}