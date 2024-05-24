namespace Hymson.MES.CoreServices.Services.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划更新Dto
    /// </summary>
    public record GenerateEquMaintenanceTaskDto
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 计划ID
        /// </summary>
        public long MaintenancePlanId { get; set; }

        /// <summary>
        /// 计划类型 手动 1 自动 0（自动不用传默认0）
        /// </summary>
        public int? ExecType { get; set; } = 0;
    }
}
