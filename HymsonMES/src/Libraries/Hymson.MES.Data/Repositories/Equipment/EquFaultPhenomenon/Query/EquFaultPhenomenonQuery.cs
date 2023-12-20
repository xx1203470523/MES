namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 查询参数（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonQuery: QueryAbstraction
    {
        /// <summary>
        /// 设备故障现象Id
        /// </summary>
        public long? Id { get; set; }

        ///// <summary>
        ///// 站点Id
        ///// </summary>
        //public long? SiteId { get; set; }
    }
}
