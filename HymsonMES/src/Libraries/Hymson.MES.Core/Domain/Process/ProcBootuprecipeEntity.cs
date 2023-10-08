/*
 *creator: Karl
 *
 *describe: 开机配方表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 开机配方表，数据实体对象   
    /// proc_bootuprecipe
    /// @author wxk
    /// @date 2023-07-05 04:11:36
    /// </summary>
    public class ProcBootuprecipeEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 配方编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本1.01.01
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 是否使用0 未使用，1使用
        /// </summary>
        public bool? IsUsed { get; set; }

       
    }
}
