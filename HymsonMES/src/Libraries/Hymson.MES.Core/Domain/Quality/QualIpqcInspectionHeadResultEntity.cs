using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（首检检验单结果）   
    /// qual_ipqc_inspection_head_result
    /// @author xiaofei
    /// @date 2023-08-21 06:12:44
    /// </summary>
    public class QualIpqcInspectionHeadResultEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 首检检验单Id;qual_ipqc_inspection_head id
        /// </summary>
        public long IpqcInspectionHeadId { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 报检人
        /// </summary>
        public string InspectionBy { get; set; }

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime InspectionOn { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteOn { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseOn { get; set; }

        /// <summary>
        /// 不合格处理方式;1、让步 2、？
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 是否当前结果;0-否  1、是
        /// </summary>
        public TrueOrFalseEnum IsCurrent { get; set; }


    }
}
