using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.AgvTaskRecord
{
    /// <summary>
    /// AGV任务记录表新增/更新Dto
    /// </summary>
    public record AgvTaskRecordSaveDto : BaseEntityDto
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
        /// 任务类型
        /// </summary>
        public string TaskType { get; set; }

       /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent { get; set; }

       /// <summary>
        /// 接收内容
        /// </summary>
        public string ReceiveContent { get; set; }

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
    /// AGV任务记录表Dto
    /// </summary>
    public record AgvTaskRecordDto : BaseEntityDto
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
        /// 任务类型
        /// </summary>
        public string TaskType { get; set; }

       /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent { get; set; }

       /// <summary>
        /// 接收内容
        /// </summary>
        public string ReceiveContent { get; set; }

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
    /// AGV任务记录表分页Dto
    /// </summary>
    public class AgvTaskRecordPagedQueryDto : PagerInfo { }

}
