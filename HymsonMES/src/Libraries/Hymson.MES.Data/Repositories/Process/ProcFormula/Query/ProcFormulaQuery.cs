using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方维护 查询参数
    /// </summary>
    public class ProcFormulaQuery
    {
        public long SiteId { get; set; }

        public long? MaterialId { get; set; }

        public long? ProcedureId { get; set; }

        public SysDataStatusEnum? Status { get; set; }
    }

    public class ProcFormulaByCodeAndVersion 
    {
        public long SiteId { get; set; }

        public string Code {  get; set; }

        public string Version { get; set; }
    }
}
