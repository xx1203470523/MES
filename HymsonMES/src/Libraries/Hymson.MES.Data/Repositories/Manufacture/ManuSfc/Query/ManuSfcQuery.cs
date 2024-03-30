using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：条码表;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-24</para>
/// </summary>
public class ManuSfcQuery : QueryAbstraction
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
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

    /// <summary>   
    /// 条码类型 1、生产条码 2、非生产条码
    /// </summary>
    public SfcTypeEnum? Type { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 条码组
    /// </summary>
    public IEnumerable<string>? SFCs { get; set; }

    /// <summary>
    /// 条码模糊条件
    /// </summary>
    public string? SFCLike { get; set; }


    /// <summary>
    /// 数量最小值
    /// </summary>
    public decimal? QtyMin { get; set; }

    /// <summary>
    /// 数量最大值
    /// </summary>
    public decimal? QtyMax { get; set; }


    /// <summary>
    /// 状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
    /// </summary>
    public SfcStatusEnum? Status { get; set; }


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
    /// 
    /// </summary>
    public YesOrNoEnum? IsUsed { get; set; }


    /// <summary>
    /// 报废表Id
    /// </summary>
    public long? SfcScrapId { get; set; }

    /// <summary>
    /// 报废表Id组
    /// </summary>
    public IEnumerable<long>? SfcScrapIds { get; set; }


    /// <summary>
    /// 状态备份
    /// </summary>
    public SfcStatusEnum? StatusBack { get; set; }

}