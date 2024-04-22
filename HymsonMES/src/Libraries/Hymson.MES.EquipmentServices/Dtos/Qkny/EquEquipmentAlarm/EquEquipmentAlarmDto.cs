using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.EquEquipmentAlarm
{
    /// <summary>
    /// 设备报警记录新增/更新Dto
    /// </summary>
    public record EquEquipmentAlarmSaveDto : BaseEntityDto
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
        /// 0恢复;1发生;
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 报警详细信息
        /// </summary>
        public string AlarmMsg { get; set; }

       /// <summary>
        /// 报警代码
        /// </summary>
        public string AlarmCode { get; set; }

       /// <summary>
        /// L提示不停机;M提示停机;H故障停机;
        /// </summary>
        public string AlarmLevel { get; set; }

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

       
    }

    /// <summary>
    /// 设备报警记录Dto
    /// </summary>
    public record EquEquipmentAlarmDto : BaseEntityDto
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
        /// 0恢复;1发生;
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 报警详细信息
        /// </summary>
        public string AlarmMsg { get; set; }

       /// <summary>
        /// 报警代码
        /// </summary>
        public string AlarmCode { get; set; }

       /// <summary>
        /// L提示不停机;M提示停机;H故障停机;
        /// </summary>
        public string AlarmLevel { get; set; }

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

       
    }

    /// <summary>
    /// 设备报警记录分页Dto
    /// </summary>
    public class EquEquipmentAlarmPagedQueryDto : PagerInfo { }

}
