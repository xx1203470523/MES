using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 设备最新信息数据实体对象 -- 废弃
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ManuEuqipmentNewestInfoEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :设备id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }
        
        /// <summary>
        /// 描述 :登录结果 0：通过，1不通过 
        /// 空值 : false  
        /// </summary>
        public string LoginResult { get; set; }
        
        /// <summary>
        /// 描述 :登录结果更新时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? LoginResultUpdatedOn { get; set; }
        
        /// <summary>
        /// 描述 :设备状态 
        /// 空值 : false  
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// 描述 :状态更新时间 
        /// 空值 : false  
        /// </summary>
        public DateTime? StatusUpdatedOn { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}