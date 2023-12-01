using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture;

/// <summary>
/// 条码绑定关系返回数据
/// </summary>
public class ManuSfcCirculationOutputDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 主条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? BindSFC { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }
}
