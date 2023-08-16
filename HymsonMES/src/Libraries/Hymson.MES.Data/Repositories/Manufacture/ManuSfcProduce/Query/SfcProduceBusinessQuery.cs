using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class SfcListProduceBusinessQuery
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 集合（条码）
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 业务类型;1、返修业务  2、锁业务
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SfcProduceBusinessQuery
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 业务类型;1、返修业务  2、锁业务
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }
    }
}
