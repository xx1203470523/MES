/*
 *creator: Karl
 *
 *describe: 操作面板    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-01 11:07:16
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 操作面板，数据实体对象   
    /// manu_face_plate
    /// @author Karl
    /// @date 2023-04-01 11:07:16
    /// </summary>
    public class ManuFacePlateEntity : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型;1、生产过站；2、在制品维修；3、容器装箱
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

       /// <summary>
        /// 状态;0、新建、1、启用、2、保留、3、废除；
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
