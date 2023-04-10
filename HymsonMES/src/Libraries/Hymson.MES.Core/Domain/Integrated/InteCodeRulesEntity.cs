/*
 *creator: Karl
 *
 *describe: 编码规则    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.Sequences.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 编码规则，数据实体对象   
    /// inte_code_rules
    /// @author Karl
    /// @date 2023-03-17 05:02:26
    /// </summary>
    public class InteCodeRulesEntity : BaseEntity
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum  CodeType { get; set; }

       /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public CodeRulePackTypeEnum? PackType { get; set; }

       /// <summary>
        /// 基数;10 16 32
        /// </summary>
        public int Base { get; set; }

       /// <summary>
        /// 忽略字符
        /// </summary>
        public string IgnoreChar { get; set; }

       /// <summary>
        /// 增量
        /// </summary>
        public int Increment { get; set; }

       /// <summary>
        /// 序列长度;0:表示无限长度
        /// </summary>
        public int OrderLength { get; set; }

       /// <summary>
        /// 重置序号;1：从不；2：每天；3：每周；4：每月；5：每年；
        /// </summary>
        public SerialNumberTypeEnum ResetType { get; set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 手动录入编码规则的描述信息
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}