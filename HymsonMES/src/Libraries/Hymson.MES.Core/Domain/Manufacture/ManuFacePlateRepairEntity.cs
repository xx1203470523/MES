/*
 *creator: Karl
 *
 *describe: 在制品维修面板    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 在制品维修面板，数据实体对象   
    /// manu_face_plate_repair
    /// @author Karl
    /// @date 2023-04-01 02:44:26
    /// </summary>
    public class ManuFacePlateRepairEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool IsProcedureEdit { get; set; }

       /// <summary>
        /// 是否显示产品列表
        /// </summary>
        public bool IsShowProductList { get; set; }

        /// <summary>
        /// 是否显示活动中条码
        /// </summary>
        public bool? IsShowActivityList { get; set; }
    }
}
