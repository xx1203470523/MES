namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    ///不合代码组查询参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EQualUnqualifiedGroupQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }
    }
}
