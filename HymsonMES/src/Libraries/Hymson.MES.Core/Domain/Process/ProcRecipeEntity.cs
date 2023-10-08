/*
 *creator: Karl
 *
 *describe: 配方表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-04 03:02:39
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 配方表，数据实体对象   
    /// proc_recipe
    /// @author wxk
    /// @date 2023-07-04 03:02:39
    /// </summary>
    public class ProcRecipeEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 配方编码
        /// </summary>
        public string RecipeCode { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 配方名称
        /// </summary>
        public string RecipeName { get; set; }

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
