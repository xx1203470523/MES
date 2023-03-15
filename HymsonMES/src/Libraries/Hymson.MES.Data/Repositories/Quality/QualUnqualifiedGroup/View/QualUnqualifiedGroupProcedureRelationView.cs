using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.View
{
    /// <summary>
    /// 关联工序视图
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupProcedureRelationView
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        // <summary>
        /// 不合格代码工序
        /// </summary>
        public long ProcedureId { get; set; }

        // <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
