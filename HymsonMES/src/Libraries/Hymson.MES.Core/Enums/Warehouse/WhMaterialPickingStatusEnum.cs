using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 退料单状态 1、申请成功待发料 2、发料中 3、发料完成 4、取消发料 5、退料入库完成 6、取消退料
    /// </summary>
    public enum WhMaterialPickingStatusEnum : sbyte
    {
        /// <summary>
        /// 申请成功待发料
        /// </summary>
        [Description("申请成功待发料")]
        ApplicationSuccessful = 1,

        /// <summary>
        /// 发料中
        /// </summary>
        [Description("发料中")]
        Inspectioning = 2,

        /// <summary>
        /// 发料完成
        /// </summary>
        [Description("发料完成")]
        Completed = 3,

        /// <summary>
        /// 取消发料
        /// </summary>
        [Description("取消发料")]
        CancelMaterialReturn = 4
    }
}
