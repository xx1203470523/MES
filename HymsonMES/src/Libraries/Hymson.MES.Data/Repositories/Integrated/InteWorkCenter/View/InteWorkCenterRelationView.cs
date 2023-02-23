using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.View
{
    /// <summary>
    /// 关联工作中心视图
    /// </summary>
    public class InteWorkCenterRelationView
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父级工作中心id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 子集工作中心id
        /// </summary>
        public long SubWorkCenterId { get; set; }

        // <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }

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
