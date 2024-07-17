/*
 *creator: Karl
 *
 *describe: 编码规则    Dto | 代码由框架生成
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
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 编码规则Dto
    /// </summary>
    public record InteCodeRulesDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }

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
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人;创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间;创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人;更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间;更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }


    /// <summary>
    /// 编码规则新增Dto
    /// </summary>
    public record InteCodeRulesCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public List<long>? ProductIds { get; set; }

        /// <summary>
        /// 容器id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum? CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum? CodeMode { get; set; }

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public CodeRulePackTypeEnum? PackType { get; set; }

        /// <summary>
        /// 基数;10 16 32
        /// </summary>
        public int? Base { get; set; }

        /// <summary>
        /// 忽略字符
        /// </summary>
        public string? IgnoreChar { get; set; }

        /// <summary>
        /// 增量
        /// </summary>
        public int? Increment { get; set; }

        /// <summary>
        /// 序列长度;0:表示无限长度
        /// </summary>
        public int? OrderLength { get; set; }

        /// <summary>
        /// 重置序号;1：从不；2：每天；3：每周；4：每月；5：每年；
        /// </summary>
        public SerialNumberTypeEnum ResetType { get; set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public int? StartNumber { get; set; }

        /// <summary>
        /// 手动录入编码规则的描述信息
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 编码规则
        /// </summary>
        public List<InteCodeRulesMakeCreateDto>? CodeRulesMakes { get;set;}

    }

    /// <summary>
    /// 编码规则更新Dto
    /// </summary>
    public record InteCodeRulesModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 容器id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public List<long>? ProductIds { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }

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
        public string? IgnoreChar { get; set; }

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
        public string? Remark { get; set; } = "";


        /// <summary>
        /// 编码规则
        /// </summary>
        public List<InteCodeRulesMakeCreateDto> CodeRulesMakes { get; set; }
    }

    /// <summary>
    /// 编码规则分页Dto
    /// </summary>
    public class InteCodeRulesPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum? CodeType { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 容器名称
        /// </summary>
        public string? ContainerName { get; set; }

    }

    public record class InteCodeRulesPageViewDto : BaseEntityDto
    {
        /// <summary>
        /// 前端渲染唯一key（查询出的数据编码规则Id不是唯一的）
        /// </summary>
        public long RowKey {  get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }

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
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人;创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间;创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人;更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间;更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode {  get; set; }

        /// <summary>
        /// 容器名称
        /// </summary>
        public string ContainerName { get; set; }
    }

    public record class InteCodeRulesDetailViewDto : BaseEntityDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 容器id
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }

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
        public string Remark { get; set; } = "";


        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 编码规则物料Ids
        /// </summary>
        public IEnumerable<long> ProductIds { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public IEnumerable<string> MaterialCodes { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCodesJoinStr {  get; set; }


        /// <summary>
        /// 编码规则
        /// </summary>
        public List<InteCodeRulesMakeDto> CodeRulesMakes { get; set; }
    } 
}
