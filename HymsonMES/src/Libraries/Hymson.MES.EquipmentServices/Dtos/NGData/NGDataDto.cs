using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.NGData
{
    /// <summary>
    /// NG数据Dto
    /// </summary>
    public class NGDataDto
    {
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
