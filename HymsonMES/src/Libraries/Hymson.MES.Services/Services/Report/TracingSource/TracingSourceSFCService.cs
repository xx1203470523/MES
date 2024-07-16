using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Services;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Services.Dtos.Report;
using Minio.DataModel;
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
        private readonly IMasterDataService _masterDataService;
        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="tracingSourceCoreService"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuBarCodeRelationRepository"></param>
        public TracingSourceSFCService(ICurrentSite currentSite,
            ITracingSourceCoreService tracingSourceCoreService,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IMasterDataService masterDataService,
            IManuBarCodeRelationRepository manuBarCodeRelationRepository)
        {
            _currentSite = currentSite;
            _tracingSourceCoreService = tracingSourceCoreService;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _masterDataService = masterDataService;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
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
                await PrepareStepSourceAsync(manuSfcStepEntity, stepSourceDto);
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
                var productParameterSourceDto = new ProductParameterSourceDto();
                await PrepareProductParameterSourceDtoAsync(manuProductParameterEntity, productParameterSourceDto);
                productParameterSourceDtos.Add(productParameterSourceDto);
            }
            return productParameterSourceDtos;  
        }

        

        public async Task<IEnumerable<MaterialSourceDto>> GetMaterialSourcesAsync(string sfc)
        {
            var pagedQuery = new ComUsageReportPagedQuery();
            pagedQuery.PageIndex = 1;
            pagedQuery.PageSize = 1000;
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.CirculationType = ManuBarCodeRelationTypeEnum.SFC_Consumption;
            pagedQuery.Sfc = sfc;
            var pagedInfo = await _manuBarCodeRelationRepository.GetReportPagedInfoAsync(pagedQuery);
            var materialSourceDtos = new List<MaterialSourceDto>();
            if (pagedInfo == null || !pagedInfo.Data.Any()) return materialSourceDtos;
            
            foreach (var   manuBarCodeRelationEntity in pagedInfo.Data)
            {
                var procMaterialEntity = await _masterDataService.GetProcMaterialEntityAsync(_currentSite.SiteId??0, manuBarCodeRelationEntity.InputBarCodeMaterialId);
                if (procMaterialEntity == null) continue;
                var materialSourceDto = manuBarCodeRelationEntity.ToModel<MaterialSourceDto>();
                materialSourceDto.CirculationQty= manuBarCodeRelationEntity.InputQty;
                materialSourceDto.CirculationBarCode = manuBarCodeRelationEntity.InputBarCode;
                materialSourceDto.Sfc = manuBarCodeRelationEntity.OutputBarCode;
                if (manuBarCodeRelationEntity.ProcedureId.HasValue)
                {
                    var  procProcedureEntity = await _masterDataService.GetProcProcedureEntityAsync(_currentSite.SiteId??0, manuBarCodeRelationEntity.ProcedureId.Value);
                    materialSourceDto.ProcedureCode = procProcedureEntity == null ? "" : procProcedureEntity.Code;
                    materialSourceDto.ProcedureName = procProcedureEntity == null ? "" : procProcedureEntity.Name;
                }
                materialSourceDtos.Add(materialSourceDto);

            }
            return materialSourceDtos;
        }


        #region  private

        private async Task PrepareProductParameterSourceDtoAsync(ManuProductParameterEntity manuProductParameterEntity, ProductParameterSourceDto productParameterSourceDto)
        {
            var procProcedureEntity = await _masterDataService.GetProcProcedureEntityAsync(_currentSite.SiteId ?? 0, manuProductParameterEntity.ProcedureId);
            productParameterSourceDto.ProcedureCode = procProcedureEntity == null ? "" : procProcedureEntity.Code;
            productParameterSourceDto.ProcedureName = procProcedureEntity == null ? "" : procProcedureEntity.Name;

            productParameterSourceDto.ParameterValue = manuProductParameterEntity.ParameterValue;
            var procParameterEntity = await _masterDataService.GetProcParameterEntityAsync(_currentSite.SiteId ?? 0, manuProductParameterEntity.ParameterId);
            productParameterSourceDto.ParameterName = procParameterEntity == null ? "" : procParameterEntity.ParameterName;
            productParameterSourceDto.ParameterCode = procParameterEntity == null ? "" : procParameterEntity.ParameterCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <param name="stepSourceDto"></param>
        /// <returns></returns>
        private async Task PrepareStepSourceAsync(ManuSfcStepEntity manuSfcStepEntity, StepSourceDto stepSourceDto)
        {
            var procMaterialEntity = await _masterDataService.GetProcMaterialEntityAsync(manuSfcStepEntity.SiteId, manuSfcStepEntity.ProductId);
            stepSourceDto.MaterialCode = procMaterialEntity==null?"": procMaterialEntity.MaterialCode;
            stepSourceDto.MaterialName = procMaterialEntity==null?"": procMaterialEntity.MaterialName;
            //加载工序信息
            if (manuSfcStepEntity.ProcedureId.HasValue)
            {
                var procProcedureEntity = await _masterDataService.GetProcProcedureEntityAsync(manuSfcStepEntity.SiteId, manuSfcStepEntity.ProcedureId.Value);
                stepSourceDto.ProcedureName = procProcedureEntity == null ? "" : procProcedureEntity.Name;
                stepSourceDto.ProcedureCode = procProcedureEntity == null ? "" : procProcedureEntity.Code;
            }

            if (manuSfcStepEntity.EquipmentId.HasValue)
            {
                var equEquipmentEntity = await _masterDataService.GetEquEquipmentEntityAsync(manuSfcStepEntity.SiteId, manuSfcStepEntity.EquipmentId.Value);
                stepSourceDto.EquipmentCode = equEquipmentEntity == null ? "" : equEquipmentEntity.EquipmentCode;
                stepSourceDto.EquipmentName = equEquipmentEntity == null ? "" : equEquipmentEntity.EquipmentName;
            }

            if (manuSfcStepEntity.ResourceId.HasValue)
            {
                var procResourceEntity = await _masterDataService.GetProcResourceEntityAsync(manuSfcStepEntity.SiteId, manuSfcStepEntity.ResourceId.Value);
                stepSourceDto.ResourceCode = procResourceEntity == null ? "" : procResourceEntity.ResCode;
                stepSourceDto.ResourceName = procResourceEntity == null ? "" : procResourceEntity.ResName;
            }
        }
        #endregion
    }


}