using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuJzBind
{
    /// <summary>
    /// 极组绑定新增/更新Dto
    /// </summary>
    public record ManuJzBindSaveDto : BaseEntityDto
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
        /// 极组条码1
        /// </summary>
        public string JzSfc1 { get; set; }

       /// <summary>
        /// 极组条码2
        /// </summary>
        public string JzSfc2 { get; set; }

       /// <summary>
        /// 电芯码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 绑定类型
        /// </summary>
        public string BindType { get; set; }

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
    /// 极组绑定Dto
    /// </summary>
    public record ManuJzBindDto : BaseEntityDto
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
        /// 极组条码1
        /// </summary>
        public string JzSfc1 { get; set; }

       /// <summary>
        /// 极组条码2
        /// </summary>
        public string JzSfc2 { get; set; }

       /// <summary>
        /// 电芯码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 绑定类型
        /// </summary>
        public string BindType { get; set; }

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
    /// 极组绑定分页Dto
    /// </summary>
    public class ManuJzBindPagedQueryDto : PagerInfo { }

}
