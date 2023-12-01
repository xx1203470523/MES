using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce;

/// <summary>
/// 更新在制状态
/// </summary>
public class UpdateProduceStatusCommand
{
    /// <summary>
    /// SFC
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 更新的工序
    /// </summary>
    public long ProcedureId { get; set; }

    /// <summary>
    /// 更新的状态
    /// </summary>
    public SfcProduceStatusEnum Status { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string UpdatedBy { get; set; }
}