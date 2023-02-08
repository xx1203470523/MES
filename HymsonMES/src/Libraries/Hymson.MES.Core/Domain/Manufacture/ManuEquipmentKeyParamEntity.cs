using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 设备重点参数表。对于参数表，一些使用频繁的参数，在这多记录一份数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ManuEquipmentKeyParamEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :设备编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentCode { get; set; }
        
        /// <summary>
        /// 描述 :参数编码 
        /// 空值 : false  
        /// </summary>
        public string ParamCode { get; set; }
        
        /// <summary>
        /// 描述 :参数值 
        /// 空值 : false  
        /// </summary>
        public string ParamValue { get; set; }
        }
}