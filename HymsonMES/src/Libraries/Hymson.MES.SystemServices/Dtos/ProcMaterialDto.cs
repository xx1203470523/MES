using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 物料维护Dto
    /// </summary>
    public record ProcMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 物料单位代码
        /// </summary>
        public string? UnitCode { get; set; }

        /// <summary>
        /// 物料组编号
        /// </summary>
        public string MaterialGroupCode { get; set; }

        /// <summary>
        /// 属性分类 1:自制，2:采购，3:自制/采购
        /// </summary>
        public MaterialBuyTypeEnum AttributeClassification { get; set; }

        /// <summary>
        /// 物料有效期(有效天数)
        /// </summary>
        public int? ExpirationDate { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 批次大小
        /// </summary>
        public int Batch { get; set; }

        /// <summary>
        /// 数据收集方式: 1：内部  2：外部 3：批次
        /// </summary>
        public MaterialSerialNumberEnum SerialNumber { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public string? Specification { get; set; }
    }
}
