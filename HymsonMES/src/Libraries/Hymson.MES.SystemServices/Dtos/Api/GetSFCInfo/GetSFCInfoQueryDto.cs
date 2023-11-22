using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.Api;

/// <summary>
/// 条码信息查询参数
/// </summary>
public class GetSFCInfoQueryDto
{
    /// <summary>
    /// SFCs
    /// </summary>
    public IEnumerable<string>? SFC { get; set; }
}