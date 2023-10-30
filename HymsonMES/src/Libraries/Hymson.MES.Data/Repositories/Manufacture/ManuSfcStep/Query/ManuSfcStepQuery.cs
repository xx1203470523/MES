/*
 *creator: Karl
 *
 *describe: 条码步骤表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-22 05:17:57
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表 查询参数
    /// </summary>
    public class ManuSfcStepQuery
    {
    }

    public class SfcInOutStepQuery
    {
        public long SiteId { get; set; }

        public string Sfc { get; set; }

    }

    public class SfcInStepQuery
    {
        public long SiteId { get; set; }

        public string[] Sfcs { get; set; }

    }
}
