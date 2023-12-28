using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    public class InteContainerSpecificationEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 容器ID
        /// </summary>
        public long? ContainerId { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// 最大填充重量
        /// </summary>
        public decimal? MaxFillWeight { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 容器规格描述
        /// </summary>
        public string? Remark { get; set; }
    }

}
