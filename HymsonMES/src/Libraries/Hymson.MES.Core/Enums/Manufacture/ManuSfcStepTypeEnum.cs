using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码步骤表:步骤类型
    /// </summary>
    public enum ManuSfcStepTypeEnum : short
    {
        // ; 跟枚举的对应不上了，具体以枚举的为准
        // 步骤类型;1：创建；3：进站；4：出站；5：将来锁定；6：及时锁定；7：解锁；9：报废；14：转换；15;关闭标识;16:关闭缺陷;17:删除;18:维修;19:步骤控制;20:生产更改
        /// <summary>
        /// 下达
        /// </summary>
        [Description("下达")]
        Create = 1,
        /// <summary>
        /// 条码接收
        /// </summary>
        [Description("条码接收")]
        Receive = 2,
        /// <summary>
        /// 开始
        /// </summary>
        [Description("开始")]
        InStock = 3,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        OutStock = 4,
        /// <summary>
        /// 将来锁
        /// </summary>
        [Description("将来锁")]
        FutureLock = 5,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        InstantLock = 6,
        /// <summary>
        /// 取消锁
        /// </summary>
        [Description("取消锁")]
        Unlock = 7,
        /// <summary>
        /// 不良录入
        /// </summary>
        [Description("不良录入")]
        BadEntry = 8,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Discard = 9,
        /// <summary>
        /// 停止
        /// </summary>
        [Description("停止")]
        Stop = 10,
        /// <summary>
        /// 不良复判
        /// </summary>
        [Description("不良复判")]
        BadRejudgment = 11,
        ///// <summary>
        ///// 合并
        ///// </summary>
        //[Description("合并")]
        //Merge = 12,
        ///// <summary>
        ///// 转换
        ///// </summary>
        //[Description("转换")]
        //Change = 13,
        /// <summary>
        /// 取消报废
        /// </summary>
        [Description("取消报废")]
        CancelDiscard = 14,
        /// <summary>
        /// 关闭标识
        /// </summary>
        [Description("关闭标识")]
        CloseIdentification = 15,
        /// <summary>
        /// 关闭缺陷
        /// </summary>
        [Description("关闭缺陷")]
        CloseDefect = 16,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 17,
        /// <summary>
        /// 开始维修
        /// </summary>
        [Description("开始维修")]
        Repair = 18,
        /// <summary>
        /// 步骤控制
        /// </summary>
        [Description("步骤控制")]
        StepControl = 19,
        /// <summary>
        /// 生产更改
        /// </summary>
        [Description("生产更改")]
        ManuUpdate = 20,
        /// <summary>
        /// 完成维修
        /// </summary>
        [Description("完成维修")]
        RepairComplete = 21,
        ///// <summary>
        ///// 拆解
        ///// </summary>
        //[Description("拆解")]
        //Disassembly = 22,
        ///// <summary>
        ///// 添加
        ///// </summary>
        //[Description("添加")]
        //Add = 23,
        ///// <summary>
        ///// 组装
        ///// </summary>
        //[Description("组装")]
        //Assemble= 24,
        ///// <summary>
        ///// 替换
        ///// </summary>
        //[Description("替换")]
        //Replace = 25,
        ///// <summary>
        ///// 条码绑定
        ///// </summary>
        //[Description("条码绑定")]
        //BarcodeBinding = 26,
        ///// <summary>
        ///// 条码解绑
        ///// </summary>
        //[Description("条码解绑")]
        //BarcodeUnbinding = 27,
        ///// <summary>
        ///// 包装
        ///// </summary>
        //[Description("包装")]
        //Package = 28,
        ///// <summary>
        ///// 解包
        ///// </summary>
        //[Description("解包")]
        //Unpack = 29
    }
}
