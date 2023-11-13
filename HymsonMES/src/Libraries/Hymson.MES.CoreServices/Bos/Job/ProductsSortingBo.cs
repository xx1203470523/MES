using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcGrade.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    public class ProductsSortingBo
    {
    }

    public class ProductsSortingResponseBo
    {
        /// <summary>
        /// 条码档位新增列表
        /// </summary>
        public List<ManuSfcGradeEntity> InsertGrades { get; set; }

        /// <summary>
        /// 条码档位修改列表
        /// </summary>
        public List<UpdateGradeCommand> UpdateGrades { get; set; }

        /// <summary>
        /// 分选参数信息
        /// </summary>
        public List<ManuSfcGradeDetailEntity> GradeDetailEntities { get; set; }
    }

    /// <summary>
    /// 档位
    /// </summary>
    public class SortingRuleGradeDto
    {
        /// <summary>
        /// 等级
        /// </summary>
        public IEnumerable<string> Ratings { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
