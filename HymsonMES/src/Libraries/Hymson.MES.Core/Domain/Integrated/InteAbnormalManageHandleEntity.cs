using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常管理之处理结果表（包括处理和关闭两种状态数据）数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalManageHandleEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常管理表id 
        /// 空值 : false  
        /// </summary>
        public long AbnormalManageId { get; set; }
        
        /// <summary>
        /// 描述 :责任部门（sys_dept表id） 
        /// 空值 : false  
        /// </summary>
        public long DutyDepartmentId { get; set; }
        
        /// <summary>
        /// 描述 :责任人（sys_user表id） 
        /// 空值 : false  
        /// </summary>
        public long DutyUserId { get; set; }
        
        /// <summary>
        /// 描述 :异常原因 
        /// 空值 : false  
        /// </summary>
        public string AbnormalReason { get; set; }
        
        /// <summary>
        /// 描述 :异常原因分析报告地址 
        /// 空值 : false  
        /// </summary>
        public string AbnormalReasonReportUrl { get; set; }
        
        /// <summary>
        /// 描述 :处理方案 
        /// 空值 : false  
        /// </summary>
        public string AbnormalSolution { get; set; }
        
        /// <summary>
        /// 描述 :异常处理方案报告地址 
        /// 空值 : false  
        /// </summary>
        public string AbnormalSolutionReportUrl { get; set; }
        
        /// <summary>
        /// 描述 :接收时长（分钟） 
        /// 空值 : false  
        /// </summary>
        public int ReceiveTime { get; set; }
        
        /// <summary>
        /// 描述 :处理时长 
        /// 空值 : false  
        /// </summary>
        public int HandleTime { get; set; }
        
        /// <summary>
        /// 描述 :关闭备注（关闭时填写的备注） 
        /// 空值 : false  
        /// </summary>
        public string CloseRemark { get; set; }
        
        /// <summary>
        /// 描述 :关闭人 
        /// 空值 : false  
        /// </summary>
        public string CloseBy { get; set; }
        
        /// <summary>
        /// 描述 :关闭时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? CloseOn { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : false  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}