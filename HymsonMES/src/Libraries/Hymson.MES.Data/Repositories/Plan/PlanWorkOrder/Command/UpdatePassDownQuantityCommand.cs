namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 修改工单下达数量
    /// </summary>
    public class UpdatePassDownQuantityCommand
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQuantity { get; set; }

        /// <summary>
        /// 下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
