/*
 *creator: Karl
 *
 *describe: 维修结果    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:58:46
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.EquRepairResult
{
    /// <summary>
    /// 维修结果，数据实体对象   
    /// equ_repair_result
    /// @author pengxin
    /// @date 2024-06-12 10:58:46
    /// </summary>
    public class EquRepairResultEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 维修单idequ_repair_orderid
        /// </summary>
        public string RepairOrderId { get; set; }

       /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime RepairStartTime { get; set; }

       /// <summary>
        /// 维修结束时间
        /// </summary>
        public DateTime RepairEndTime { get; set; }

       /// <summary>
        /// 长期处理措施
        /// </summary>
        public string LongTermHandlingMeasures { get; set; }

       /// <summary>
        /// 临时处理措施
        /// </summary>
        public string TemporaryTermHandlingMeasures { get; set; }

       /// <summary>
        /// 维修人员 数组
        /// </summary>
        public string RepairPerson { get; set; }

       /// <summary>
        /// 维修确认 1、维修完成2、重新维修
        /// </summary>
        public EquConfirmResultEnum? ConfirmResult { get; set; }

       /// <summary>
        /// 确认确认时间
        /// </summary>
        public DateTime? ConfirmOn { get; set; }

       /// <summary>
        /// 维修确认人
        /// </summary>
        public string ConfirmBy { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
