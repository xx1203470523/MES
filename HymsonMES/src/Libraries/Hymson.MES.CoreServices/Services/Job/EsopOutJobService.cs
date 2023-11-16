using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// ESOP获取
    /// </summary>
    [Job("ESOP获取", JobTypeEnum.Standard)]
    public class EsopOutJobService : IJobService
    {
        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EsopOutRequestBo> _validationEsopOutJob;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        #region 仓储
        private readonly IProcEsopRepository _procEsopRepository;
        private readonly IProcEsopFileRepository _esopFileRepository;
        private readonly IInteAttachmentRepository _inteAttachmentRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="validationEsopOutJob"></param>
        /// <param name="procEsopRepository"></param>
        /// <param name="esopFileRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public EsopOutJobService(AbstractValidator<EsopOutRequestBo> validationEsopOutJob, IProcEsopRepository procEsopRepository, IProcEsopFileRepository esopFileRepository, IMasterDataService masterDataService, IInteAttachmentRepository inteAttachmentRepository, IInteWorkCenterRepository inteWorkCenterRepository, IPlanWorkOrderRepository planWorkOrderRepository, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, IProcMaterialRepository procMaterialRepository)
        {
            _validationEsopOutJob = validationEsopOutJob;
            _procEsopRepository = procEsopRepository;
            _esopFileRepository = esopFileRepository;
            _masterDataService = masterDataService;
            _inteAttachmentRepository = inteAttachmentRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 数据校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<EsopOutRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

            await _validationEsopOutJob.ValidateAndThrowAsync(bo);
        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var requestBo = param.ToBo<EsopOutRequestBo>();
            if (requestBo == null)
            {
                return default;
            }

            //获取工作中心（线体）
            var workCenterIds = await _inteWorkCenterRepository.GetWorkCenterIdByResourceIdAsync(new List<long> { requestBo.ResourceId.GetValueOrDefault()});
            if (workCenterIds == null || !workCenterIds.Any()) {
                return null;
            }

            //获取激活工单
            var workOrderActivationEntities = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(new PlanWorkOrderActivationQuery { LineIds = workCenterIds,SiteId=param.SiteId });
            if (workOrderActivationEntities == null || !workOrderActivationEntities.Any())
            {  
                return null; 
            }

            //获取工单
            var workOrderIds = workOrderActivationEntities.Select(a => a.WorkOrderId);
            var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            if (workOrderEntities == null || !workOrderEntities.Any()) {
                return null;
            }

            //获取物料
            var materialIds = workOrderEntities.Select(a => a.ProductId);
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds);
            if (procMaterialEntities == null|| !procMaterialEntities.Any())
            {
                return null;
            }

            var procEsopQuery = new ProcEsopQuery();
            procEsopQuery.ProcedureId = requestBo.ProcedureId;
            procEsopQuery.MaterialIds = materialIds;
            procEsopQuery.Status = DisableOrEnableEnum.Enable;
            procEsopQuery.SiteId = param.SiteId;
            //获取ESOP数据
            var procEsopEntities = await _procEsopRepository.GetProcEsopEntitiesAsync(procEsopQuery);
            if (procEsopEntities == null || !procEsopEntities.Any()) {
                return null;
            }

            //获取ESOPFile
            var procEsopIds = procEsopEntities.Select(a => a.Id);
            var procEsopFileEntities = await _esopFileRepository.GetProcEsopFileEntitiesAsync(new ProcEsopFileQuery { EsopIds = procEsopIds });
            if (procEsopFileEntities == null || !procEsopFileEntities.Any()) {
                return null;
            }

            //获取文件信息
            var attachmentIds= procEsopFileEntities.Select(a => a.AttachmentId);
            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(attachmentIds);

            var result =  Enumerable.Empty<EsopOutResponseBo>();
            foreach ( var item in procEsopFileEntities) {

                var procEsopEntity = procEsopEntities.FirstOrDefault(a => a.Id == item.EsopId);
                if (procEsopEntity == null) {
                    return null;
                }

                var procMaterialEntity = procMaterialEntities.FirstOrDefault(a => a.Id == procEsopEntity.MaterialId);
                if (procMaterialEntity == null) 
                {  
                    return null; 
                }

                var attachmentEntity = attachmentEntities.FirstOrDefault(a => a.Id == item.AttachmentId);
                if (attachmentEntity == null)
                {
                    return null;
                }

                var model=new EsopOutResponseBo();
                model.MaterialCode = procMaterialEntity.MaterialCode;
                model.MaterialName= procMaterialEntity.MaterialName;
                model.Version = procMaterialEntity.Version;
                model.FileName = attachmentEntity.Name;
                model.Path= attachmentEntity.Path;
                result.Append(model);
            }

            return result;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not PackageIngResponseBo data) return responseBo;
            return await Task.FromResult(new JobResponseBo { Content = data.Content! });
        }

        
    }
}
