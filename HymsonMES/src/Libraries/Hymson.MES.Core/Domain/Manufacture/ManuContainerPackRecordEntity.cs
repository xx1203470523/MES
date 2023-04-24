/*
 *creator: Karl
 *
 *describe: 容器装载记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 容器装载记录，数据实体对象   
    /// manu_container_pack_record
    /// @author wxk
    /// @date 2023-04-12 02:32:21
    /// </summary>
    public class ManuContainerPackRecordEntity : BaseEntity
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
        public long? ContainerBarCodeId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

        /// <summary>
        /// 操作类型;1、装载2、移除
        /// </summary>
        public int OperateType { get; set; } = 1;
    }
}
