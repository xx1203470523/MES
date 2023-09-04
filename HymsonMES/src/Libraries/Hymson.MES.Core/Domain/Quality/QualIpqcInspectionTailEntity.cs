using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（尾检检验单）   
    /// qual_ipqc_inspection_tail
    /// @author xiaofei
    /// @date 2023-08-24 10:52:02
    /// </summary>
    public class QualIpqcInspectionTailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// IPQC检验项目Id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

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
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecuteBy { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? ExecuteOn { get; set; }
    }
}
