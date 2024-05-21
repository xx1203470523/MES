using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public class StepQueryDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public long SFCStepId { get; set; }
    }

    /// <summary>
    /// 步骤详情
    /// </summary>
    public class StepDetailDto
    {
        /// <summary>
        /// 操作前
        /// </summary>
        public StepDto BeforeStepDDto { get; set; }

        /// <summary>
        /// 操作后
        /// </summary>
        public StepDto AfterStepDDto { get; set; }

        /// <summary>
        /// 序列码历史
        /// </summary>
        public IEnumerable<SFCRelationDto> SequenceCodeHistories { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public IEnumerable<ExtendedPropertieDto> ExtendedProperties { get; set; }
    }

    /// <summary>
    /// 步骤表实体
    /// </summary>
    public class StepDto : ManuSfcStepEntity
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }

        /// <summary>
        ///工艺路线名称
        /// </summary>
        public string ProcessRouteName { get; set; }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// BOM编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        ///bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// bom版本
        /// </summary>
        public string BomVersion { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }
    }

    public class SFCRelationDto
    {
        /// <summary>
        /// 投入条码
        /// </summary>
        public string InputBarCode { get; set; }

        /// <summary>
        /// 产出条码
        /// </summary>
        public string OutputBarCode { get; set; }
    }

    /// <summary>
    /// 扩展属性
    /// </summary>
    public class ExtendedPropertieDto
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string? FieldValue { get; set; }
    }
}
