
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：IQC检验项目详情;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemDetailQuery : QueryAbstraction
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
    /// IQC检验项目Id
    /// </summary>
    public long? QualIqcInspectionItemId { get; set; }

    /// <summary>
    /// IQC检验项目Id组
    /// </summary>
    public IEnumerable<long>? QualIqcInspectionItemIds { get; set; }


    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long? ParameterId { get; set; }

    /// <summary>
    /// 参数Id proc_parameter 的 id组
    /// </summary>
    public IEnumerable<long>? ParameterIds { get; set; }


    /// <summary>
    /// 项目类型;1、计量2、计数
    /// </summary>
    public IQCProjectTypeEnum? Type { get; set; }


    /// <summary>
    /// 检验器具;二次元影像仪；卡尺；薄膜测厚仪；千分尺；钢板尺；氦检仪器；电子称；电子天平；保圧气密设备；绝缘耐压测试仪；烘箱；放大镜；达因笔；拉力测试仪；高度卡尺；万用表；百分表；直流微电阻测试仪；高精度数字显微镜；卡尔费休滴定仪；透气度测试仪；测试；其他；
    /// </summary>
    public string? Utensil { get; set; }

    /// <summary>
    /// 检验器具;二次元影像仪；卡尺；薄膜测厚仪；千分尺；钢板尺；氦检仪器；电子称；电子天平；保圧气密设备；绝缘耐压测试仪；烘箱；放大镜；达因笔；拉力测试仪；高度卡尺；万用表；百分表；直流微电阻测试仪；高精度数字显微镜；卡尔费休滴定仪；透气度测试仪；测试；其他；模糊条件
    /// </summary>
    public string? UtensilLike { get; set; }


    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }

    /// <summary>
    /// 小数位数组
    /// </summary>
    public IEnumerable<int>? Scales { get; set; }


    /// <summary>
    /// 规格下限最小值
    /// </summary>
    public decimal? LowerLimitMin { get; set; }

    /// <summary>
    /// 规格下限最大值
    /// </summary>
    public decimal? LowerLimitMax { get; set; }


    /// <summary>
    /// 规格中心最小值
    /// </summary>
    public decimal? CenterMin { get; set; }

    /// <summary>
    /// 规格中心最大值
    /// </summary>
    public decimal? CenterMax { get; set; }


    /// <summary>
    /// 规格上限最小值
    /// </summary>
    public decimal? UpperLimitMin { get; set; }

    /// <summary>
    /// 规格上限最大值
    /// </summary>
    public decimal? UpperLimitMax { get; set; }


    /// <summary>
    /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
    /// </summary>
    public string? InspectionType { get; set; }

    /// <summary>
    /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验模糊条件
    /// </summary>
    public string? InspectionTypeLike { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 备注模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点ID组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：IQC检验项目详情;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemDetailPagedQuery : PagerInfo
{
    /// <summary>
    /// 排序
    /// </summary>
    new public string Sorting { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 主键组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// IQC检验项目Id
    /// </summary>
    public long? QualIqcInspectionItemId { get; set; }

    /// <summary>
    /// IQC检验项目Id组
    /// </summary>
    public IEnumerable<long>? QualIqcInspectionItemIds { get; set; }


    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long? ParameterId { get; set; }

    /// <summary>
    /// 参数Id proc_parameter 的 id组
    /// </summary>
    public IEnumerable<long>? ParameterIds { get; set; }


    /// <summary>
    /// 项目类型;1、计量2、计数
    /// </summary>
    public IQCProjectTypeEnum? Type { get; set; }


    /// <summary>
    /// 检验器具;二次元影像仪；卡尺；薄膜测厚仪；千分尺；钢板尺；氦检仪器；电子称；电子天平；保圧气密设备；绝缘耐压测试仪；烘箱；放大镜；达因笔；拉力测试仪；高度卡尺；万用表；百分表；直流微电阻测试仪；高精度数字显微镜；卡尔费休滴定仪；透气度测试仪；测试；其他；
    /// </summary>
    public string? Utensil { get; set; }

    /// <summary>
    /// 检验器具;二次元影像仪；卡尺；薄膜测厚仪；千分尺；钢板尺；氦检仪器；电子称；电子天平；保圧气密设备；绝缘耐压测试仪；烘箱；放大镜；达因笔；拉力测试仪；高度卡尺；万用表；百分表；直流微电阻测试仪；高精度数字显微镜；卡尔费休滴定仪；透气度测试仪；测试；其他；模糊条件
    /// </summary>
    public string? UtensilLike { get; set; }


    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }

    /// <summary>
    /// 小数位数组
    /// </summary>
    public IEnumerable<int>? Scales { get; set; }


    /// <summary>
    /// 规格下限最小值
    /// </summary>
    public decimal? LowerLimitMin { get; set; }

    /// <summary>
    /// 规格下限最大值
    /// </summary>
    public decimal? LowerLimitMax { get; set; }


    /// <summary>
    /// 规格中心最小值
    /// </summary>
    public decimal? CenterMin { get; set; }

    /// <summary>
    /// 规格中心最大值
    /// </summary>
    public decimal? CenterMax { get; set; }


    /// <summary>
    /// 规格上限最小值
    /// </summary>
    public decimal? UpperLimitMin { get; set; }

    /// <summary>
    /// 规格上限最大值
    /// </summary>
    public decimal? UpperLimitMax { get; set; }


    /// <summary>
    /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
    /// </summary>
    public string? InspectionType { get; set; }

    /// <summary>
    /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验模糊条件
    /// </summary>
    public string? InspectionTypeLike { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 备注模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点ID组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

}