namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序配置打印表 查询参数
    /// </summary>
    public class ProcProcedurePrintReleationQuery
    {
        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 描述 :所属物料ID 
        /// 空值 : false  
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        ///物料
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }
}
