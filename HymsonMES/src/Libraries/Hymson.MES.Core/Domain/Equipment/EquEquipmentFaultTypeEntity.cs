using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 不合格组表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentFaultTypeEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备故障编码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 设备故障名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 转态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}