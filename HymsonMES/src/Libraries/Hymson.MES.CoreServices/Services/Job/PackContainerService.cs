using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;
using System.Security.Policy;

namespace Hymson.MES.CoreServices.Services.Job;

/// <summary>
/// 容器包装
/// </summary>
[Job("容器包装", JobTypeEnum.Standard)]
public class PackContainerService : IJobService
{
    /// <summary>
    /// 验证器
    /// </summary>
    private readonly AbstractValidator<ManuFacePlatePackBo> _validationRepairJob;

    /// <summary>
    /// 构造函数 
    /// </summary>
    /// <param name="manuCommonService"></param>
    /// <param name="procProcessRouteDetailNodeRepository"></param>
    /// <param name="procProcessRouteDetailLinkRepository"></param>
    public PackContainerService(AbstractValidator<ManuFacePlatePackBo> validationRepairJob)
    {
        _validationRepairJob = validationRepairJob;
    }


    /// <summary>
    /// 参数校验
    /// </summary>
    /// <param name="param"></param>
    /// <param name="proxy"></param> 
    /// <returns></returns>
    public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
    {
        var input = param.ToBo<ManuFacePlatePackBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

        await _validationRepairJob.ValidateAndThrowAsync(input);
    }

    /// <summary>
    /// 执行前节点
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
    {
        await Task.CompletedTask;
        return null;
    }

    /// <summary>
    /// 数据组装
    /// </summary>
    /// <param name="param"></param>
    /// <param name="proxy"></param>
    /// <returns></returns>
    public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
    {
        var input = param.ToBo<ManuFacePlatePackBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));

        var result = new ManuFacePlatePackResponseBo
        {
            ContainerInfos = new List<ManuFacePlatePackInfoResponseBo>()
        };

        var defaultDto = new PackageIngResponseBo { };

        defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageService.ParseToInt().ToString());

        return await Task.FromResult(defaultDto);
    }

    /// <summary>
    /// 执行入库
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public async Task<JobResponseBo> ExecuteAsync(object obj)
    {
        JobResponseBo responseBo = new();
        if (obj is not PackageIngResponseBo data) return responseBo;
        return await Task.FromResult(new JobResponseBo { Content = data.Content! });
    }


    /// <summary>
    /// 执行后节点
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
    {
        await Task.CompletedTask;
        return null;
    }
}
