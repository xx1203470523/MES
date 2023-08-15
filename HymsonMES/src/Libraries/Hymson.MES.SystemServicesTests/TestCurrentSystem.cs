using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using System.Security.Claims;

namespace Hymson.MES.SystemServicesTests
{
    public class TestCurrentSystem : ICurrentSystem
    {
        public long? Id { get { return CurrentSystemInfo.SystemDic.Value?["Id"].ParseToLong(); } }

        public string Name { get { return CurrentSystemInfo.SystemDic.Value?["Name"].ParseToString() ?? string.Empty; } }

        public long FactoryId { get { return CurrentSystemInfo.SystemDic.Value?["FactoryId"].ParseToLong() ?? 0; } }

        public long SiteId { get { return CurrentSystemInfo.SystemDic.Value?["SiteId"].ParseToLong() ?? 0; } }

        public bool IsAuthenticated => Id.HasValue;

        public Claim? FindClaim(string claimType)
        {
            return null;
        }
    }
}
