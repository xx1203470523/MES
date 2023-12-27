//using Hymson.Infrastructure;
//using System.Text;
//using Hymson.MES.Core.Enums.Integrated;

//namespace Hymson.MES.Services.Dtos.Integrated
//{
//    public record InteContainerFreightDto: BaseEntityDto
//    {
//        /// <summary>
//        /// 状态1、物料 2、物料组 3、容器
//        /// </summary>
//        public ContainerFreightTypeEnum Type { get; set; }

//        /// <summary>   
//        /// 物料id
//        /// </summary>
//        public long? MaterialId { get; set; }


//        /// <summary>   
//        /// 包装等级值
//        /// </summary>
//        public string LevelValue { get;set; }

//        /// <summary>   
//        /// 物料Code
//        /// </summary>
//        public long? MaterialCode { get; set; }

//        /// <summary>   
//        /// 版本
//        /// </summary>
//        public string? Version { get; set; }

//        /// <summary>
//        /// 物料组Id
//        /// </summary>
//        public long? MaterialGroupId { get; set; }

//        /// <summary>
//        /// 装载容器Id
//        /// </summary>
//        public long? FreightContainerId { get; set; }

//        /// <summary>
//        /// 最大用量
//        /// </summary>
//        public decimal? Maximum { get; set; }

//        /// <summary>
//        /// 最小用量
//        /// </summary>
//        public decimal? Minimum { get; set; }

//        /// <summary>
//        /// 备注
//        /// </summary>
//        public string? Remark { get; set; } = "";
//    }

//}


using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Inte;

/// <summary>
/// <para>@描述：容器装载维护; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerFreightDto : BaseEntityDto
{
    /// <summary>
    /// 容器 id  inte_container_info Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 类型 1、物料 2、物料组 3、容器
    /// </summary>
    public ContainerFreightTypeEnum? Type { get; set; }

    /// <summary>
    /// 物料id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料组Id
    /// </summary>
    public long? MaterialGroupId { get; set; }

    /// <summary>
    /// 装载容器 id  inte_container Id
    /// </summary>
    public long? FreightContainerId { get; set; }

    /// <summary>
    /// 最小用量
    /// </summary>
    public decimal? Minimum { get; set; }

    /// <summary>
    /// 最大用量
    /// </summary>
    public decimal? Maximum { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 包装等级值
    /// </summary>
    public string? LevelValue { get; set; }

}

/// <summary>
/// <para>@描述：容器装载维护; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerFreightDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerFreightUpdateDto : InteContainerFreightDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}

/// <summary>
/// <para>@描述：容器装载维护; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerFreightUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerFreightOutputDto : InteContainerFreightUpdateDto
{
    /// <summary>
    /// 创建人;创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建时间;创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 更新人;更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间;更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 物料版本
    /// </summary>
    public string Version {  get; set; }

}

/// <summary>
/// <para>@描述：容器装载维护; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// </summary>
public record InteContainerFreightDeleteDto : InteContainerFreightDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：容器装载维护; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerFreightPagedQueryDto : PagerInfo
{
}

/// <summary>
/// <para>@描述：容器装载维护; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerFreightQueryDto : QueryDtoAbstraction
{
}