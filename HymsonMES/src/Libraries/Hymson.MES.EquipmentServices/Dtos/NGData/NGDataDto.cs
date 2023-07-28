using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.NGData
{
    /// <summary>
    /// Ng数据查询Dto
    /// </summary>
    public class NGDataQueryDto
    {
        public string SFC { get; set; }
        public string? ProcedureCode { get; set; }
    }
    /// <summary>
    /// NG数据Dto
    /// </summary>
    public class NGDataDto
    {
        /// <summary>
        /// 是否合格，true合格，false不合格
        /// </summary>
        public bool Passed { get; set; }
        /// <summary>
        /// 不合格代码信息
        /// </summary>
        public NGUnqualifiedDto[]? NGList { get; set; } = Array.Empty<NGUnqualifiedDto>();
    }

    /// <summary>
    /// NG数据不合格代码Dto
    /// </summary>
    public class NGUnqualifiedDto
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public string NGCode { get; set; }

        /// <summary>
        /// 不合格代码名称 
        /// </summary>
        public string NGName { get; set; }
    }
}
