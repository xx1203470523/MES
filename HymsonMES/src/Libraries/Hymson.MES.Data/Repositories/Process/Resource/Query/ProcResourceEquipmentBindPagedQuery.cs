/*
 *creator: Karl
 *
 *describe: 资源设备绑定表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源设备绑定表 分页参数
    /// </summary>
    public class ProcResourceEquipmentBindPagedQuery : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
