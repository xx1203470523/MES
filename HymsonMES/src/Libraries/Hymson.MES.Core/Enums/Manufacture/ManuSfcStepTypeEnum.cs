using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码步骤表:步骤类型
    /// </summary>
    public enum ManuSfcStepTypeEnum : short
    {
        // ; 跟枚举的对应不上了，具体以枚举的为准
        // 步骤类型;1：创建；2：复用；3：进站；4：出站；5：将来锁定；6：及时锁定；7：解锁；8：完成；9：报废；10：NG标识；11：删除；12：拆解；13：合并；14：转换；
        /// <summary>
        /// 创建
        /// </summary>
        [Description("创建")]
        Create = 1,
        /// <summary>
        /// 复用
        /// </summary>
        [Description("复用")]
        Multiplex = 2,
        /// <summary>
        /// 进站
        /// </summary>
        [Description("进站")]
        InStock = 3,
        /// <summary>
        /// 出站
        /// </summary>
        [Description("出站")]
        OutStock = 4,
        /// <summary>
        /// 将来锁定
        /// </summary>
        [Description("将来锁定")]
        FutureLock = 5,
        /// <summary>
        /// 及时锁定
        /// </summary>
        [Description("及时锁定")]
        InstantLock = 6,
        /// <summary>
        /// 解锁
        /// </summary>
        [Description("解锁")]
        Unlock = 7,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 8,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Discard = 9,
        /// <summary>
        /// NG标识
        /// </summary>
        [Description("NG标识")]
        NGMark = 10,
        /// <summary>
        /// 拆解
        /// </summary>
        [Description("拆解")]
        Split = 11,
        /// <summary>
        /// 合并
        /// </summary>
        [Description("合并")]
        Merge = 12,
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
        /// 维修
        /// </summary>
        [Description("维修")]
        Repair = 18
    }
}
