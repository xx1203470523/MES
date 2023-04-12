using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command
{
    /// <summary>
    /// 条码状态更新
    /// </summary>
    public class ManuSfcUpdateCommand
    {
        /// <summary>
        /// 产品条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }
    }
}
