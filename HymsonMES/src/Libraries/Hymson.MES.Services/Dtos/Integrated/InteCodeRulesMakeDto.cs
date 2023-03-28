/*
 *creator: Karl
 *
 *describe: 编码规则组成    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 编码规则组成Dto
    /// </summary>
    public record InteCodeRulesMakeDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public long CodeRulesId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 取值方式;1：固定值；2：可变值；
        /// </summary>
        public CodeValueTakingTypeEnum ValueTakingType { get; set; }

       /// <summary>
        /// 分段值
        /// </summary>
        public string SegmentedValue { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }


    /// <summary>
    /// 编码规则组成新增Dto
    /// </summary>
    public record InteCodeRulesMakeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public long? CodeRulesId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 取值方式;1：固定值；2：可变值；
        /// </summary>
        public CodeValueTakingTypeEnum ValueTakingType { get; set; }

       /// <summary>
        /// 分段值
        /// </summary>
        public string SegmentedValue { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 编码规则组成更新Dto
    /// </summary>
    public record InteCodeRulesMakeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public long CodeRulesId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 取值方式;1：固定值；2：可变值；
        /// </summary>
        public CodeValueTakingTypeEnum ValueTakingType { get; set; }

       /// <summary>
        /// 分段值
        /// </summary>
        public string SegmentedValue { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       
    }

    /// <summary>
    /// 编码规则组成分页Dto
    /// </summary>
    public class InteCodeRulesMakePagedQueryDto : PagerInfo
    {
    }
}
