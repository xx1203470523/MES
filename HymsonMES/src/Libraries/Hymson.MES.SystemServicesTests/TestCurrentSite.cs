using Hymson.Authentication.JwtBearer.Security;
using Hymson.Utils;

namespace Hymson.MES.SystemServicesTests
{
    public class TestCurrentSite : ICurrentSite
    {
        public bool IsAvailable => SiteId.HasValue;

        public long? SiteId { get { return CurrentSystemInfo.SystemDic.Value?["SiteId"].ParseToLong(); } }

        public string Name { get { return CurrentSystemInfo.SystemDic.Value?["SiteName"]?.ToString() ?? string.Empty; } }
    }
}
