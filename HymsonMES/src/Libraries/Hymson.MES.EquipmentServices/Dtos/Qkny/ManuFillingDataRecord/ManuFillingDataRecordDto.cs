using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuFillingDataRecord
{
    /// <summary>
    /// 补液数据上传记录新增/更新Dto
    /// </summary>
    public record ManuFillingDataRecordSaveDto : BaseEntityDto
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
        /// 电芯进入一次注液时间
        /// </summary>
        public DateTime InTime { get; set; }

       /// <summary>
        /// 电芯离开一次注液时间
        /// </summary>
        public DateTime OutTime { get; set; }

       /// <summary>
        /// 注液前重量
        /// </summary>
        public decimal? BeforeWeight { get; set; }

       /// <summary>
        /// 注液后重量
        /// </summary>
        public decimal? AfterWeight { get; set; }

       /// <summary>
        /// 自动注液量
        /// </summary>
        public decimal? ElWeight { get; set; }

       /// <summary>
        /// 自动补液量
        /// </summary>
        public decimal? AddEl { get; set; }

       /// <summary>
        /// 自动注液总量
        /// </summary>
        public decimal? TotalEl { get; set; }

       /// <summary>
        /// 手工补液量
        /// </summary>
        public decimal? ManualEl { get; set; }

       /// <summary>
        /// 最终注液量
        /// </summary>
        public decimal? FinalEl { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 补液结果
        /// </summary>
        public bool IsOk { get; set; }

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
    /// 补液数据上传记录Dto
    /// </summary>
    public record ManuFillingDataRecordDto : BaseEntityDto
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
        /// 电芯进入一次注液时间
        /// </summary>
        public DateTime InTime { get; set; }

       /// <summary>
        /// 电芯离开一次注液时间
        /// </summary>
        public DateTime OutTime { get; set; }

       /// <summary>
        /// 注液前重量
        /// </summary>
        public decimal? BeforeWeight { get; set; }

       /// <summary>
        /// 注液后重量
        /// </summary>
        public decimal? AfterWeight { get; set; }

       /// <summary>
        /// 自动注液量
        /// </summary>
        public decimal? ElWeight { get; set; }

       /// <summary>
        /// 自动补液量
        /// </summary>
        public decimal? AddEl { get; set; }

       /// <summary>
        /// 自动注液总量
        /// </summary>
        public decimal? TotalEl { get; set; }

       /// <summary>
        /// 手工补液量
        /// </summary>
        public decimal? ManualEl { get; set; }

       /// <summary>
        /// 最终注液量
        /// </summary>
        public decimal? FinalEl { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 补液结果
        /// </summary>
        public bool IsOk { get; set; }

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
    /// 补液数据上传记录分页Dto
    /// </summary>
    public class ManuFillingDataRecordPagedQueryDto : PagerInfo { }

}
