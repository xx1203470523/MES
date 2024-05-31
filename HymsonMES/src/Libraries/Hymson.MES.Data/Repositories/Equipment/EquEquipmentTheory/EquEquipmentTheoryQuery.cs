using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentTheory;

public class EquEquipmentTheoryQuery
{
    /// <summary>
    /// 设备编码
    /// </summary>
    public string EquipmentCode { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public IEnumerable<string> EquipmentCodes { get; set; }
}
