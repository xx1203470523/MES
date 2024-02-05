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
  }
