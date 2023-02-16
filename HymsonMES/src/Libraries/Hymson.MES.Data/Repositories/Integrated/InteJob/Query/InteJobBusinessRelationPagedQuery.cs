/*
 *creator: Karl
 *
 *describe: job业务配置配置表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// job业务配置配置表 分页参数
    /// </summary>
    public class InteJobBusinessRelationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码
        /// </summary>
        public string? SiteCode { get; set; }

        /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public string? BusinessType { get; set; }

        /// <summary>
        /// 关联的业务表的ID
        /// </summary>
        public long BusinessId { get; set; }
    }
}
