/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表 分页查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 条码步骤ng信息记录表 分页参数
/// </summary>
public class ManuSfcStepNgPagedQuery : PagerInfo
{
    /// <summary>
    /// 产品条码
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Ids
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }

    /// <summary>
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 工序Ids
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }

    /// <summary>
    /// 资源Id
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 资源Ids
    /// </summary>
    public IEnumerable<long>? ResourceIds { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截至时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}
