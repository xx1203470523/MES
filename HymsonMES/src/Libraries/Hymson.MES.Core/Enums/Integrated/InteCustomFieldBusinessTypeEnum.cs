using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 自定义字段业务类型枚举
    /// </summary>
    public enum InteCustomFieldBusinessTypeEnum : sbyte
    {
        /// <summary>
        /// 供应商管理
        /// </summary>
        [Description("供应商管理")]
        Supplier = 1,
        /// <summary>
        /// 作业维护
        /// </summary>
        [Description("作业维护")]
        Job = 2,
        /// <summary>
        /// 工作中心
        /// </summary>
        [Description("工作中心")]
        WorkCenter = 3,
        /// <summary>
        /// 客户维护
        /// </summary>
        [Description("客户维护")]
        Customer = 4,
        /// <summary>
        /// 载具类型
        /// </summary>
        [Description("载具类型")]
        VehicleType = 5,
        /// <summary>
        /// 载具注册
        /// </summary>
        [Description("载具注册")]
        Vehicle = 6,
        /// <summary>
        /// 单位维护
        /// </summary>
        [Description("单位维护")]
        Unit = 7,
        /// <summary>
        /// 消息组
        /// </summary>
        [Description("消息组")]
        MessageGroup = 8,
        /// <summary>
        /// 事件类型
        /// </summary>
        [Description("事件类型")]
        EventType =9,
        /// <summary>
        /// 事件维护
        /// </summary>
        [Description("事件维护")]
        Event = 10,
        /// <summary>
        /// 上料点维护
        /// </summary>
        [Description("上料点维护")]
        LoadingPoint = 11,
        /// <summary>
        /// 物料维护
        /// </summary>
        [Description("物料维护")]
        Material = 12,
        /// <summary>
        /// 物料组维护
        /// </summary>
        [Description("物料组维护")]
        MaterialGroup = 13,
        /// <summary>
        /// BOM维护
        /// </summary>
        [Description("BOM维护")]
        BOM = 14,

        /// <summary>
        /// 标准参数
        /// </summary>
        [Description("标准参数")]
        Parameter = 15,
        /// <summary>
        /// 工艺路线
        /// </summary>
        [Description("工艺路线")]
        ProcessRoute = 16,
        /// <summary>
        /// 资源维护
        /// </summary>
        [Description("资源维护")]
        Resource = 17,
        /// <summary>
        /// 资源类型维护
        /// </summary>
        [Description("资源类型维护")]
        ResourceType = 18,
        /// <summary>
        /// 分选规则维护
        /// </summary>
        [Description("分选规则维护")]
        SortingRule = 19,
        /// <summary>
        /// 产品参数收集组
        /// </summary>
        [Description("产品参数收集组")]
        ProductParameterCollectionGroup = 20,
        /// <summary>
        /// Recipe参数
        /// </summary>
        [Description("Recipe参数")]
        RecipeParamter = 21,
        /// <summary>
        /// 工艺设备组
        /// </summary>
        [Description("工艺设备组")]
        ProcessEquipmentGroup = 22,
        /// <summary>
        /// 生产工单
        /// </summary>
        [Description("生产工单")]
        WorkOrder = 24,
        /// <summary>
        /// 降级规则
        /// </summary>
        [Description("降级规则")]
        DowngradingRule = 25,
        /// <summary>
        /// 不合格代码
        /// </summary>
        [Description("不合格代码")]
        UnqualifiedCode = 26,
        /// <summary>
        /// 不合格代码组
        /// </summary>
        [Description("不合格代码组")]
        UnqualifiedCodeGroup = 27,
        /// <summary>
        /// 全检验证参数
        /// </summary>
        [Description("全检验证参数")]
        FullInspectionVerificationParameter = 28,
        /// <summary>
        /// 设备注册
        /// </summary>
        [Description("设备注册")]
        Device = 29,
        /// <summary>
        /// 设备组
        /// </summary>
        [Description("设备组")]
        DeviceGroup = 30,
        /// <summary>
        /// 工具维护
        /// </summary>
        [Description("工具")]
        ToolingManage = 31
    }
}
