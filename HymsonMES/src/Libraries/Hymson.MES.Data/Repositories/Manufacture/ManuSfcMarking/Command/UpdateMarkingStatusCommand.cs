using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcMarking.Command
{
    /// <summary>
    /// 修改Marking状态
    /// </summary>
    public class UpdateMarkingStatusCommand : UpdateCommand
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态(0-关闭 1-开启)
        /// </summary>
        public MarkingStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { set; get; }
    }
}
