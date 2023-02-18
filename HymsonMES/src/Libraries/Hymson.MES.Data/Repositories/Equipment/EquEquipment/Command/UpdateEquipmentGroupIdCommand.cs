namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateEquipmentGroupIdCommand
    {
        /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// ID集合（设备）
        /// </summary>
        public IEnumerable<long> EquipmentIds { get; set; }
    }
}
