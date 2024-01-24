/*
 *creator: Karl
 *
 *describe: 生产汇总表 分页查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 生产汇总表 分页参数
/// </summary>
public class ManuSfcSummaryPagedQuery : PagerInfo
{
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
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点ID组
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
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Id组
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }


    /// <summary>
    /// 工单id
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 工单id组
    /// </summary>
    public IEnumerable<long>? WorkOrderIds { get; set; }


    /// <summary>
    /// 产品id
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// 产品id组
    /// </summary>
    public IEnumerable<long>? ProductIds { get; set; }
}
