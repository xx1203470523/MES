using Hymson.MES.Core.Domain.Manufacture;

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
        /// 分选参数信息
        /// </summary>
        public List<ManuSfcGradeDetailEntity> GradeDetailEntities { get; set; }
    }

    /// <summary>
    /// 档位
    /// </summary>
    public class SortingRuleGradeBo
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
