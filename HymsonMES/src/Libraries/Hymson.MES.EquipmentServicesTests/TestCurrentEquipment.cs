using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using System.Security.Claims;

namespace Hymson.MES.EquipmentServicesTests
{
    public class TestCurrentEquipment : ICurrentEquipment
    {
        public long? Id { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["Id"].ParseToLong(); } }

        public string Name { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["Name"].ParseToString() ?? string.Empty; } }

        public long FactoryId { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["FactoryId"].ParseToLong() ?? 0; } }

        public long SiteId { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["SiteId"].ParseToLong() ?? 0; } }

        public bool IsAuthenticated => Id.HasValue;
        public string Code { get { return CurrentEquipmentInfo.EquipmentInfoDic.Value?["Code"].ParseToString() ?? string.Empty; } }

        public Claim? FindClaim(string claimType)
        {
            return null;
        }
    }
}
