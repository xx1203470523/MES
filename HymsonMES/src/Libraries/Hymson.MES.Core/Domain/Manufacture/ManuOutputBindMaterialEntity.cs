/*
 *creator: Karl
 *
 *describe: 产出上报绑定物料    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-05-19 10:46:49
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 产出上报绑定物料，数据实体对象   
    /// manu_output_bind_material
    /// @author pengxin
    /// @date 2023-05-19 10:46:49
    /// </summary>
    public class ManuOutputBindMaterialEntity : BaseEntity
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
        /// 绑定原材料
        /// </summary>
        public string BindCode { get; set; }

        /// <summary>
        /// 绑定类型;1 原材料  2半成品
        /// </summary>
        public ManuOutputBindMaterialTypeEnum Type { get; set; }


    }
}
