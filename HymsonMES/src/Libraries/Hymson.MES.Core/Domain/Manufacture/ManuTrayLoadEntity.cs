/*
 *creator: Karl
 *
 *describe: 托盘装载信息表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 托盘装载信息表，数据实体对象   
    /// manu_tray_load
    /// @author chenjianxiong
    /// @date 2023-05-16 11:10:43
    /// </summary>
    public class ManuTrayLoadEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 托盘条码
        /// </summary>
        public string TrayCode { get; set; }

       /// <summary>
        /// 托盘id
        /// </summary>
        public long TrayId { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

       
    }
}
