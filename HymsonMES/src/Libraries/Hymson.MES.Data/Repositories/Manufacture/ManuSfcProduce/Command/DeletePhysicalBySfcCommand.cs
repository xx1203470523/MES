namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    /// 删除 (物料删除)
    /// </summary>
    public class DeletePhysicalBySfcCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }
    }

    /// <summary>
    /// 删除 (物料删除)
    /// </summary>
    public class DeletePhysicalBySfcsCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 删除条码id
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }

    /// <summary>
    /// 删除 (物料删除)
    /// </summary>
    public class DeletePhysicalByProduceIdCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 删除条码id
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 删除 (物料删除)
    /// </summary>
    public class PhysicalDeleteSFCProduceByIdsCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 删除条码id
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }

}
