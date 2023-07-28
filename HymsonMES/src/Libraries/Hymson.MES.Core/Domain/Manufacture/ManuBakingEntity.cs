/*
 *creator: Karl
 *
 *describe: 烘烤工序    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-28 05:41:12
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 烘烤工序，数据实体对象   
    /// manu_baking
    /// @author wxk
    /// @date 2023-07-28 05:41:12
    /// </summary>
    public class ManuBakingEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工序Id
        /// </summary>
        public long ProduceId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 极卷条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 位置(最近一次烘烤的位置)
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime BakingOn { get; set; }

       /// <summary>
        /// 烘烤预计时长
        /// </summary>
        public int? BakingPlan { get; set; }

       /// <summary>
        /// 烘烤执行时长
        /// </summary>
        public int? BakingExecution { get; set; }

       /// <summary>
        /// 烘烤状态0烘烤中 1暂停
        /// </summary>
        public bool? Status { get; set; }

       
    }
}
