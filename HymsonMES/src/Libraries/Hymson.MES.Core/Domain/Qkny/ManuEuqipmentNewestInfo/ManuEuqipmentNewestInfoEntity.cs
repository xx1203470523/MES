using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity
{
    /// <summary>
    /// 数据实体（设备最新信息）   
    /// manu_euqipment_newest_info
    /// @author Yxx
    /// @date 2024-03-07 09:00:41
    /// </summary>
    public class ManuEuqipmentNewestInfoEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 登录结果 0：通过，1不通过
        /// </summary>
        public string LoginResult { get; set; }

        /// <summary>
        /// 登录结果更新时间
        /// </summary>
        public DateTime? LoginResultUpdatedOn { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 状态更新时间
        /// </summary>
        public DateTime? StatusUpdatedOn { get; set; }

        /// <summary>
        /// 心跳
        /// </summary>
        public string Heart { get; set; }

        /// <summary>
        /// 心跳更新时间
        /// </summary>
        public DateTime? HeartUpdatedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 停机原因
        /// </summary>
        public string DownReason { get; set; }
    }
}
