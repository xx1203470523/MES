using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备开机参数记录表 分页参数
    /// </summary>
    public class EquOpenParamRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }
}
