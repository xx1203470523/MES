/*
 *creator: Karl
 *
 *describe: 生产过站面板    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 生产过站面板，数据实体对象   
    /// manu_face_plate_production
    /// @author Karl
    /// @date 2023-04-01 02:44:26
    /// </summary>
    public class ManuFacePlateProductionEntity : BaseEntity
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
        /// 是否有成功提示音
        /// </summary>
        public bool? IsSuccessBeep { get; set; }

       /// <summary>
        /// 成功提示音地址
        /// </summary>
        public string SuccessBeepUrl { get; set; }

       /// <summary>
        /// 成功提示音时间（ms）
        /// </summary>
        public decimal SuccessBeepTime { get; set; }

       /// <summary>
        /// 是否有错误提示音
        /// </summary>
        public bool? IsErrorBeep { get; set; }

       /// <summary>
        /// 错误提示音地址
        /// </summary>
        public string ErrorBeepUrl { get; set; }

       /// <summary>
        /// 错误提示音时间（ms）
        /// </summary>
        public decimal? ErrorBeepTime { get; set; }

       /// <summary>
        /// 是否绑定工单
        /// </summary>
        public bool IsShowBindWorkOrder { get; set; }

       /// <summary>
        /// 是否显示合格数量
        /// </summary>
        public bool IsShowQualifiedQty { get; set; }

       /// <summary>
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }

       /// <summary>
        /// 是否显示不合格数量
        /// </summary>
        public bool IsShowUnqualifiedQty { get; set; }

       /// <summary>
        /// 报警颜色
        /// </summary>
        public string UnqualifiedColour { get; set; }

       /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool IsShowLog { get; set; }

       
    }
}
