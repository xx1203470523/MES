using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 工序
    /// </summary>
    public class ManuProcedureBo : MultiSfcBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; } = null;

    }
}
