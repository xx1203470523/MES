using Hymson.Infrastructure;

/// <summary>
/// 设备理论值，数据实体对象   
/// equ_status_statistics
/// @author Czhipu
/// @date 2023-05-16 04:51:59
/// </summary>
public class EquEquipmentTheoryEntity : BaseEntity
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备状态
    /// </summary>
    public string EquipmentCode { get; set; }

    /// <summary>
    /// 理论ct(s)
    /// </summary>
    public decimal? TheoryOutputQty { get; set; }

    /// <summary>
    /// 每小时产量
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 理论开机时长
    /// </summary>
    public decimal? TheoryOnTime { get; set; }

}