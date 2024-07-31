namespace Hymson.MES.BackgroundServices.NIO.Dtos.ERP
{
    /// <summary>  
    /// 物料发货信息实体类  
    /// </summary>  
    public class ActualDeliveryDto
    {
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
        /// 订单周期内的需求数量（可选）  
        /// </summary>  
        public decimal? DemandQty { get; set; } // 使用Nullable<decimal>（即decimal?）来允许空值  

        /// <summary>  
        /// 实际发货时间（可选）  
        /// </summary>  
        public DateTime? ActualDeliveryTime { get; set; } // 使用Nullable<DateTime>（即DateTime?）来允许空值  

        /// <summary>  
        /// 计划发货时间（可选）  
        /// </summary>  
        public DateTime? DeliveryTime { get; set; } // 使用Nullable<DateTime>（即DateTime?）来允许空值  

        /// <summary>  
        /// 订单周期LT（Lead Time，前置时间/交货期）（可选）  
        /// </summary>  
        public decimal? LeadTime { get; set; } // 使用Nullable<decimal>（即decimal?）来允许空值  
    }
}
