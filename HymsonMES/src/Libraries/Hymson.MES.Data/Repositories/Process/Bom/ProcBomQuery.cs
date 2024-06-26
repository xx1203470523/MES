namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM表 查询参数
    /// </summary>
    public class ProcBomQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :BOM 
        /// 空值 : false  
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 集合（BOMId） 
        /// </summary>
        public IEnumerable<long>? BomIds { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : false  
        /// </summary>
        public string Version { get; set; }

    }
}
