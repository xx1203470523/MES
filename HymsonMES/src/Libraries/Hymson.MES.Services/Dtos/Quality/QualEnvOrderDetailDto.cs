/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细Dto
    /// </summary>
    public record QualEnvOrderDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 环境检验单Id
        /// </summary>
        public long EnvOrderId { get; set; }

       /// <summary>
        /// 环境检验参数组明细Id
        /// </summary>
        public long GroupDetailSnapshootId { get; set; }

       /// <summary>
        /// 应检时间
        /// </summary>
        public DateTime StartTime { get; set; }

       /// <summary>
        /// 截止录入时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 实际录入时间
        /// </summary>
        public DateTime? RealTime { get; set; }

       /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

       /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

       /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 环境检验单检验明细新增Dto
    /// </summary>
    public record QualEnvOrderDetailCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 环境检验单Id
        /// </summary>
        public long EnvOrderId { get; set; }

       /// <summary>
        /// 环境检验参数组明细Id
        /// </summary>
        public long GroupDetailSnapshootId { get; set; }

       /// <summary>
        /// 应检时间
        /// </summary>
        public DateTime StartTime { get; set; }

       /// <summary>
        /// 截止录入时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 实际录入时间
        /// </summary>
        public DateTime? RealTime { get; set; }

       /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

       /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

       /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 环境检验单检验明细更新Dto
    /// </summary>
    public record QualEnvOrderDetailModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 环境检验单Id
        /// </summary>
        public long EnvOrderId { get; set; }

       /// <summary>
        /// 环境检验参数组明细Id
        /// </summary>
        public long GroupDetailSnapshootId { get; set; }

       /// <summary>
        /// 应检时间
        /// </summary>
        public DateTime StartTime { get; set; }

       /// <summary>
        /// 截止录入时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 实际录入时间
        /// </summary>
        public DateTime? RealTime { get; set; }

       /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

       /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

       /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 环境检验单检验明细分页Dto
    /// </summary>
    public class QualEnvOrderDetailPagedQueryDto : PagerInfo
    {
    }
}
