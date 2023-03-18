/*
 *creator: Karl
 *
 *describe: 条码零部件表（组件合并）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:40:55
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码零部件表（组件合并），数据实体对象   
    /// manu_sfc_parts
    /// @author zhaoqing
    /// @date 2023-03-18 05:40:55
    /// </summary>
    public class ManuSfcPartsEntity : BaseEntity
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public string OperationId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public string ResourceId { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public string EquipmentId { get; set; }

       /// <summary>
        /// 主条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 零部件信息id
        /// </summary>
        public long PartsBarCodeId { get; set; }

       /// <summary>
        /// 绑定
        /// </summary>
        public bool IsBind { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
