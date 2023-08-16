/*
 *creator: Karl
 *
 *describe: 资源设备绑定表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源设备绑定表 查询参数
    /// </summary>
    public class ProcResourceEquipmentBindQuery
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// id集合
        /// </summary>
        public long[] Ids { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        public bool IsMain { get; set; } = false;

        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }
    }
}
