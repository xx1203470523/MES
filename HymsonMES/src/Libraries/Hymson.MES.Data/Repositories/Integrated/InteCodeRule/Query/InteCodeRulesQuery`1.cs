using Hymson.MES.Core.Enums.Integrated;
using Hymson.Sequences.Enums;

namespace Hymson.MES.Data.Repositories.Integrated;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：编码规则;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-25</para>
/// </summary>
public class InteCodeRulesReQuery : QueryAbstraction
{
    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 主键组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// 产品id
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// 产品id组
    /// </summary>
    public IEnumerable<long>? ProductIds { get; set; }


    /// <summary>
    /// 编码类型;1：过程控制序列码；2：包装序列码；
    /// </summary>
    public CodeRuleCodeTypeEnum? CodeType { get; set; }


    /// <summary>
    /// 编码模式   1单个  2多个
    /// </summary>
    public CodeRuleCodeModeEnum? CodeMode { get; set; }


    /// <summary>
    /// 包装等级;1：一级；2：二级；3：三级；
    /// </summary>
    public CodeRulePackTypeEnum? PackType { get; set; }


    /// <summary>
    /// 忽略字符
    /// </summary>
    public string? IgnoreChar { get; set; }

    /// <summary>
    /// 忽略字符模糊条件
    /// </summary>
    public string? IgnoreCharLike { get; set; }


    /// <summary>
    /// 重置序号;1：持续生成；3：按天生成；5：按周生成；7：按月生成；9：按年生成；
    /// </summary>
    public SerialNumberTypeEnum? ResetType { get; set; }


    /// <summary>
    /// 容器id inte_container_Info id
    /// </summary>
    public long? ContainerInfoId { get; set; }

    /// <summary>
    /// 容器id inte_container_Info id组
    /// </summary>
    public IEnumerable<long>? ContainerInfoIds { get; set; }


    /// <summary>
    /// 手动录入编码规则的描述信息
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 手动录入编码规则的描述信息模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

}