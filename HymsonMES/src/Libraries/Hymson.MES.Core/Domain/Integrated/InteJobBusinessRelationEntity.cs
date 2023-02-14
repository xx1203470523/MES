/*
 *creator: Karl
 *
 *describe: job业务配置配置表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// job业务配置配置表，数据实体对象   
    /// inte__job_business_relation
    /// @author zhaoqing
    /// @date 2023-02-14 02:55:48
    /// </summary>
    public class InteJobBusinessRelationEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }

       /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public bool BusinessType { get; set; }

       /// <summary>
        /// 所属不合格代码ID
        /// </summary>
        public long BusinessId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public string OrderNumber { get; set; }

       /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

       /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

       /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
