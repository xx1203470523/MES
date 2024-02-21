
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Qual;

/// <summary>
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemDetailEntity : BaseEntity
{

    /// <summary>
    /// IQC检验项目Id
    /// </summary>
    public long QualIqcInspectionItemId { get; set; }


    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long ParameterId { get; set; }


    /// <summary>
    /// 项目类型;1、计量2、计数
    /// </summary>
    public IQCProjectTypeEnum? Type { get; set; }


    /// <summary>
    /// 检验器具;二次元影像仪；卡尺；薄膜测厚仪；千分尺；钢板尺；氦检仪器；电子称；电子天平；保圧气密设备；绝缘耐压测试仪；烘箱；放大镜；达因笔；拉力测试仪；高度卡尺；万用表；百分表；直流微电阻测试仪；高精度数字显微镜；卡尔费休滴定仪；透气度测试仪；测试；其他；
    /// </summary>
    public IQCUtensilTypeEnum? Utensil { get; set; }


    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }


    /// <summary>
    /// 规格下限
    /// </summary>
    public decimal? LowerLimit { get; set; }


    /// <summary>
    /// 规格中心
    /// </summary>
    public decimal? Center { get; set; }


    /// <summary>
    /// 规格上限
    /// </summary>
    public decimal? UpperLimit { get; set; }


    /// <summary>
    /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
    /// </summary>
    public IQCInspectionTypeEnum? InspectionType { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }


    /// <summary>
    /// 站点ID
    /// </summary>
    public long SiteId { get; set; }

}