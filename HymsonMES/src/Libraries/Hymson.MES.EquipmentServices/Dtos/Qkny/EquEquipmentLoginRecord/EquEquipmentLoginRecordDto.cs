using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.EquEquipmentLoginRecord
{
    /// <summary>
    /// 操作员登录记录新增/更新Dto
    /// </summary>
    public record EquEquipmentLoginRecordSaveDto : BaseEntityDto
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
        /// 账号
        /// </summary>
        public string Account { get; set; } = "";

       /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; } = "";

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; } = "";

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
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 操作员登录记录Dto
    /// </summary>
    public record EquEquipmentLoginRecordDto : BaseEntityDto
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
        /// 账号
        /// </summary>
        public string Account { get; set; }

       /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; }

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
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 操作员登录记录分页Dto
    /// </summary>
    public class EquEquipmentLoginRecordPagedQueryDto : PagerInfo { }

}
