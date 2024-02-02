using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码信息表 查询参数
    /// </summary>
    public class ManuSfcStatusQuery
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public  IEnumerable<string>  Sfcs { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public IEnumerable<SfcStatusEnum>? Statuss { get; set; } 
    }
}
