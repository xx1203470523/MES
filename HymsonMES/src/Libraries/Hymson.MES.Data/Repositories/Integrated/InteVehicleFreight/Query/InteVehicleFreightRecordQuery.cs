namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载记录 查询参数
    /// </summary>
    public class InteVehicleFreightRecordQuery
    {
        /// <summary>
        /// 载具id
        /// </summary>
        public long? VehicleId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 状态;0-绑定 1-解绑
        /// </summary>
        public int? OperateType { get; set; }
    }
}
