namespace Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command
{
    public class UpdateConsumableTypeIdCommand
    {
        /// <summary>
        /// ID（工装类型）
        /// </summary>
        public long ConsumableTypeId { get; set; }

        /// <summary>
        /// ID集合（工装）
        /// </summary>
        public IEnumerable<long> ConsumableIds { get; set; }
    }
}
