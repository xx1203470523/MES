/*
 *creator: Karl
 *
 *describe: 仓库标签模板    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 仓库标签模板，数据实体对象   
    /// proc_label_template
    /// @author wxk
    /// @date 2023-03-09 02:51:26
    /// </summary>
    public class ProcLabelTemplateEntity : BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 标签物理路径
        /// </summary>
        public string? Path { get; set; }

       /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 模板通用类型
        /// </summary>
        public CurrencyTemplateTypeEnum CurrencyTemplateType {  get; set; }
    }
}
