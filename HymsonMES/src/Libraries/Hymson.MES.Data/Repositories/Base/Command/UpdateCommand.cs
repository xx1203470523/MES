using Hymson.Utils;

namespace Hymson.MES.Data.Repositories.Common.Command
{
    /// <summary>
    /// 实体（更新）
    /// @author Czhipu
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class UpdateCommand
    {
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; } = HymsonClock.Now();
    }
}
