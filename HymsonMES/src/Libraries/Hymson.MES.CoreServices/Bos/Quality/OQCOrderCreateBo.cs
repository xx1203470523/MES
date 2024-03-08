using Hymson.MES.Core.Domain.WhShipment;
using Hymson.MES.Core.Domain.WhShipmentMaterial;
using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Quality
{
    public class OQCOrderCreateBo : CoreBaseBo
    {
        /// <summary>
        /// 出货单
        /// </summary>
        public WhShipmentEntity ShipmentEntity { get; set; }

        /// <summary>
        /// 出货单明细列表
        /// </summary>
        public IEnumerable<WhShipmentMaterialEntity> ShipmentMaterialEntities { get; set; }
    }
}
