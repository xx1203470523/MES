using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Manufacture.ProductionProcess;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    [Route("EquipmentService/api/v1/[controller]")]
    [ApiController]
    public class ProductionProcessController
    {
        private readonly IProductionProcessServices _productionProcessServices;

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="productionProcessServices"></param>
        public ProductionProcessController(IProductionProcessServices productionProcessServices)
        {
            _productionProcessServices = productionProcessServices;
        }

        /// <summary>
        ///单条码进站
        /// </summary>
        /// <param name="param"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("InStation")]
        public async Task InStation(InStationDto param)
        {
            await _productionProcessServices.InStationAsync(param);
        }
    }
}
