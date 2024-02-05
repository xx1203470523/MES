using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcGrade.Command
{
    public class UpdateGradeCommand :UpdateCommand
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        public string Grade { get; set; }
    }
}
