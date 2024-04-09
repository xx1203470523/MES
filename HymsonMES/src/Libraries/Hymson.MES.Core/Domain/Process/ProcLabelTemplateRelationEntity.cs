/*
 *creator: Karl
 *
 *describe: 标准模板打印配置信息    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-10-09 09:13:47
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 标准模板打印配置信息，数据实体对象   
    /// proc_label_template_relation
    /// @author Karl
    /// @date 2023-10-09 09:13:47
    /// </summary>
    public class ProcLabelTemplateRelationEntity : BaseEntity
    {
        /// <summary>
        /// 标签模板Id
        /// </summary>
        public long LabelTemplateId { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 打印配置信息
        /// </summary>
        public string PrintConfig { get; set; }

       /// <summary>
        /// 打印模板地址
        /// </summary>
        public string? PrintTemplatePath { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string? PrintDataModel { get; set; }
    }
}
