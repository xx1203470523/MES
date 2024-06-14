using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.View
{
    public class EquToolsEquipmentBindRecordView:BaseEntity
    {
        ///// <summary>
        ///// 主键Id
        ///// </summary>
        //public long Id { get; set; }

        /// <summary>
        /// 工具编码
        /// </summary>
        public string ToolCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? ToolName { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string ToolType { get; set; }

        /// <summary>
        /// 工具类型名称
        /// </summary>
        public string ToolTypeName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        ///// <summary>
        ///// 安装时间
        ///// </summary>
        //public DateTime CreatedOn { get; set; }

        ///// <summary>
        ///// 安装人
        ///// </summary>
        //public string CreatedBy { get; set; }

        /// <summary>
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 当前使用寿命
        /// </summary>
        public decimal? CurrentUsedLife { get; set; }

        /// <summary>
        /// 卸载人
        /// </summary>
        public string? UninstallBy { get; set; }

        /// <summary>
        /// 卸载时间
        /// </summary>
        public DateTime? UninstallOn { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 操作类型1、绑定 2、卸载
        /// </summary>
        public BindOperationTypeEnum OperationType { get; set; }
    }
}
