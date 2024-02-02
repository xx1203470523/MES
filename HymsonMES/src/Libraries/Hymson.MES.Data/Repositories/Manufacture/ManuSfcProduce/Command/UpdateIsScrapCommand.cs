using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class UpdateIsScrapCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum IsScrap { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 条码的当前状态
        /// </summary>
        public TrueOrFalseEnum CurrentIsScrap { get; set; }
    }
}
