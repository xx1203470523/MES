/*
 *creator: Karl
 *
 *describe: 设备维修记录 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录 查询参数
    /// </summary>
    public class EquRepairOrderPageView : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 报修单号
        /// </summary>
        public string RepairOrder { get; set; }

        /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum? Status { get; set; }


        /// <summary>
        /// 报修人
        /// </summary>
        //public string CreatedBy { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairPerson { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string ConfirmBy { get; set; }

        /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum? ConfirmResult { get; set; }

        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairStartTime { get; set; }

        /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime? RepairEndTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmOn { get; set; }
    }
}
