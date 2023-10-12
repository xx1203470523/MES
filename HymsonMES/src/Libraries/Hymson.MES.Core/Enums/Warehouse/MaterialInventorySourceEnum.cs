﻿using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料库存来源类型
    /// </summary>
    public enum MaterialInventorySourceEnum : sbyte
    {
        /// <summary>
        /// 手动录入
        /// </summary>
        [Description("手动录入")]
        ManualEntry = 1,
        /// <summary>
        /// WMS
        /// </summary>
        [Description("WMS")]
        WMS = 2,
        /// <summary>
        /// 上料点编号
        /// </summary>
        [Description("上料点")]
        LoadingPoint = 3,
        /// <summary>
        /// 生产完成
        /// </summary>
        [Description("生产完成")]
        ManuComplete = 4,

        /// <summary>
        /// 库存维护   (仅仅做记录使用)
        /// </summary>
        [Description("库存维护")]
        InventoryModify = 6,
        /// <summary>
        /// 拆解
        /// </summary>
        [Description("拆解")]
        Disassembly = 6
    }
}
