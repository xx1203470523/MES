/*
 *creator: Karl
 *
 *describe: manu_sfc_ng_judge    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// ManuSfcNgJudgeEntity，数据实体对象   
    /// manu_sfc_ng_judge
    /// @author chenjianxiong
    /// @date 2023-05-16 11:11:13
    /// </summary>
    public class ManuSfcNgJudgeEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// StepId
        /// </summary>
        public long? StepId { get; set; }

        /// <summary>
        /// 修改NG原因
        /// </summary>
        public long? NGJudgeType { get; set; }
    }
}
