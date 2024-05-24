/*
 *creator: Karl
 *
 *describe: 设备点检模板    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment.EquSpotcheck
{
    /// <summary>
    /// 设备点检模板，数据实体对象   
    /// equ_spotcheck_template
    /// @author pengxin
    /// @date 2024-05-13 03:06:41
    /// </summary>
    public class EquSpotcheckTemplateEntity : BaseEntity
    {
        /// <summary>
        /// 点检模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
