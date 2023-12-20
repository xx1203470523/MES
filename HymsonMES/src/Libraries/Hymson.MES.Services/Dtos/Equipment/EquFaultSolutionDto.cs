using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备故障解决措施新增/更新Dto
    /// </summary>
    public record EquFaultSolutionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string FaultSolutionCode { get; set; }

       /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string FaultSolutionName { get; set; }

       /// <summary>
        /// 启用状态;新建/启用/保留/废除
        /// </summary>
        public bool UseStatus { get; set; }

       /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 设备故障解决措施Dto
    /// </summary>
    public record EquFaultSolutionDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string FaultSolutionCode { get; set; }

       /// <summary>
        /// 故障解决措施代码
        /// </summary>
        public string FaultSolutionName { get; set; }

       /// <summary>
        /// 启用状态;新建/启用/保留/废除
        /// </summary>
        public bool UseStatus { get; set; }

       /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 设备故障解决措施分页Dto
    /// </summary>
    public class EquFaultSolutionPagedQueryDto : PagerInfo { }

}
