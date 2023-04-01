/*
 *creator: Karl
 *
 *describe: 操作面板按钮    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 操作面板按钮，数据实体对象   
    /// manu_face_plate_button
    /// @author Karl
    /// @date 2023-04-01 02:58:19
    /// </summary>
    public class ManuFacePlateButtonEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 面板Id
        /// </summary>
        public long? FacePlateId { get; set; }

       /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }

       /// <summary>
        /// 热键
        /// </summary>
        public HotkeyEnum Hotkeys { get; set; }

       
    }
}
