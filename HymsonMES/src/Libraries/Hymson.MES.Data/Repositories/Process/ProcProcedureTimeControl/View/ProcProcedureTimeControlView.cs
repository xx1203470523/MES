using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcProcedureTimeControlView : ProcProcedureTimeControlEntity
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public string FromProcedure { get; set; }

        /// <summary>
        /// 到达工序
        /// </summary>
        public string ToProcedure { get; set; }
    }
}
