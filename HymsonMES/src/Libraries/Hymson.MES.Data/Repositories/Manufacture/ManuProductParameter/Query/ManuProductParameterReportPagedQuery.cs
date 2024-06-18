using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuProductParameterReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 工序id列表
        /// </summary>
        public long[]? ProcedureIds { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }
        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }
       
        /// <summary>
        /// 条码集合
        /// </summary>
        public string[] SFCS { get; set; }
        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime[]? LocalTimes { get; set; }
    }

    public class ManuProductParameterReportTestPagedQuery : PagerInfo
    {

        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParameterId { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public long ProductCode { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterLineId { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime[]? LocalTimes { get; set; }
    }
}
