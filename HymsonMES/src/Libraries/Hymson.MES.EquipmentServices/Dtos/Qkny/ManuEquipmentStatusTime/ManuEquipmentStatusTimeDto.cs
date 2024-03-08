using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuEquipmentStatusTime
{
    /// <summary>
    /// 设备状态时间新增/更新Dto
    /// </summary>
    public record ManuEquipmentStatusTimeSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentStatus { get; set; }

       /// <summary>
        /// 下一个状态
        /// </summary>
        public string NextStatus { get; set; }

       /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

       /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 状态持续时间（单位秒）
        /// </summary>
        public int StatusDuration { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备停机原因
        /// </summary>
        public string EquipmentDownReason { get; set; }

       
    }

    /// <summary>
    /// 设备状态时间Dto
    /// </summary>
    public record ManuEquipmentStatusTimeDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentStatus { get; set; }

       /// <summary>
        /// 下一个状态
        /// </summary>
        public string NextStatus { get; set; }

       /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

       /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 状态持续时间（单位秒）
        /// </summary>
        public int StatusDuration { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备停机原因
        /// </summary>
        public string EquipmentDownReason { get; set; }

       
    }

    /// <summary>
    /// 设备状态时间分页Dto
    /// </summary>
    public class ManuEquipmentStatusTimePagedQueryDto : PagerInfo { }

}
