using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Common.Command
{
    /// <summary>
    /// 删除实体
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class DeleteCommand
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }
    }
}
