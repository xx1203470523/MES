/*
 *creator: Karl
 *
 *describe: 供应商 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 供应商 分页参数
    /// </summary>
    public class WhSupplierPagedQuery : PagerInfo
    {
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

    }
}
