using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 退料单状态 1、申请成功待检验 2、检验中 3、检验完成待入库 4、退料入库中 5、退料入库完成 6、取消退料
    /// </summary>
    public enum WhWarehouseMaterialReturnStatusEnum : sbyte
    {
        /// <summary>
        /// 申请成功待检验
        /// </summary>
        [Description("申请成功待检验")]
        ApplicationSuccessful = 1,

        /// <summary>
        /// 检验中
        /// </summary>
        [Description("检验中")]
        Inspectioning = 2,

        ///// <summary>
        ///// 检验不通过
        ///// </summary>
        //[Description("检验不通过")]
        //InspectionFailed = 3,

        /// <summary>
        /// 待入库
        /// </summary>
        [Description("待入库")]
        PendingStorage = 4,

        /// <summary>
        /// 退料入库中
        /// </summary>
        [Description("退料入库中")]
        InStorage = 5,

        /// <summary>
        /// 退料入库完成
        /// </summary>
        [Description("退料入库完成")]
        Completed = 6,

        /// <summary>
        /// 取消退料
        /// </summary>
        [Description("取消退料")]
        CancelMaterialReturn = 7
    }
}
