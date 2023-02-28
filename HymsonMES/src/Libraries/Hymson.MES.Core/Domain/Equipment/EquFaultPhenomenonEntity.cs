using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障现象数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultPhenomenonEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :故障现象代码 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonCode { get; set; }

        /// <summary>
        /// 描述 :故障现象名称 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonName { get; set; }

        /// <summary>
        /// 描述 :设备组ID 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :使用状态 0-禁用 1-启用 
        /// 空值 : false  
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }
    }
}