using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.EquProcessParamRecord
{
    /// <summary>
    /// 过程参数记录表新增/更新Dto
    /// </summary>
    public record EquProcessParamRecordSaveDto : BaseEntityDto
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
        /// 位置号
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParamId { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

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
    /// 过程参数记录表Dto
    /// </summary>
    public record EquProcessParamRecordDto : BaseEntityDto
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
        /// 参数Id
        /// </summary>
        public long? ParamId { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }

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
    /// 过程参数记录表分页Dto
    /// </summary>
    public class EquProcessParamRecordPagedQueryDto : PagerInfo { }

}
