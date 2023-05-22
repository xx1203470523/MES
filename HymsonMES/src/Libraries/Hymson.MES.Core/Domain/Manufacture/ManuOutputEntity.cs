/*
 *creator: Karl
 *
 *describe: 产出上报    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-05-19 10:44:12
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 产出上报，数据实体对象   
    /// manu_output
    /// @author pengxin
    /// @date 2023-05-19 10:44:12
    /// </summary>
    public class ManuOutputEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 合格数量
        /// </summary>
        public decimal? OKQty { get; set; }

       /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime LocalTime { get; set; }

       
    }
}
