using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（工艺设备组）   
    /// proc_process_equipment_group
    /// @author Hjy
    /// @date 2023-07-25 10:22:45
    /// </summary>
    public class ProcProcessEquipmentGroupEntity : BaseEntity
    {
        /// <summary>
        /// 设备组编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 设备组名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
