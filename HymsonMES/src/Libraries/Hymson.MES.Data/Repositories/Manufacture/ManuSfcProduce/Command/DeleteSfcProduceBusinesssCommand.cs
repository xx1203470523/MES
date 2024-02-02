using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
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
        /// 删除条码id（这里的ID其实是指在制品表的ID）
        /// </summary>
        public long SfcInfoId { get; set; }
    }

    /// <summary>
    /// 删除在制品业务实体类
    /// </summary>
    public class DeleteSFCProduceBusinesssByIdsCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 删除条码id（大坑：用这个值进行搜索的地方其实是在制品表ID）
        /// </summary>
        public IEnumerable<long> SfcInfoIds { get; set; }
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
