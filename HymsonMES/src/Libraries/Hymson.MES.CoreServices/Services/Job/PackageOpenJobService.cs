using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using MySqlX.XDevAPI.Common;
using System.Threading.Tasks.Dataflow;
using System.Linq;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Newtonsoft.Json;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 包装（打开）
    /// </summary>
    [Job("包装", JobTypeEnum.Standard)]
    public class PackageOpenJobService : IJobService
    {

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PackageOpenRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public PackageOpenJobService(IManuCommonService manuCommonService,
            AbstractValidator<PackageOpenRequestBo> validationRepairJob,
            IMasterDataService masterDataService)
        {
            _manuCommonService = manuCommonService;
            _validationRepairJob = validationRepairJob;
            _masterDataService = masterDataService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            await _validationRepairJob.ValidateAndThrowAsync(bo);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));


            return null;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return 0;
        }

    }
}
