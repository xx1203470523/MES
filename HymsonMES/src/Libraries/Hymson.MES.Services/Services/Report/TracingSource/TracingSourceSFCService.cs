using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.CoreServices.Services;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Services.Dtos.Report;
using OfficeOpenXml.Utils;

namespace Hymson.MES.Services.Services
{
    /// <summary>
    /// 条码追溯服务
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 当前站点对象
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（条码追溯）
        /// </summary>
        private readonly ITracingSourceCoreService _tracingSourceCoreService;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="tracingSourceCoreService"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public TracingSourceSFCService(ICurrentSite currentSite,
            ITracingSourceCoreService tracingSourceCoreService,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _currentSite = currentSite;
            _tracingSourceCoreService = tracingSourceCoreService;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceDto> SourceAsync(string sfc)
        {
            var data = await _tracingSourceCoreService.SourceAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            return data.ToDto<NodeSourceDto>();
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceDto> DestinationAsync(string sfc)
        {
            var data = await _tracingSourceCoreService.DestinationAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            return data.ToDto<NodeSourceDto>();
        }

        public async Task<IEnumerable< ProcedureSourceDto>> GetProcedureSourcesAsync(string sfc)
        {
           var  manuSfcSummaryEntities=await  _manuSfcSummaryRepository.GetSummaryEntitiesBySfcAsync(_currentSite.SiteId??0,sfc);
            var  procedureSourceDtos=new List<ProcedureSourceDto>();
            foreach (var  manuSfcSummaryEntity in manuSfcSummaryEntities)
            {
                procedureSourceDtos.Add(manuSfcSummaryEntity.ToModel<ProcedureSourceDto>());
            }
            return procedureSourceDtos;
        }

        public async Task<IEnumerable<StepSourceDto>> GetStepSourcesAsync(string sfc)
        {
            var manuSfcStepEntities = await _manuSfcStepRepository.GetStepsBySFCAsync(new EntityBySFCQuery { SFC = sfc, SiteId = _currentSite.SiteId ?? 0 });
            var stepSourceDtos = new List<StepSourceDto>();
            if (manuSfcStepEntities == null || !manuSfcStepEntities.Any()) return stepSourceDtos;
            foreach (var manuSfcStepEntity in manuSfcStepEntities)
            {
                var stepSourceDto = manuSfcStepEntity.ToModel<StepSourceDto>();
                stepSourceDtos.Add(stepSourceDto);
            }
            return stepSourceDtos;
        }

        public async Task<IEnumerable<ProductParameterSourceDto>> GetProductParameterSourcesAsync(string sfc)
        {

            var  manuProductParameterEntities =await _manuProductParameterRepository.GetProductParameterBySFCEntitiesAsync(new ManuProductParameterBySfcQuery { 
             SFCs=new List<string>() { sfc },
             SiteId=_currentSite.SiteId ?? 0,   
            });
            var productParameterSourceDtos = new List<ProductParameterSourceDto>();
            if (manuProductParameterEntities == null|| !manuProductParameterEntities.Any()) return productParameterSourceDtos;
            foreach (var  manuProductParameterEntity in manuProductParameterEntities)
            {
                var productParameterSourceDto = manuProductParameterEntity.ToModel<ProductParameterSourceDto>();

                productParameterSourceDtos.Add(productParameterSourceDto);
            }
            return productParameterSourceDtos;  
        }

        public async Task<IEnumerable<MaterialSourceDto>> GetMaterialSourcesAsync(string sfc)
        {
            var manuSFCNodeEntities = await _tracingSourceCoreService.OriginalSourceAsync(new EntityBySFCQuery { SFC = sfc, SiteId = _currentSite.SiteId ?? 0 });
            var materialSourceDtos = new List<MaterialSourceDto>();
            return materialSourceDtos;
        }
    }


}