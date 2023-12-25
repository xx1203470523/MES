using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated.Command
{
    /// <summary>
    /// 硬删除
    /// </summary>
    public class InternationalizationDeleteByCustomFieldIdsCommand
    {
        public long SiteId {  get; set; }

        public IEnumerable<long> CustomFieldIds { get; set; }
    }
}
