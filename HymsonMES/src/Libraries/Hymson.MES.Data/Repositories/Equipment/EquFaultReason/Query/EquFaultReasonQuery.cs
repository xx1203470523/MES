namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表 查询故障原因
    /// </summary>
    public class EquFaultReasonQuery: QueryAbstraction
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :故障原因代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; } 

        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }
    }
}
