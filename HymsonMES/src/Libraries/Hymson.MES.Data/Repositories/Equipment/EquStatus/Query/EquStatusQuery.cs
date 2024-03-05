
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：设备状态;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-5</para>
/// </summary>
public class EquStatusQuery : QueryAbstraction
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
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Id组
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }


    /// <summary>
    /// 状态;0.自动运行、1.手动运行、2.停机、3.故障、4.离线
    /// </summary>
    public long? EquipmentStatus { get; set; }

    /// <summary>
    /// 状态;0.自动运行、1.手动运行、2.停机、3.故障、4.离线组
    /// </summary>
    public IEnumerable<long>? EquipmentStatuss { get; set; }


    /// <summary>
    /// 停机原因
    /// </summary>
    public string? LossRemark { get; set; }

    /// <summary>
    /// 停机原因模糊条件
    /// </summary>
    public string? LossRemarkLike { get; set; }


    /// <summary>
    /// 设备停机开始时间开始日期
    /// </summary>
    public DateTime? BeginTimeStart { get; set; }

    /// <summary>
    /// 设备停机开始时间结束日期
    /// </summary>
    public DateTime? BeginTimeEnd { get; set; }


    /// <summary>
    /// 设备停机开始时间开始日期
    /// </summary>
    public DateTime? EndTimeStart { get; set; }

    /// <summary>
    /// 设备停机开始时间结束日期
    /// </summary>
    public DateTime? EndTimeEnd { get; set; }


    /// <summary>
    /// 传输时间开始日期
    /// </summary>
    public DateTime? LocalTimeStart { get; set; }

    /// <summary>
    /// 传输时间结束日期
    /// </summary>
    public DateTime? LocalTimeEnd { get; set; }


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

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：设备状态;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-5</para>
/// </summary>
public class EquStatusPagedQuery : PagerInfo
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
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Id组
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }


    /// <summary>
    /// 状态;0.自动运行、1.手动运行、2.停机、3.故障、4.离线
    /// </summary>
    public long? EquipmentStatus { get; set; }

    /// <summary>
    /// 状态;0.自动运行、1.手动运行、2.停机、3.故障、4.离线组
    /// </summary>
    public IEnumerable<long>? EquipmentStatuss { get; set; }


    /// <summary>
    /// 停机原因
    /// </summary>
    public string? LossRemark { get; set; }

    /// <summary>
    /// 停机原因模糊条件
    /// </summary>
    public string? LossRemarkLike { get; set; }


    /// <summary>
    /// 设备停机开始时间开始日期
    /// </summary>
    public DateTime? BeginTimeStart { get; set; }

    /// <summary>
    /// 设备停机开始时间结束日期
    /// </summary>
    public DateTime? BeginTimeEnd { get; set; }


    /// <summary>
    /// 设备停机开始时间开始日期
    /// </summary>
    public DateTime? EndTimeStart { get; set; }

    /// <summary>
    /// 设备停机开始时间结束日期
    /// </summary>
    public DateTime? EndTimeEnd { get; set; }


    /// <summary>
    /// 传输时间开始日期
    /// </summary>
    public DateTime? LocalTimeStart { get; set; }

    /// <summary>
    /// 传输时间结束日期
    /// </summary>
    public DateTime? LocalTimeEnd { get; set; }


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

}