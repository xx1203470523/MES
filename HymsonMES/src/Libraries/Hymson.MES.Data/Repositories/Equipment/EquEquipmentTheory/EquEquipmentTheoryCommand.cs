using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment;

public class EquEquipmentTheoryCommand : BaseCommand
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 理论产出数
    /// </summary>
    public decimal? TheoryOutputQty { get; set; }

    /// <summary>
    /// 理论产出产出数（EA）
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 理论开机时长
    /// </summary>
    public decimal? TheoryOnTime { get; set; }
}

/// <summary>
/// 创建Command
/// </summary>
public class EquEquipmentTheoryCreateCommand : EquEquipmentTheoryCommand
{
}

/// <summary>
/// 创建Command
/// </summary>
public class EquEquipmentTheoryUpdateCommand : EquEquipmentTheoryCommand
{
}