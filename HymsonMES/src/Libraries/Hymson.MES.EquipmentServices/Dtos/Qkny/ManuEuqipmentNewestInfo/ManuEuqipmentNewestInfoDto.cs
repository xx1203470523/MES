using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Qkny;

namespace Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 设备最新信息新增/更新Dto
    /// </summary>
    public record ManuEuqipmentNewestInfoSaveDto : BaseEntityDto
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
        /// 登录结果 0：通过，1不通过
        /// </summary>
        public string LoginResult { get; set; } = string.Empty;

       /// <summary>
        /// 登录结果更新时间
        /// </summary>
        public DateTime LoginResultUpdateOn { get; set; }

       /// <summary>
        /// 设备状态
        /// </summary>
        public string Status { get; set; } = string.Empty;

       /// <summary>
        /// 状态更新时间
        /// </summary>
        public DateTime StatusUpdateOn { get; set; }

       /// <summary>
        /// 心跳
        /// </summary>
        public string Heart { get; set; } = string.Empty;

       /// <summary>
        /// 心跳更新时间
        /// </summary>
        public DateTime HeartUpdateOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 停机原因
        /// </summary>
        public string DownReason { get; set; } = string.Empty;

        /// <summary>
        /// 类型 1-心跳 2-状态 3-登录
        /// </summary>
        public NewestInfoEnum Type { get; set; }
    }

    /// <summary>
    /// 设备最新信息Dto
    /// </summary>
    public record ManuEuqipmentNewestInfoDto : BaseEntityDto
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
        /// 登录结果 0：通过，1不通过
        /// </summary>
        public string LoginResult { get; set; } = string.Empty;

       /// <summary>
        /// 登录结果更新时间
        /// </summary>
        public DateTime LoginResultUpdateOn { get; set; }

       /// <summary>
        /// 设备状态
        /// </summary>
        public string Status { get; set; } = string.Empty;

       /// <summary>
        /// 状态更新时间
        /// </summary>
        public DateTime StatusUpdateOn { get; set; }

       /// <summary>
        /// 心跳
        /// </summary>
        public string Heart { get; set; } = string.Empty;

       /// <summary>
        /// 心跳更新时间
        /// </summary>
        public DateTime HeartUpdateOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; } = string.Empty;

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; } = string.Empty;

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 停机原因
        /// </summary>
        public string DownReason { get; set; } = string.Empty;

       
    }

    /// <summary>
    /// 设备最新信息分页Dto
    /// </summary>
    public class ManuEuqipmentNewestInfoPagedQueryDto : PagerInfo { }

}
