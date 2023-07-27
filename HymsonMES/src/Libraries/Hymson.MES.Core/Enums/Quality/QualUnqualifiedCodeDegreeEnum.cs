using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.QualUnqualifiedCode
{
    public enum QualUnqualifiedCodeDegreeEnum : short
    {
        [Description("高")]
        High =0,
        [Description("中")]
        Centre=1,
        [Description("低")]
        Low =2
    }
}
