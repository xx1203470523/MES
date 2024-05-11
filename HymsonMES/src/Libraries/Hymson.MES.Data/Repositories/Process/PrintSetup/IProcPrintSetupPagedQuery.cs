/*
 *creator: Karl
 *
 *describe: 上料点表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 转换系数表 分页参数
    /// </summary>
    public class IProcPrintSetupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 描述 :资源ID
        /// 空值 : false  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述 :打印机配置ID
        /// 空值 : false  
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :仓库标签模板ID
        /// 空值 : false  
        /// </summary>
        public long? LabelTemplateId { get; set; }

        /// <summary>
        /// 描述 :资源类型ID
        /// 空值 : false  
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 物料名
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// 空值 : false  
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模板名称
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 打印机名称
        /// 空值 : false  
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
