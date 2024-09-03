using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.NIO
{
    /// <summary>
    /// 数据实体（物料发货信息表）   
    /// nio_push_ActualDelivery
    /// @author YXX
    /// @date 2024-09-03 10:03:18
    /// </summary>
    public class NioPushActualDeliveryEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

        /// <summary>
        /// 合作业务（1:电池，2:电驱）
        /// </summary>
        public int PartnerBusiness { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 实际发货数量
        /// </summary>
        public decimal ShippedQty { get; set; }

        /// <summary>
        /// 实际发货时间
        /// </summary>
        public long? ActualDeliveryTime { get; set; }

        
    }
}
