namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public  class QualUnqualifiedGroupByCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long Site { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
