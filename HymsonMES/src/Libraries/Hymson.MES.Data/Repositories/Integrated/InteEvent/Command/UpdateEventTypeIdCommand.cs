using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated.InteEvent.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateEventTypeIdCommand : UpdateCommand
    {
        /// <summary>
        /// ID（事件类型）
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// ID集合（事件）
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }
}
