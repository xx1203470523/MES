using Hymson.MES.Core.Domain.Process;
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
    public class ManuRouteProcedureWithInfoBo
    {
        /// <summary>
        /// 连线
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailLinkEntity> ProcessRouteDetailLinks { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailNodeEntity> ProcessRouteDetailNodes { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

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
