namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateEquipmentGroupIdCommand
    {
        /// <summary>
        /// ID（设备组）
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// ID集合（设备）
        /// </summary>
        public IEnumerable<long> EquipmentIds { get; set; }
    }
}
