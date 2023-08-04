using Hymson.Authentication.JwtBearer.Security;
using Hymson.Utils;

namespace Hymson.MES.EquipmentServicesTests
{
    public class TestCurrentSite : ICurrentSite
    {
        public bool IsAvailable => SiteId.HasValue;

        public long? SiteId { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["SiteId"].ParseToLong(); } }

        public string Name { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["SiteName"]?.ToString() ?? string.Empty; } }
    }
}
