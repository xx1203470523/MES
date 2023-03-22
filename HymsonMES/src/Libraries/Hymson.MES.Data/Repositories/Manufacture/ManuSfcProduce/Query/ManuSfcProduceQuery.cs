/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除） 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 查询参数
    /// </summary>
    public class ManuSfcProduceQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }
    }
}
