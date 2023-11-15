using FluentValidation;
using FluentValidation.Validators;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.Data.Repositories.Integrated;
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
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="validationEsopOutJob"></param>
        /// <param name="procEsopRepository"></param>
        /// <param name="esopFileRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="inteAttachmentRepository"></param>
        public EsopOutJobService(AbstractValidator<EsopOutRequestBo> validationEsopOutJob, IProcEsopRepository procEsopRepository, IProcEsopFileRepository esopFileRepository, IMasterDataService masterDataService, IInteAttachmentRepository inteAttachmentRepository)
        {
            _validationEsopOutJob = validationEsopOutJob;
            _procEsopRepository = procEsopRepository;
            _esopFileRepository = esopFileRepository;
            _masterDataService = masterDataService;
            _inteAttachmentRepository = inteAttachmentRepository;
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

            //获取物料数据
            var procMaterialEntity = await _masterDataService.GetProcMaterialEntityWithNullCheckAsync(requestBo.MaterialId.GetValueOrDefault());
            if (procMaterialEntity == null)
            {
                return null;
            }

            var procEsopQuery = new ProcEsopQuery();
            procEsopQuery.ProcedureId = requestBo.ProcedureId;
            procEsopQuery.MaterialId = requestBo.MaterialId;
            procEsopQuery.Status = DisableOrEnableEnum.Enable;
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

                //var procEsopFileEntity = procEsopEntities.FirstOrDefault(a => a.Id == item.EsopId);

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
