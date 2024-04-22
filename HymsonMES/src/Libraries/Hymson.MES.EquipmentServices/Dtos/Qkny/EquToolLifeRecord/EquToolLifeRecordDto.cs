using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.EquToolLifeRecord
{
    /// <summary>
    /// 设备夹具寿命新增/更新Dto
    /// </summary>
    public record EquToolLifeRecordSaveDto : BaseEntityDto
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
        /// 夹具ID
        /// </summary>
        public long? ToolId { get; set; }

       /// <summary>
        /// 夹具编码
        /// </summary>
        public string ToolCode { get; set; }

       /// <summary>
        /// 重量
        /// </summary>
        public decimal ToolLife { get; set; }

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
    /// 设备夹具寿命Dto
    /// </summary>
    public record EquToolLifeRecordDto : BaseEntityDto
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
        /// 夹具ID
        /// </summary>
        public long? ToolId { get; set; }

       /// <summary>
        /// 夹具编码
        /// </summary>
        public string ToolCode { get; set; }

       /// <summary>
        /// 重量
        /// </summary>
        public decimal ToolLife { get; set; }

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
    /// 设备夹具寿命分页Dto
    /// </summary>
    public class EquToolLifeRecordPagedQueryDto : PagerInfo { }

}
