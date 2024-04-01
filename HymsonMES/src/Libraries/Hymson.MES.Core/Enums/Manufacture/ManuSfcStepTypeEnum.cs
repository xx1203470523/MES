using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码步骤表:步骤类型
    /// </summary>
    public enum ManuSfcStepTypeEnum : short
    {
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
        /// <summary>
        /// 返修
        /// </summary>
        [Description("返修")]
        Maintenance = 12,
        /// <summary>
        /// 转换
        /// </summary>
        [Description("转换")]
        Change = 13,
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
        /// <summary>
        /// 维修返回
        /// </summary>
        [Description("维修返回")]
        RepairReturn = 22,
        /// <summary>
        /// 拆解
        /// </summary>
        [Description("拆解")]
        Disassembly = 23,
        /// <summary>
        /// 添加
        /// </summary>
        [Description("添加")]
        Add = 24,
        /// <summary>
        /// 组装
        /// </summary>
        [Description("组装")]
        Assemble = 25,
        /// <summary>
        /// 替换
        /// </summary>
        [Description("替换")]
        Replace = 26,
        /// <summary>
        /// 条码绑定
        /// </summary>
        [Description("条码绑定")]
        BarcodeBinding = 27,
        /// <summary>
        /// 条码解绑
        /// </summary>
        [Description("条码解绑")]
        BarcodeUnbinding = 28,
        /// <summary>
        /// 包装
        /// </summary>
        [Description("包装")]
        Package = 29,
        /// <summary>
        /// 解包
        /// </summary>
        [Description("解包")]
        Unpack = 30,

        /// <summary>
        /// 录入等级
        /// </summary>
        [Description("录入等级")]
        EnterDowngrading = 31,

        /// <summary>
        /// 移除降级
        /// </summary>
        [Description("移除降级")]
        RemoveDowngrading = 32,

        /// <summary>
        /// Marking
        /// </summary>
        [Description("Marking录入")]
        Marking = 33,

        /// <summary>
        /// 关闭Marking
        /// </summary>
        [Description("关闭Marking")]
        CloseMarking = 34,

        /// <summary>
        /// 产出上报
        /// </summary>
        [Description("产出上报")]
        OutputReport = 35,

        /// <summary>
        /// 供应商条码接收
        /// </summary>
        [Description("供应商条码接收")]
        SupplierReceive = 36,

        /// <summary>
        /// 部分报废
        /// </summary>
        [Description("部分报废")]
        PartialScrap = 37,

        /// <summary>
        /// 条码合并
        /// </summary>
        [Description("条码合并")]
        SfcMerge = 38,
        /// <summary>
        /// 条码合并-新增
        /// </summary>
        [Description("条码合并-新增")]
        SfcMergeAdd = 39,
        /// <summary>
        /// 条码数量调整
        /// </summary>
        [Description("条码数量调整")]
        SfcQtyAdjust = 40,
        /// <summary>
        /// 条码拆分
        /// </summary>
        [Description("条码拆分")]
        Split = 41,
        /// <summary>
        /// 条码拆分-新增
        /// </summary>
        [Description("条码拆分-新增")]
        SplitCreate = 42,
        /// <summary>
        /// 离脱
        /// </summary>
        [Description("离脱")]
        Detachment = 43,
        /// <summary>
        /// 设备误判
        /// </summary>
        [Description("设备误判")]
        Misjudgment = 44
    }
}
