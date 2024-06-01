using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 计量单位Dto
    /// </summary>
    public record InteUnitDto : BaseEntityDto
    {
        /// <summary>
        /// 计量单位编码 
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 计量单位名称 
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 计量单位描述 
        /// </summary>
        public string? Description { get; set; }
    }
}
