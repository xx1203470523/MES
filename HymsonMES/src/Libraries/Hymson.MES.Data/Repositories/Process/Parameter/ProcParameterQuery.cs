namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表 查询参数
    /// </summary>
    public class ProcParameterQuery
    {
        ///// <summary>
        ///// 描述 :所属站点代码 
        ///// 空值 : false  
        ///// </summary>
        //public long SiteId { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :参数代码 
        /// 空值 : false  
        /// </summary>
        public string? ParameterCode { get; set; }
    }
}
