using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码使用状态更新
    /// </summary>
    public class MultiSfcUpdateIsUsedCommand : UpdateCommand
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; }
    }
}
