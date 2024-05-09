namespace Hymson.MES.EquipmentServices.Dtos.Parameter
{
    /// <summary>
    /// 设备过程参数
    /// </summary>
    public  class EquipmentProcessParameterDto
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; } = "";

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; } = "";

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }

    /// <summary>
    /// 获取参数名称dto
    /// </summary>
    public class GetParamNameDto
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = string.Empty;

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; } = string.Empty;
    }
  }
