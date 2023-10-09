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
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 工序编码集合
        /// </summary>
        public string[]? ProcedureCodes { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResName { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string? ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string? ParameterName { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string[]? ParameterNameStr { get; set; }
        /// <summary>
        /// 条码字符，多个使用分号;分割
        /// </summary>
        public string? SFCStr { get; set; }
        /// <summary>
        /// 条码集合
        /// </summary>
        public string[] SFCS { get; set; }
        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime[]? LocalTimes { get; set; }
    }
}
