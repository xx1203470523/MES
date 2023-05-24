using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（设备故障现象）
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultPhenomenonView : BaseEntity
    {
        /// <summary>
        /// 描述 :故障现象代码 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonCode { get; set; } = "";

        /// <summary>
        /// 描述 :故障现象名称 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonName { get; set; } = "";

        /// <summary>
        /// 描述 :使用状态 0-禁用 1-启用 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum UseStatus { get; set; }

        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

     

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";
    }
}