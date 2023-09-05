using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 工序
    /// </summary>
    public class ManuProcedureBo : MultiSFCBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; } = null;

    }

    /// <summary>
    /// 工序
    /// </summary>
    public class ManuRouteProcedureBo
    {
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

    }
}
