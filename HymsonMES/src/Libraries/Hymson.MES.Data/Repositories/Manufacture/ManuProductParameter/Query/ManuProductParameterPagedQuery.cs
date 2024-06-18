using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产品参数/设备参数查询
    /// </summary>
    public class ManuProductParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterTypeEnum? ParameterType { get; set; }
        /// <summary>
        ///采集开始时间
        ///CreatedOn
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        ///采集结束时间
        ///CreatedOn
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        ///设备上报开始时间
        ///CreatedOn
        /// </summary>
        public DateTime? LocalTimeStartTime { get; set; }
        /// <summary>
        ///设备上报结束时间
        ///CreatedOn
        /// </summary>
        public DateTime? LocalTimeEndTime { get; set; }

    }

    public class ManuProductParameterTestPagedQuery : PagerInfo
    {

        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// SFC
        /// </summary>
        public IEnumerable<string> SFC { get; set; }
        /// <summary>
        /// 参数Id
        /// </summary>
        public IEnumerable<long>? ParameterId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public long? ProcedureId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime[]? LocalTimes { get; set; }

        /// <summary>
        /// 是否OCV
        /// </summary>
        public bool IsOCV { get; set; } = false;
    }
}
