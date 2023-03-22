using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码步骤表:步骤类型
    /// </summary>
    public enum ManuSfcStepTypeEnum : sbyte
    {
        //步骤类型;1：创建；2：复用；3：进站；4：出站；5：将来锁定；6：及时锁定；7：解锁；8：完成；9：报废；10：异常标识；11：删除；12：拆解；13：合并；14：转换；
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
        Unlock = 7
    }
}
