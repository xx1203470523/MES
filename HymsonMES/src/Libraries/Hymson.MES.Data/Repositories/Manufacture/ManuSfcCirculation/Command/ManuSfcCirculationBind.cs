using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture;

public class ManuSfcCirculationBind
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

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
