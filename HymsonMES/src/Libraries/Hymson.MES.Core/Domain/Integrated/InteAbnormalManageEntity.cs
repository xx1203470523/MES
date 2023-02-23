using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常管理数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalManageEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常编号 
        /// 空值 : false  
        /// </summary>
        public string AbnormalCode { get; set; }
        
        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterShopId { get; set; }
        
        /// <summary>
        /// 描述 :工作中心线体id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterLineId { get; set; }
        
        /// <summary>
        /// 描述 :工作中心工厂id（根据选择的车间带出来） 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterFactoryId { get; set; }
        
        /// <summary>
        /// 描述 :异常类型id（从inte_abnormal_type表过滤关联车间找） 
        /// 空值 : false  
        /// </summary>
        public long AbnormalTypeId { get; set; }
        
        /// <summary>
        /// 描述 :异常事件名称 
        /// 空值 : false  
        /// </summary>
        public string AbnormalEventName { get; set; }
        
        /// <summary>
        /// 描述 :资源/工位id 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 描述 :紧急程度（1-高 2-中 3-低）（字典类型：inte_abnormal_urgent_level） 
        /// 空值 : false  
        /// </summary>
        public byte UrgentLevel { get; set; }
        
        /// <summary>
        /// 描述 :异常状态（1-触发 2-接收 3-处理 4-关闭）（字典类型：inte_abnormal_status） 
        /// 空值 : false  
        /// </summary>
        public byte AbnormalStatus { get; set; }
        
        /// <summary>
        /// 描述 :接收人 
        /// 空值 : false  
        /// </summary>
        public string ReceiveBy { get; set; }
        
        /// <summary>
        /// 描述 :接收时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? ReceiveOn { get; set; }
        
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
        
        /// <summary>
        /// 描述 :资源名称/工位名称 
        /// 空值 : false  
        /// </summary>
        public string ResourceName { get; set; }
        
        /// <summary>
        /// 描述 :是否接收超时 
        /// 空值 : false  
        /// </summary>
        public byte IsReceiveTimeout { get; set; }
        
        /// <summary>
        /// 描述 :是否处理超时 
        /// 空值 : false  
        /// </summary>
        public byte IsHandleTimeout { get; set; }
        
        /// <summary>
        /// 描述 :异常等级 
        /// 空值 : false  
        /// </summary>
        public int ReceiveLevel { get; set; }
        
        /// <summary>
        /// 描述 :处理异常级别 
        /// 空值 : false  
        /// </summary>
        public int HandleLevel { get; set; }
        }
}