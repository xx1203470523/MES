using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    public record ProcProductSetDto : BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 资源或者工序id
        /// </summary>
        public long SetPointId { get; set; }

        /// <summary>
        /// 半成品id
        /// </summary>
        public long SemiProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 半产品编码
        /// </summary>
        public string SemiMaterialCode { get; set; }

        /// <summary>
        /// 半产品名称
        /// </summary>
        public string SemiMaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string SemiVersion { get; set; }
    }

    /// <summary>
    /// 产出设置配置表新增Dto
    /// </summary>
    public record ProcProductSetCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 半成品id
        /// </summary>
        public long SemiProductId { get; set; }
    }

    /// <summary>
    /// 产出配置配置表
    /// </summary>
    public class ProcProductSetQueryDto : PagerInfo
    {
        /// <summary>
        /// 工序和资源半成品产品设置表
        /// </summary>
        public long SetPointId { get; set; }
    }
}
