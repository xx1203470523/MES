namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 控制项
    /// </summary>
    public class FieldDto : BaseDto
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
        /// 工位唯一标识
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 合作伙伴总成产品代码
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 控制项代码, 合作伙伴自定义的字段唯一编码.
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 合作伙伴自定义的控制项名称
        /// </summary>
        public string VendorFieldName { get; set; }

        ///// <summary>
        ///// 字段的描述信息. 补充描述该字段信息
        ///// </summary>
        //public string VendorFieldDesc { get; set; }

        ///// <summary>
        ///// 控制项所对应的对象
        ///// </summary>
        //public string FieldObject { get; set; }

        ///// <summary>
        ///// 控制项所对应的测试项目
        ///// </summary>
        //public string FieldType { get; set; }

        ///// <summary>
        ///// 生命周期代码
        ///// </summary>
        //public string LifecycleCode { get; set; }

        /// <summary>
        /// 一级分类, 合作伙伴自定义的控制项类型
        /// </summary>
        public string Category01 { get; set; }

        ///// <summary>
        ///// 二级分类
        ///// </summary>
        //public string Category02 { get; set; }

        ///// <summary>
        ///// 三级分类
        ///// </summary>
        //public string Category03 { get; set; }

        /// <summary>
        /// 是非为 CC 项
        /// </summary>
        public bool Cc { get; set; }

        /// <summary>
        /// 是非为 SC 项
        /// </summary>
        public bool Sc { get; set; }

        /// <summary>
        /// 是非为 SPC 项
        /// </summary>
        public bool Spc { get; set; }

        /// <summary>
        /// 控制项数据类型
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// 标准值, 针对 SPC 项为必填
        /// </summary>
        public decimal StdValue { get; set; }

        /// <summary>
        /// 上限值, 针对 SPC 项为必填
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 下限值, 针对 SPC 项为必填
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 单位(中文)
        /// </summary>
        public string UnitCn { get; set; }

        /// <summary>
        /// 单位(英文)
        /// </summary>
        public string UnitEn { get; set; }

        ///// <summary>
        ///// 检测方式
        ///// </summary>
        //public string DetectionMethod { get; set; }

        ///// <summary>
        ///// 控制项采集的仪器/工具的唯一编号
        ///// </summary>
        //public string DeviceId { get; set; }

        ///// <summary>
        ///// 控制项采集的仪器/工具的名称
        ///// </summary>
        //public string DeviceName { get; set; }

        ///// <summary>
        ///// 控制项要求采样率, 为空表示不做要求. max value 1.00, 1.00 即表示 100% 采样.（最大支持10位整数+5位小数）
        ///// </summary>
        //public decimal SamplingRate { get; set; }

        ///// <summary>
        ///// 追溯方式, 仅用于展示
        ///// </summary>
        //public string TraceMode { get; set; }

        ///// <summary>
        ///// 控制项检测方式的 inline / offline 设置
        ///// </summary>
        //public string DetectionMode { get; set; }

        ///// <summary>
        ///// 数据分析方法论
        ///// </summary>
        //public string AnalysisMethod { get; set; }

        ///// <summary>
        ///// 控制项上下限更新时间, Unix 时间戳, 单位: 秒
        ///// </summary>
        //public long LimitUpdateTime { get; set; }

        ///// <summary>
        ///// 控制项点位代码
        ///// </summary>
        //public string GatherSpot { get; set; }

    }
}
