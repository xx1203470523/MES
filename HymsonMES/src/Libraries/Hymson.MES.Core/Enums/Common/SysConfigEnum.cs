using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（系统配置）
    /// </summary>
    public enum SysConfigEnum : sbyte
    {
        /// <summary>
        /// AQL检验水平
        /// </summary>
        [Description("AQLLevel")]
        AQLLevel = 1,
        /// <summary>
        /// AQL检验计划
        /// </summary>
        [Description("AQLPlan")]
        AQLPlan = 2,
        /// <summary>
        /// 请求站点（默认值）
        /// </summary>
        [Description("MainSite")]
        MainSite = 3,
        ///// <summary>
        ///// 转子配置
        ///// </summary>
        //[Description("Rotor")]
        //Rotor = 4,
        ///// <summary>
        ///// 定子配置
        ///// </summary>
        //[Description("Stator")]
        //Stator = 5,
        /// <summary>
        /// NIO物料数据
        /// </summary>
        [Description("NioMaterial")]
        NioMaterial = 6,
        /// <summary>
        /// NIO一次良率
        /// </summary>
        [Description("NioOneYield")]
        NioOneYield = 7,
        /// <summary>
        /// NIO主配置（转子线/定子线）
        /// </summary>
        [Description("NioBaseConfig")]
        NioBaseConfig = 8,
        /// <summary>
        /// 转子LMS的TOKEN
        /// </summary>
        [Description("RotorLmsToken")]
        RotorLmsToken = 10,
        /// <summary>
        /// 转子LMS的Url
        /// </summary>
        [Description("RotorLmsUrl")]
        RotorLmsUrl = 11,
        /// <summary>
        /// 转子LMS同步开关
        /// </summary>
        [Description("RotorLmsSwitch")]
        RotorLmsSwitch = 12,
        /// <summary>
        /// 转子工序良率
        /// </summary>
        [Description("RotorPassrateStation")]
        RotorPassrateStation = 13,

        // TODO: 14

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        [Description("ProcessRouteCode")]
        ProcessRouteCode = 15,
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Description("WorkCenterCode")]
        WorkCenterCode = 16,
        /// <summary>
        /// 转子LMS物料同步同步开关
        /// </summary>
        [Description("RotorLmsMaterialSwitch")]
        RotorLmsMaterialSwitch = 17
    }
}
