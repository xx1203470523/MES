using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.SystemApi;

public class GetSFCInfoQuery
{
    /// <summary>
    /// SFCs
    /// </summary>
    public IEnumerable<string>? SFC { get; set; }
}
