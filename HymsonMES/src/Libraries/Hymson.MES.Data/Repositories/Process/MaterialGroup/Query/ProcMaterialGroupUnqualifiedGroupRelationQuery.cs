namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 物料组与不合格代码组关系 查询参数
    /// </summary>
    public class ProcMaterialGroupUnqualifiedGroupRelationQuery
    {
        /// <summary>
        /// 物料组Id
        /// </summary>
        public long? MaterialGroupId { get; set; }
    }
}
