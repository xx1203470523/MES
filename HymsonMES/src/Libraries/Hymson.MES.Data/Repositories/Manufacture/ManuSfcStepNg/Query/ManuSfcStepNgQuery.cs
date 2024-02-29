/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表 查询类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 条码步骤ng信息记录表 查询参数
/// </summary>
public class ManuSfcStepNgQuery
{

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
