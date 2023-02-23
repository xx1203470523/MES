/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
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
    /// 标准参数关联类型表 分页参数
    /// </summary>
    public class ProcParameterLinkTypePagedQuery : PagerInfo
    {
        ///// <summary>
        ///// 描述 :所属站点代码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 类型（设备/产品参数） （1:设备参数;2:产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 编码（设备/产品参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（设备/产品参数）
        /// </summary>
        public string ParameterName { get; set; }
    }

    /// <summary>
    /// 查询对象（设备/产品参数） 分页
    /// </summary>
    public class ProcParameterDetailPagerQuery : PagerInfo
    {
        ///// <summary>
        ///// 描述 :所属站点代码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 操作类型 1:add；2:edit；3:view；
        /// </summary>
        public string OperateType { get; set; } = "add";

        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public int ParameterType { get; set; } = 1;

        /// <summary>
        /// 编码（设备/产品参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（设备/产品参数）
        /// </summary>
        public string ParameterName { get; set; }
    }
}
