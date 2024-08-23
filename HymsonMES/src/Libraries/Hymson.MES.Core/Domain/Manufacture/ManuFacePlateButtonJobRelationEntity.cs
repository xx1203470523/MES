/*
 *creator: Karl
 *
 *describe: 操作面板按钮作业    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-01 03:34:48
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 操作面板按钮作业，数据实体对象   
    /// manu_face_plate_button_job_relation
    /// @author Karl
    /// @date 2023-04-01 03:34:48
    /// </summary>
    public class ManuFacePlateButtonJobRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 按钮Id
        /// </summary>
        public long FacePlateButtonId { get; set; }

       /// <summary>
        /// 作业Id
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否清除条码
        /// </summary>
        public YesOrNoEnum IsClear { get; set; }
       
    }
}
