using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    /// 删除在制品业务实体类
    /// </summary>
    public class DeleteSfcProduceBusinesssBySfcInfoIdCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 删除条码id
        /// </summary>
        public long SfcInfoId { get; set; }
    }

    /// <summary>
    /// 删除在制品业务实体类
    /// </summary>
    public class DeleteSfcProduceBusinesssCommand
    {
        /// <summary>
        /// 删除条码id
        /// </summary>
        public IEnumerable<long> SfcInfoIds { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }
    }
}
