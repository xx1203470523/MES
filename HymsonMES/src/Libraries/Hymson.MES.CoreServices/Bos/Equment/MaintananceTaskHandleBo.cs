using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.CoreServices.Bos.Equment
{
    public class MaintananceTaskHandleBo
    {
        /// <summary>
        /// 处理方式
        /// </summary>
        public EquMaintenanceTaskProcessedEnum HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
