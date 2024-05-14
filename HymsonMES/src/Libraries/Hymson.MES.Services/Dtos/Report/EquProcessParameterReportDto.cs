using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 设备过程参数报表Dto
    /// </summary>
    public record EquProcessParameterReportDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心Name
        /// </summary>
        public string WorkCenterName { get; set; }

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
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

       /// <summary>
        /// 采集值
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// 空值 : false  
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectionTime { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
       
    }

    /// <summary>
    /// 设备过程参数报表分页Dto
    /// </summary>
    public class EquProcessParameterReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }

}
