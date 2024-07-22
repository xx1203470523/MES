namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护 查询参数
    /// </summary>
    public partial class ProcMaterialQuery
    {
        /// <summary>
        /// 物料编码列表
        /// </summary>
        public IEnumerable<string>? MaterialCodes { get; set; }

    }
}
