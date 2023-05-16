/*
 *creator: Karl
 *
 *describe: 托盘条码关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 托盘条码关系，数据实体对象   
    /// manu_tray_sfc_relation
    /// @author chenjianxiong
    /// @date 2023-05-16 11:11:13
    /// </summary>
    public class ManuTraySfcRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 托盘id
        /// </summary>
        public long TrayLoadId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int? Seq { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

       
    }
}
