using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 追溯条码日志 DTO
    /// </summary>
    public record StepSourceDto:BaseEntityDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string SFC { get; set; }

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

        /// <summary>
        /// 作业编码
        /// </summary>
        public string JobOrAssemblyCode { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public JobOrAssemblyNameEnum JobOrAssemblyName { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
