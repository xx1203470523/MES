namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 查询工序条码最后数据
    /// </summary>
    public  class LastManuSfcSummaryByProcedureIdAndSfcsQuery
    {

        /// <summary>
        /// 条码集合
        /// </summary>
        public  IEnumerable<string>  Sfcs { get; set; }
    }
}
