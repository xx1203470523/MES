using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateFeedingQtyByIdCommand : UpdateCommand
    {
        /// <summary>
        /// ID（主键）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public decimal Qty { get; set; }

    }
}
