using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.Utils;

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
        public EsopOutJobService(AbstractValidator<EsopOutRequestBo> validationEsopOutJob)
        {
            _validationEsopOutJob = validationEsopOutJob;
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
            return await Task.FromResult(new EmptyRequestBo { });
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            var panelModules = new List<PanelModuleEnum>() { PanelModuleEnum.ESOPGet };
            return await Task.FromResult(new JobResponseBo {
                Content = new Dictionary<string, string> { 
                    { "PanelModules", panelModules.ToSerialize()}
                },
                Message=""
            });
        }
    }
}
