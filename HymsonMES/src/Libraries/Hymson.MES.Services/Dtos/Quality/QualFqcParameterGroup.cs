using Hymson.MES.Core.Domain.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Quality
{
    public class QualFqcParameterGroup
    {
        public record ParameterGroupQuery
        {
            /// <summary>
            /// 条码
            /// </summary>
            public string SFC { get; set; }
            /// <summary>
            /// 物料ID
            /// </summary>
            public long MaterialId { get; set; }
        }

    }
}
