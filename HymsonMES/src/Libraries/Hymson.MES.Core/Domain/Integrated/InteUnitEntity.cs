/*
 *creator: Karl
 *
 *describe: 单位表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-06-29 02:13:40
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 单位表，数据实体对象   
    /// inte_unit
    /// @author zhaoqing
    /// @date 2023-06-29 02:13:40
    /// </summary>
    public class InteUnitEntity : BaseEntity
    {
        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

       /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       
    }
}
