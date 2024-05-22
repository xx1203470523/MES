/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */

/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment.EquSpotcheck
{
    /// <summary>
    /// 设备点检模板与项目关系，数据实体对象   
    /// equ_spotcheck_template_item_relation
    /// @author pengxin
    /// @date 2024-05-13 03:22:39
    /// </summary>
    public class EquSpotcheckTemplateItemRelationEntity : BaseEntity
    {
        /// <summary>
        /// 点检模板ID;equ_spotcheck_template的Id
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

        /// <summary>
        /// 点检项目ID;equ_spotcheck_item的Id
        /// </summary>
        public long SpotCheckItemId { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）
        /// </summary>
        public decimal? Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }


    }
}
