using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.CoreServices.Bos.Common.MasterData;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.Web.Framework.WorkContext;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Manufacture.ProductionProcess
{
    /// <summary>
    /// 生产过程
    /// </summary>
    public class ProductionProcessServices : IProductionProcessServices
    {
        private readonly IExecuteJobService<JobBaseBo> _executeJobService;
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 生产过程
        /// </summary>
        public ProductionProcessServices(IExecuteJobService<JobBaseBo> executeJobService,
            ICurrentEquipment currentEquipment,
            IMasterDataService masterDataService)
        {
            _executeJobService = executeJobService;
            _currentEquipment = currentEquipment;
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// 条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationAsync(InStationDto param)
        {
            var manufactureProcedureBo = await _masterDataService.GetManufactureEquipmentAsync(new ManufactureEquipmentBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = param.ResourceCode,
                EquipmentCode= _currentEquipment.Code
            });
            List<InStationRequestBo> SFCs = new();
            SFCs.Add(new InStationRequestBo { SFC= param .SFC});

            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Code,
                ProcedureId = manufactureProcedureBo.ProcedureId,
                ResourceId = manufactureProcedureBo.ResourceId,
                InStationRequestBos = SFCs
            };

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            var responseBo = await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }
    }
}
