namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateQtyByProductIdCommand
    {
        /// <summary>
        /// ID（资源）
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public decimal Qty { get; set; }

    }
}
