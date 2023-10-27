namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderIdBo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 是否需要验证激活状态
        /// </summary>
        public bool IsVerifyActivation { get; set; } = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderIdsBo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public IEnumerable<long> WorkOrderIds { get; set; }

        /// <summary>
        /// 是否需要验证激活状态
        /// </summary>
        public bool IsVerifyActivation { get; set; } = true;
    }
}
