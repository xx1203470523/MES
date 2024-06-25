using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    //领料类型
    public enum ManuRequistionTypeEnum : sbyte
    {
        /// <summary>
        /// PICKING
        /// </summary>
        [Description("工单领料")]
        WorkOrderPicking = 0,
        /// <summary>
        /// 工单补料
        /// </summary>
        [Description("工单补料")]
        WorkOrderReplenishment = 1
    }
    public enum ManuReturnTypeEnum : sbyte
    {
        /// <summary>
        /// PICKING
        /// </summary>
        [Description("工单退料")]
        WorkOrderReturn = 0,
        /// <summary>
        /// 工单补料
        /// </summary>
        [Description("工单借料")]
        WorkOrderBorrow = 1
    }
    public enum ManuMaterialFormResponseEnum : sbyte
    {
        /// <summary>
        /// 创建成功
        /// </summary>
        [Description("创建成功")]
        Created = 0,
        /// <summary>
        /// 创建失败
        /// </summary>
        [Description("创建失败")]
        Failed = 1,

       

        /// <summary>
        /// WMS审批成功
        /// </summary>
        [Description("WMS审批成功")]
        ApprovalingSuccess = 2,
        /// <summary>
        /// WMS审批失败
        /// </summary>
        [Description("WMS审批失败")]
        ApprovalingFailed = 3,
        
    }

}
