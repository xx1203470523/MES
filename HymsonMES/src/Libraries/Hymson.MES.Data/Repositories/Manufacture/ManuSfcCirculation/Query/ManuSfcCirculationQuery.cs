/*
 *creator: Karl
 *
 *describe: 条码流转表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:50:00
 */

using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query
{
    /// <summary>
    /// 条码流转表 查询参数
    /// </summary>
    public class ManuSfcCirculationQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum[] CirculationTypes { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }
    }
}
