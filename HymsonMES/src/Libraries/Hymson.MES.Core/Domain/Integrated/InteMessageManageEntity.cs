using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（消息管理）   
    /// inte_message_manage
    /// @author Czhipu
    /// @date 2023-08-15 08:47:52
    /// </summary>
    public class InteMessageManageEntity : BaseEntity
    {
        /// <summary>
        /// 消息单号
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 车间id
        /// </summary>
        public long WorkShopId { get; set; }

       /// <summary>
        /// 线体id
        /// </summary>
        public long LineId { get; set; }

       /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

       /// <summary>
        /// 事件描述
        /// </summary>
        public string? EventDescription { get; set; }

       /// <summary>
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum Status { get; set; }

       /// <summary>
        /// 紧急程度;1、高2、中3、低
        /// </summary>
        public DegreeEnum UrgencyLevel { get; set; }

       /// <summary>
        /// 责任部门
        /// </summary>
        public long? DepartmentId { get; set; }

       /// <summary>
        /// 责任人
        /// </summary>
        public string? ResponsibleBy { get; set; }

       /// <summary>
        /// 原因分析
        /// </summary>
        public string? ReasonAnalysis { get; set; }

       /// <summary>
        /// 处理方案
        /// </summary>
        public string? HandleSolution { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 接收时长
        /// </summary>
        public decimal? ReceiveDuration { get; set; }

       /// <summary>
        /// 处理时长
        /// </summary>
        public decimal? HandleDuration { get; set; }

       /// <summary>
        /// 评价时间
        /// </summary>
        public string? EvaluateOn { get; set; }

       /// <summary>
        /// 评价人
        /// </summary>
        public string? EvaluateBy { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
