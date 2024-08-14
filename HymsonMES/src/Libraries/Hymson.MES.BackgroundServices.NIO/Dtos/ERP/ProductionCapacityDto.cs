﻿namespace Hymson.MES.BackgroundServices.NIO.Dtos.ERP
{
    /// <summary>  
    /// 合作伙伴精益与生产能力  
    /// </summary>  
    public class ProductionCapacityDto
    {
        /// <summary>  
        /// 合作业务（1:电池，2:电驱）  
        /// </summary>  
        public int PartnerBusiness { get; set; }

        /// <summary>  
        /// 订单号  
        /// </summary>  
        //public string OrderNo { get; set; }

        /// <summary>  
        /// 物料编码  
        /// </summary>  
        public string MaterialCode { get; set; }

        /// <summary>  
        /// 物料名称  
        /// </summary>  
        public string MaterialName { get; set; }

        /// <summary>  
        /// 日期（格式为yyyy-MM-dd HH:mm:ss）  
        /// </summary>  
        public string Date { get; set; }

        /// <summary>  
        /// 排班数/天  
        /// </summary>  
        public int WorkingSchedule { get; set; }

        /// <summary>  
        /// 计划产能/天  
        /// </summary>  
        public decimal PlannedCapacity { get; set; }

        /// <summary>  
        /// 稼动率（%）  
        /// </summary>  
        public decimal Efficiency { get; set; }

        /// <summary>  
        /// 节拍（s）  
        /// </summary>  
        public decimal Beat { get; set; }

        /// <summary>  
        /// 日生产工单或生产计划  
        /// </summary>  
        public decimal DailyProductionPlan { get; set; }

        /// <summary>  
        /// 瓶颈工序  
        /// </summary>  
        public string BottleneckProcess { get; set; }

        /// <summary>  
        /// 实际产量  
        /// </summary>  
        //public decimal ActualityOutput { get; set; }

        /// <summary>  
        /// 计划产量  
        /// </summary>  
        //public decimal PlanOutput { get; set; }

        /// <summary>  
        /// 下线合格数量  
        /// </summary>  
        public decimal DownlineNum { get; set; }

        /// <summary>  
        /// 成品实际入库数量  
        /// </summary>  
        public decimal ProductInNum { get; set; }

        /// <summary>  
        /// 成品库存合格量  
        /// </summary>  
        public decimal ProductStockQualified { get; set; }

        /// <summary>  
        /// 成品库存不合格量  
        /// </summary>  
        public decimal ProductStockRejection { get; set; }

        /// <summary>  
        /// 成品库存待判定  
        /// </summary>  
        public decimal ProductStockUndetermined { get; set; }

        /// <summary>  
        /// 成品备库策略（最大值）  
        /// </summary>  
        public decimal ProductBackUpMax { get; set; }

        /// <summary>  
        /// 成品备库策略（最小值）  
        /// </summary>  
        public decimal ProductBackUpMin { get; set; }

        /// <summary>  
        /// 产品批次号  
        /// </summary>  
        //public string VendorProductBatch { get; set; }

        /// <summary>  
        /// 检测数量  
        /// </summary>  
        //public decimal ParaConfigQty { get; set; }

        /// <summary>  
        /// 单位  
        /// </summary>  
        public string ParaConfigUnit { get; set; }

        /// <summary>  
        /// 停留天数  
        /// </summary>  
        //public decimal StayDays { get; set; }
    }
}
