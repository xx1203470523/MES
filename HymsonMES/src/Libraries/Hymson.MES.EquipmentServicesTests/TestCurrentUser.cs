using Hymson.Authentication;
using System.Security.Claims;

namespace Hymson.MES.EquipmentServicesTests
{
    public class TestCurrentUser : ICurrentUser
    {
        public string UserName => "单元测试用户";

        public long? UserId => 8888888;

        public bool IsAuthenticated => UserId.HasValue;

        public Claim? FindClaim(string claimType)
        {
            return null;
        }
    }
}
