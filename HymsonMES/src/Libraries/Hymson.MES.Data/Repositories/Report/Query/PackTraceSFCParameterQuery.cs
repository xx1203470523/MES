using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;


public record PackTraceSFCParameterQuery 
{
    /// <summary>
    /// 批量查询Pack码
    /// </summary>
    public IEnumerable<long>? SFC { get; set; }
}