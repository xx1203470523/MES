/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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
    /// QualEnvOrderDetailDto的扩展
    /// </summary>
    public record QualEnvOrderDetailExtendDto : QualEnvOrderDetailDto
    {

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DataTypeEnum ParameterDataType { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 应检时间
        /// </summary>
        public new string StartTime { get; set; }

        /// <summary>
        /// 截止录入时间
        /// </summary>
        public new string EndTime { get; set; }

        /// <summary>
        /// 实际录入时间
        /// </summary>
        public new string? RealTime { get; set; }

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
