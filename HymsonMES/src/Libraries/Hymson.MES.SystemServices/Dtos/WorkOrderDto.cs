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
    ///工单及BOM Dto
    /// </summary>
    public record WorkOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号 
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 分录号内码
        /// </summary>
        public string? FentryId { get; set; }

        /// <summary>
        /// 需求单号
        /// </summary>
        public string? RequirementNumber { get; set; }

        /// <summary>
        /// 产品编号 
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// 工作中心编码 
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划生产时间
        /// </summary>
        public string PlanStart { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string PlanEnd { get; set; }

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// Bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// Bom版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// Bom物料清单
        /// </summary>
        public List<BomMaterialDto> BomMaterials { get; set; }
    }

    /// <summary>
    /// bom物料
    /// </summary>
    public class BomMaterialDto
    {
        /// <summary>
        /// 发料方式
        /// </summary>
        public MaterialMethodEnum MaterialMethod { get; set; }

        /// <summary>
        /// 产出工序
        /// </summary>
        public string? OutPutProcedure { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// 物料用量
        /// </summary>
        public decimal MaterialDosage { get; set; }

        /// <summary>
        /// 物料损耗
        /// </summary>
        public decimal MaterialLoss { get; set; }

        /// <summary>
        /// 物料的替代料列表
        /// </summary>
        public List<BomReplaceMaterialDto>? BomReplaceMaterials { get; set; }
    }

    /// <summary>
    /// bom替代料
    /// </summary>
    public class BomReplaceMaterialDto
    {
        /// <summary>
        /// 替代料编码
        /// </summary>
        public string AlternativeCode { get; set; }

        /// <summary>
        /// 替代料版本
        /// </summary>
        public string AlternativeVersion { get; set; }

        /// <summary>
        /// 替代料用量
        /// </summary>
        public decimal AlternativeDosage { get; set; }

        /// <summary>
        /// 替代料损耗
        /// </summary>
        public decimal AlternativeLoss { get; set; }
    }
}
