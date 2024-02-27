using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQC设备
    /// </summary>
    [Flags]
    public enum IQCUtensilTypeEnum
    {
        /// <summary>
        /// 二次元影像仪
        /// </summary>
        [Description("二次元影像仪")]
        QuadraticElementImager = 1,

        /// <summary>
        /// 卡尺
        /// </summary>
        [Description("卡尺")]
        Calipers = 2,

        /// <summary>
        /// 薄膜测厚仪
        /// </summary>
        [Description("薄膜测厚仪")]
        FilmThicknessGauge = 4,

        /// <summary>
        /// 千分尺
        /// </summary>
        [Description("千分尺")]
        MicrometerMeter = 8,

        /// <summary>
        /// 钢板尺
        /// </summary>
        [Description("钢板尺")]
        SteelPlateRuler = 16,

        /// <summary>
        /// 氦检仪器
        /// </summary>
        [Description("氦检仪器")]
        HeliumDetectingInstrument = 32,

        /// <summary>
        /// 电子秤
        /// </summary>
        [Description("电子秤")]
        ElectronicScale = 64,

        /// <summary>
        /// 电子天平
        /// </summary>
        [Description("电子天平")]
        ElectronicBalance = 128,

        /// <summary>
        /// 保压气密设备
        /// </summary>
        [Description("保压气密设备")]
        PressurizingAndAirtightEquipment = 256,

        /// <summary>
        /// 绝缘耐压测试仪
        /// </summary>
        [Description("绝缘耐压测试仪")]
        InsulationWithstandVoltageTester = 512,

        /// <summary>
        /// 烘箱
        /// </summary>
        [Description("烘箱")]
        Oven = 1024,

        /// <summary>
        /// 放大镜
        /// </summary>
        [Description("放大镜")]
        MagnifyingGlass = 2048,

        /// <summary>
        /// 达因笔
        /// </summary>
        [Description("达因笔")]
        DynePen = 4096,

        /// <summary>
        /// 拉力测试仪
        /// </summary>
        [Description("拉力测试仪")]
        TensionTester = 8192,

        /// <summary>
        /// 高度卡尺
        /// </summary>
        [Description("高度卡尺")]
        HeightCaliper = 16384,

        /// <summary>
        /// 万用表
        /// </summary>
        [Description("万用表")]
        MultipurposeMeter = 32768,

        /// <summary>
        /// 百分表
        /// </summary>
        [Description("百分表")]
        DialIndicator = 65536,

        /// <summary>
        /// 直流微电阻测试仪
        /// </summary>
        [Description("直流微电阻测试仪")]
        DcMicroResistanceTester = 131072,

        /// <summary>
        /// 高精度数学显微镜
        /// </summary>
        [Description("高精度数学显微镜")]
        HighPrecisionMathematicalMicroscope = 262144,

        /// <summary>
        /// 卡尔费休滴定仪
        /// </summary>
        [Description("卡尔费休滴定仪")]
        KarlFischerTitrator = 524288,

        /// <summary>
        /// 透气度测试仪
        /// </summary>
        [Description("透气度测试仪")]
        AirPermeabilityTester = 1048576,

        /// <summary>
        /// 测试
        /// </summary>
        [Description("测试")]
        Testing = 2097152,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 4194304
    }
}
