using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    public class InteContainerFreightEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 关联容器ID
        /// </summary>
        public long ContainerId { get; set; }

        /// <summary>
        /// 状态1、物料 2、物料组 3、容器
        /// </summary>
        public ContainerFreightTypeEnum Type { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>   
        /// 包装等级值
        /// </summary>
        public string? LevelValue { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料Code
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 最大用量
        /// </summary>
        public decimal? Maximum { get; set; }

        /// <summary>
        /// 最小用量
        /// </summary>
        public decimal? Minimum { get; set; }

        /// <summary>
        /// 物料组Id
        /// </summary>
        public long? MaterialGroupId { get; set; }

        /// <summary>
        /// 装载容器Id
        /// </summary>
        public long? FreightContainerId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";
    }
}
