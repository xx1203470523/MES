using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 设备停机时间数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ManuEquipmentDownTimeEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :设备id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }
        
        /// <summary>
        /// 描述 :停机原因 
        /// 空值 : false  
        /// </summary>
        public string DownReason { get; set; }
        
        /// <summary>
        /// 描述 :是否计划停机（0-否 1-是） 
        /// 空值 : true  
        /// </summary>
        public byte IsPlanDown { get; set; }
        
        /// <summary>
        /// 描述 :状态开始时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? BeginTime { get; set; }
        
        /// <summary>
        /// 描述 :状态结束时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? EndTime { get; set; }
        
        /// <summary>
        /// 描述 :状态持续时间（单位秒） 
        /// 空值 : false  
        /// </summary>
        public int StatusDuration { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}