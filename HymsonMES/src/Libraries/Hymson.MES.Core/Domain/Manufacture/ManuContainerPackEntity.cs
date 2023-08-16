/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除），数据实体对象   
    /// manu_container_pack
    /// @author wxk
    /// @date 2023-04-12 02:33:13
    /// </summary>
    public class ManuContainerPackEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 容器条码id
        /// </summary>
        public long ContainerBarCodeId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

       
    }
}
