using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源设备绑定表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceEquipmentBindEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :所属资源ID 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 描述 :设备ID 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 描述 :是否主设备 
        /// 空值 : true  
        /// </summary>
        public TrueOrFalseEnum IsMain { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

    }
}