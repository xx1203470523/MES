using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
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

        //var process = new ConfirmOnContainerPackPlateProcess();

        //#region 获取面板实体和面板信息实体

        //var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(
        //    new EntityByCodeQuery
        //    {
        //        Code = input.Code,
        //        Site = siteId
        //    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        //var manuFacePlateInfoEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id)
        //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        //#endregion

        //#region 获取面板容器条码实体

        //var manuFacePlateContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
        //    new ManuContainerBarcodeQuery
        //    {
        //        ContainerId = manuFacePlateInfoEntity.ContainerId,
        //        SiteId = siteId
        //    });

        //#endregion

        //#region 获取面板容器装载实体

        //var manuFacePlateContainerBarcodePackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
        //{
        //    LadeBarCode = manuFacePlateContainerBarcodeEntity.BarCode,
        //    SiteId = siteId
        //});

        //#endregion

        //#region 获取生产序列码实体

        //var sfc = input.Sfcs.FirstOrDefault();
        //if (sfc == null)
        //{
        //    return result;
        //}

        //var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
        //{
        //    SFC = sfc,
        //    SiteId = siteId
        //});

        //#endregion

        //if (manuSfcEntity != null)
        //{
        //    #region 检验条码状态

        //    if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Scrapping)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES11404)).WithData("sfc", manuSfcEntity.SFC);
        //    }

        //    if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Invalid)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES11411)).WithData("sfc", manuSfcEntity.SFC);
        //    }

        //    if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Locked)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES11405)).WithData("sfc", manuSfcEntity.SFC);
        //    }

        //    #endregion

        //    #region 配置验证



        //    #endregion

        //    #region 获取序列码详细信息实体和序列码产品信息实体

        //    var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16915));

        //    process.ProcMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);
        //    process.PlanWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId);

        //    #endregion

        //    if (input.PackContainerId.HasValue)
        //    {
        //        #region 包装容器条码信息

        //        var packContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
        //            new ManuContainerBarcodeQuery
        //            {
        //                ContainerId = input.PackContainerId,
        //                SiteId = siteId
        //            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16746));

        //        #endregion

        //        #region 包装容器包装信息

        //        var packContainerBarcodePackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
        //        {
        //            LadeBarCode = packContainerBarcodeEntity.BarCode,
        //            SiteId = siteId
        //        });

        //        #endregion

        //        #region 包装容器是否打开

        //        if (packContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16740));
        //        }

        //        #endregion

        //        #region 包装容器是否允许包装此序列码

        //        var packContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        //        {
        //            ContainerId = packContainerBarcodeEntity.ContainerId,
        //            SiteId = siteId
        //        });

        //        if (packContainerFreightEntities == null || packContainerFreightEntities.IsEmpty())
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16741));
        //        }

        //        var inteContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.MaterialId == process.ProcMaterialEntity.Id);
        //        if (inteContainerFreightEntity == null)
        //        {
        //            inteContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == process.ProcMaterialEntity.GroupId);
        //        }

        //        if (inteContainerFreightEntity == null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16742));
        //        }

        //        #endregion

        //        #region 包装容器是否超过容器最大装载数量                

        //        if (packContainerBarcodeEntity.Qty >= inteContainerFreightEntity.Maximum)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16743));
        //        }

        //        #endregion

        //        process.ContainerBarcodeEntity = packContainerBarcodeEntity;
        //        process.InteContainerFreightEntity = inteContainerFreightEntity;

        //        process.ContainerPackEntity = CreateContainerPackEntity(packContainerBarcodeEntity,
        //        //packContainerBarcodePackEntity,
        //            ManuContainerBarcodePackTypeEnum.ManuSfc,
        //        procedureId,
        //        resourceId,
        //        sfc,
        //        userName,
        //            siteId);
        //        process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(process.ContainerPackEntity, userName, siteId, ManuContainerPackRecordOperateTypeEnum.Load);

        //        process.DBExecuteMode = DBExecuteModeEnum.IncrementContainerBarcodeQty;
        //    }
        //    else
        //    {
        //        #region 获取生产序列码的容器装载实体

        //        var sfcContainerPackEntity = await _manuContainerPackRepository.GetOneAsync(
        //            new ManuContainerPackQuery
        //            {
        //                LadeBarCode = sfc,
        //                SiteId = siteId
        //            });

        //        #endregion

        //        if (sfcContainerPackEntity != null)
        //        {
        //            var sfcContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
        //                new ManuContainerBarcodeQuery
        //                {
        //                    Id = sfcContainerPackEntity.ContainerBarCodeId,
        //                    SiteId = siteId
        //                });

        //            if (sfcContainerBarcodeEntity.ContainerId == manuFacePlateInfoEntity.ContainerId)
        //            {
        //                #region 显示界面                        

        //                result.ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>
        //                {
        //                    new ManuFacePlateContainerInfoOutputDto()
        //                    {
        //                        WorkOrderId = process.PlanWorkOrderEntity.Id,
        //                        WorkOrderCode = process.PlanWorkOrderEntity.OrderCode,
        //                        MaterialId = process.ProcMaterialEntity.Id,
        //                        MaterialCode = process.ProcMaterialEntity.MaterialCode,
        //                        PackingId = sfcContainerPackEntity.Id,
        //                        PackingContainerId = sfcContainerBarcodeEntity.Id,
        //                        PackingContainerCode = sfcContainerBarcodeEntity.BarCode,
        //                        SFC = sfc
        //                    }
        //                };

        //                #endregion
        //            }
        //            else
        //            {
        //                throw new CustomerValidationException(nameof(ErrorCode.MES16739));
        //            }
        //        }
        //        else
        //        {
        //            process.ContainerBarcodeEntity = manuFacePlateContainerBarcodeEntity;
        //            if (process.ContainerBarcodeEntity == null)
        //            {
        //                process.ContainerBarcodeEntity = await CreateContainerBarcodeEntity(manuFacePlateInfoEntity.ContainerId, userName, siteId);
        //                process.DBExecuteMode = DBExecuteModeEnum.NewContainerBarcode;
        //            }

        //            process.ContainerPackEntity = CreateContainerPackEntity(
        //                process.ContainerBarcodeEntity,
        //                //manuFacePlateContainerBarcodePackEntity,
        //                ManuContainerBarcodePackTypeEnum.ManuSfc,
        //                procedureId,
        //                resourceId,
        //                sfc,
        //                userName,
        //                siteId);

        //            process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(
        //                process.ContainerPackEntity,
        //                userName,
        //                siteId,
        //                ManuContainerPackRecordOperateTypeEnum.Load);
        //        }
        //    }
        //}
        //else
        //{
        //    #region 容器条码实体

        //    var containerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        //    {
        //        BarCode = sfc,
        //        SiteId = siteId
        //    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16744));

        //    #endregion

        //    #region 容器是否打开

        //    if (containerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES16740));
        //    }

        //    #endregion

        //    #region 配置验证

        //    #endregion

        //    if (input.PackContainerId.HasValue)
        //    {
        //        #region 包装容器条码信息

        //        var packContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        //        {
        //            ContainerId = input.PackContainerId,
        //            SiteId = siteId
        //        }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16746));

        //        #endregion

        //        #region 包装容器包装信息

        //        var packContainerBarcodePackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
        //        {
        //            LadeBarCode = packContainerBarcodeEntity.BarCode,
        //            SiteId = siteId
        //        });

        //        #endregion

        //        #region 包装容器是否打开

        //        if (packContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16740));
        //        }

        //        #endregion

        //        #region 包装容器是否允许包装此序列码

        //        var packContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        //        {
        //            ContainerId = packContainerBarcodeEntity.ContainerId,
        //            SiteId = siteId
        //        });

        //        if (packContainerFreightEntities == null || packContainerFreightEntities.IsEmpty())
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16741));
        //        }

        //        var packContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == containerBarcodeEntity.ContainerId);
        //        if (packContainerFreightEntity == null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16742));
        //        }

        //        #endregion

        //        #region 包装容器是否超过容器最大装载数量                

        //        if (packContainerBarcodeEntity.Qty >= packContainerFreightEntity.Maximum)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16743));
        //        }

        //        #endregion

        //        process.ContainerBarcodeEntity = packContainerBarcodeEntity;
        //        process.InteContainerFreightEntity = packContainerFreightEntity;

        //        process.ContainerPackEntity = CreateContainerPackEntity(
        //            packContainerBarcodeEntity,
        //            //packContainerBarcodePackEntity,
        //            ManuContainerBarcodePackTypeEnum.Container,
        //            procedureId,
        //            resourceId,
        //            sfc,
        //            userName,
        //            siteId);

        //        process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(
        //            process.ContainerPackEntity,
        //            userName,
        //            siteId,
        //            ManuContainerPackRecordOperateTypeEnum.Load);

        //        process.DBExecuteMode = DBExecuteModeEnum.IncrementContainerBarcodeQty;
        //    }
        //    else
        //    {
        //        #region 包装容器是否允许包装此序列码

        //        var packContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        //        {
        //            ContainerId = manuFacePlateInfoEntity.ContainerId,
        //            SiteId = siteId
        //        });

        //        if (packContainerFreightEntities == null || packContainerFreightEntities.IsEmpty())
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16741));
        //        }

        //        var packContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == containerBarcodeEntity.ContainerId);
        //        if (packContainerFreightEntity == null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16742));
        //        }

        //        #endregion

        //        process.ContainerBarcodeEntity = manuFacePlateContainerBarcodeEntity;
        //        if (process.ContainerBarcodeEntity == null)
        //        {
        //            process.ContainerBarcodeEntity = await CreateContainerBarcodeEntity(manuFacePlateInfoEntity.ContainerId, userName, siteId);
        //            process.DBExecuteMode = DBExecuteModeEnum.NewContainerBarcode;
        //        }

        //        process.ContainerPackEntity = CreateContainerPackEntity(
        //            process.ContainerBarcodeEntity,
        //            //manuFacePlateContainerBarcodePackEntity,
        //            ManuContainerBarcodePackTypeEnum.Container,
        //            procedureId,
        //            resourceId,
        //            sfc,
        //            userName,
        //            siteId);

        //        process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(
        //            process.ContainerPackEntity,
        //            userName,
        //            siteId,
        //            ManuContainerPackRecordOperateTypeEnum.Load);
        //    }
        //}

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
