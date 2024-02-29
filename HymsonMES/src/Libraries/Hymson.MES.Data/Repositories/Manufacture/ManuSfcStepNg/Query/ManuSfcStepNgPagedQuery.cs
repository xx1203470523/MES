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
    /// 步骤表id
    /// </summary>
    public string? BarCodeStepId { get; set; }

    /// <summary>
    /// 步骤表id模糊条件
    /// </summary>
    public string? BarCodeStepIdLike { get; set; }


    /// <summary>
    /// 不合格代码
    /// </summary>
    public string? UnqualifiedCode { get; set; }

    /// <summary>
    /// 不合格代码模糊条件
    /// </summary>
    public string? UnqualifiedCodeLike { get; set; }


    /// <summary>
    /// 0未补料,1已补料
    /// </summary>
    public long? IsReplenish { get; set; }

    /// <summary>
    /// 0未补料,1已补料组
    /// </summary>
    public IEnumerable<long>? IsReplenishs { get; set; }


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
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }
}
