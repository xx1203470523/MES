namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 环境监测
    /// </summary>
    public class EnvFieldDto : BaseDto
    {
        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 合作伙伴自定义的字段唯一编码. 合作伙伴通过这个字段来上报对应的采集的数据的值
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 合作伙伴自定义的字段名称, 用于简要描述
        /// </summary>
        public string VendorFieldName { get; set; }

        /// <summary>
        /// 控制项数据类型
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// 上限值, 针对 SPC 项为必填
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 下限值, 针对 SPC 项为必填
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 中文单位
        /// </summary>
        public string UnitCn { get; set; }

        /// <summary>
        /// 控制项采集的仪器/工具代码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 控制项采集的仪器/工具
        /// </summary>
        public string DeviceName { get; set; }

    }
}
