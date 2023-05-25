/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码，数据实体对象   
    /// manu_sfc_step_material
    /// @author chenjianxiong
    /// @date 2023-05-25 08:58:04
    /// </summary>
    public class ManuSfcStepMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 步骤ID
        /// </summary>
        public long StepId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 批次条码
        /// </summary>
        public string MaterialBarcode { get; set; }

       
    }
}
