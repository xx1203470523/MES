using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 报废
    /// </summary>
    public  class ScrapManuSfcByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 报废表
        /// </summary>
        public long SfcScrapId { get; set; }

        /// <summary>
        /// 条码当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }
    }
}
