namespace Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command
{
    public class UpdateSparePartTypeIdCommand
    {
        /// <summary>
        /// ID（备件类型）
        /// </summary>
        public long SparePartTypeId { get; set; }

        /// <summary>
        /// ID集合（备件）
        /// </summary>
        public IEnumerable<long> SparePartIds { get; set; }
    }
}
