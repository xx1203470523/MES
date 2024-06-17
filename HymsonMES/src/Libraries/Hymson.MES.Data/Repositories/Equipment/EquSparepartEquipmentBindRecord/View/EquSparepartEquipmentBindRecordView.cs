using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.View
{
    public class EquSparepartEquipmentBindRecordView:BaseEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }

        /// <summary>
        /// 备件记录表id equ_sparepart_record 的Id
        /// </summary>
        public long SparepartRecordId { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 安装人
        /// </summary>
        public string CreatedBy { get; set; }

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
