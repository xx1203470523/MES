/*
 *creator: Karl
 *
 *describe: 条码信息表 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */

using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query
{
    /// <summary>
    /// 条码信息表 查询参数
    /// </summary>
    public class ManuSfcInfoQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }
    }
}
