using FluentValidation;
using Google.Protobuf;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;

namespace Hymson.MES.Services.Services.Manufacture;

/// <summary>
/// 操作面板 服务
/// </summary>
public partial class ManuFacePlateService : IManuFacePlateService
{
    /// <summary>
    /// 容器条码执行模式
    /// </summary>
    private enum ExecuteModeEnum : byte
    {
        /// <summary>
        /// 不执行任何操作
        /// </summary>
        None,

        /// <summary>
        /// 新增容器条码
        /// </summary>
        NewContainerBarcode,

        /// <summary>
        /// 增加容器条码数量
        /// </summary>
        IncrementContainerBarcodeQty
    }

    /// <summary>
    /// 装箱流程控制
    /// </summary>
    private class ConfirmOnContainerPackPlateProcess
    {
        /// <summary>
        /// 包装面板实体
        /// </summary>
        public ManuFacePlateContainerPackEntity FacePlateContainerPackEntity { get; set; }

        /// <summary>
        /// 包装容器条码
        /// </summary>
        public ManuContainerBarcodeEntity ContainerBarcodeEntity { get; set; }

        /// <summary>
        /// 包装容器基础信息
        /// </summary>
        public InteContainerInfoEntity InteContainerInfoEntity { get; set; }

        /// <summary>
        /// 包装容器所有规格信息
        /// </summary>
        public IEnumerable<InteContainerFreightEntity> InteContainerFreightEntities { get; set; } = Enumerable.Empty<InteContainerFreightEntity>();

        /// <summary>
        /// 包装容器检索出的规格信息
        /// </summary>
        public InteContainerFreightEntity? InteContainerFreightEntity { get; set; }

        /// <summary>
        /// 包装容器已包装的装载信息
        /// </summary>
        public IEnumerable<ManuContainerPackEntity> ContainerPackEntities { get; set; } = Enumerable.Empty<ManuContainerPackEntity>();

        /// <summary>
        /// 包装容器已包装的所有容器条码信息
        /// </summary>
        public IEnumerable<ManuContainerBarcodeEntity> ContainerBarcodeEntities { get; set; } = Enumerable.Empty<ManuContainerBarcodeEntity>();

        /// <summary>
        /// 包装容器已包装的所有生产条码信息
        /// </summary>
        public IEnumerable<ManuSfcEntity> ManuSfcEntities { get; set; } = Enumerable.Empty<ManuSfcEntity>();

        /// <summary>
        /// 包装容器已包装的生产条码详细信息
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> ManuSfcInfoEntities { get; set; } = Enumerable.Empty<ManuSfcInfoEntity>();

        /// <summary>
        /// 包装容器已包装的生产条码的工单信息
        /// </summary>
        public IEnumerable<PlanWorkOrderEntity> PlanWorkOrderEntities { get; set; } = Enumerable.Empty<PlanWorkOrderEntity>();

        /// <summary>
        /// 包装容器已包装的生产条码的产品信息
        /// </summary>
        public IEnumerable<ProcMaterialEntity> ProcMaterialEntities { get; set; } = Enumerable.Empty<ProcMaterialEntity>();

        /// <summary>
        /// 预包装的生产条码实体
        /// </summary>
        public ManuSfcEntity ManuSfcEntityByInsertion { get; set; }

        /// <summary>
        /// 预包装的生产条码的信息实体
        /// </summary>
        public ManuSfcInfoEntity ManuSfcInfoEntityByInsertion { get; set; }

        /// <summary>
        /// 预包装的生产条码的工单信息
        /// </summary>
        public PlanWorkOrderEntity PlanWorkOrderEntityByInsertion { get; set; }

        /// <summary>
        /// 预包装生产条码的产品信息
        /// </summary>
        public ProcMaterialEntity ProcMaterialEntityByInsertion { get; set; }

        /// <summary>
        /// 预包装的包装容器条码实体
        /// </summary>
        public ManuContainerBarcodeEntity ContainerBarcodeEntityByPacked { get; set; }

        /// <summary>
        /// 预包装的包装容器的所有装载信息
        /// </summary>
        public IEnumerable<ManuContainerPackEntity> ContainerPackEntitiesByPacked { get; set; } = Enumerable.Empty<ManuContainerPackEntity>();

        /// <summary>
        /// 预包装的包装容器的生产条码信息
        /// </summary>
        public IEnumerable<ManuSfcEntity> ManuSfcEntitiesByPacked { get; set; } = Enumerable.Empty<ManuSfcEntity>();

        /// <summary>
        /// 预包装的包装容器的生产条码详细信息
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> ManuSfcInfoEntitiesByPacked { get; set; } = Enumerable.Empty<ManuSfcInfoEntity>();

        /// <summary>
        /// 预包装的包装容器已包装的生产条码的工单信息
        /// </summary>
        public IEnumerable<PlanWorkOrderEntity> PlanWorkOrderEntitiesByPacked { get; set; } = Enumerable.Empty<PlanWorkOrderEntity>();

        /// <summary>
        /// 预包装的包装容器已包装的生产条码的产品信息
        /// </summary>
        public IEnumerable<ProcMaterialEntity> ProcMaterialEntitiesByPacked { get; set; } = Enumerable.Empty<ProcMaterialEntity>();

        /// <summary>
        /// 预插入的装载信息
        /// </summary>
        public ManuContainerPackEntity ContainerPackEntity { get; set; }

        /// <summary>
        /// 预插入的记录信息
        /// </summary>
        public ManuContainerPackRecordEntity ContainerPackRecordEntity { get; set; }

        /// <summary>
        /// 执行模式
        /// </summary>
        public ExecuteModeEnum? ExecuteMode { get; set; } = ExecuteModeEnum.None;
    }

    /// <summary>
    /// 容器装载记录实体创建
    /// </summary>
    /// <param name="manuContainerPackEntity"></param>
    /// <param name="userName"></param>
    /// <param name="siteId"></param>
    /// <param name="operateType"></param>
    /// <returns></returns>
    private static ManuContainerPackRecordEntity CreateContainerPackRecordEntity(ManuContainerPackEntity manuContainerPackEntity, string userName, long siteId, ManuContainerPackRecordOperateTypeEnum operateType)
    {
        var result = new ManuContainerPackRecordEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            ProcedureId = manuContainerPackEntity.ProcedureId,
            ResourceId = manuContainerPackEntity.ResourceId,
            ContainerBarCodeId = manuContainerPackEntity.ContainerBarCodeId,
            LadeBarCode = manuContainerPackEntity.LadeBarCode,
            OperateType = operateType,
            CreatedBy = userName,
            CreatedOn = HymsonClock.Now(),
            SiteId = siteId
        };

        result.UpdatedOn = result.CreatedOn;
        result.UpdatedBy = result.CreatedBy;

        return result;
    }

    /// <summary>
    /// 容器装载实体创建
    /// </summary>
    /// <param name="containerBarcodeEntity"></param>
    /// <param name="manuContainerBarcodePackTypeEnum"></param>
    /// <param name="procedureId"></param>
    /// <param name="resourceId"></param>
    /// <param name="ladeBarCode"></param>
    /// <param name="userName"></param>
    /// <param name="siteId"></param>
    /// <returns></returns>
    private static ManuContainerPackEntity CreateContainerPackEntity(
        ManuContainerBarcodeEntity containerBarcodeEntity,
        ManuContainerBarcodePackTypeEnum manuContainerBarcodePackTypeEnum,
        long procedureId,
        long resourceId,
        string ladeBarCode,
        string userName,
        long siteId)
    {
        var containerPackEntity = new ManuContainerPackEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            ProcedureId = procedureId,
            ResourceId = resourceId,
            ContainerBarCodeId = containerBarcodeEntity.Id,
            LadeBarCode = ladeBarCode,
            PackType = manuContainerBarcodePackTypeEnum,
            CreatedBy = userName,
            CreatedOn = HymsonClock.Now(),
            SiteId = siteId
        };

        containerPackEntity.UpdatedOn = containerPackEntity.CreatedOn;
        containerPackEntity.UpdatedBy = containerPackEntity.CreatedBy;

        return containerPackEntity;
    }

    /// <summary>
    /// 包装容器条码创建
    /// </summary>
    /// <param name="containerId"></param>
    /// <param name="containerCode"></param>
    /// <param name="userName"></param>
    /// <param name="siteId"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    private async Task<ManuContainerBarcodeEntity> CreatedContainerBarcodeEntityAsync(
        long containerId,
        string containerCode,
        string userName,
        long siteId)
    {
        var inteCodeRuleEntity = await _inteCodeRulesRepository.GetOneAsync(new InteCodeRulesReQuery
        {
            ContainerInfoId = containerId,
            SiteId = siteId
        });

        if (inteCodeRuleEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16756)).WithData("code", containerCode);
        }

        var codes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
        {
            SiteId = siteId,
            UserName = userName,
            CodeRuleId = inteCodeRuleEntity.Id,
            Count = 1
        });

        var code = codes.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16757));
        }

        var containerBarcodeEntity = new ManuContainerBarcodeEntity
        {
            Id = IdGenProvider.Instance.CreateId(),
            ContainerId = containerId,
            Status = ManuContainerBarcodeStatusEnum.Open,
            Qty = 1,
            BarCode = code,
            IsDeleted = 0,
            CreatedBy = userName,
            CreatedOn = HymsonClock.Now(),
            SiteId = siteId
        };

        containerBarcodeEntity.UpdatedBy = containerBarcodeEntity.CreatedBy;
        containerBarcodeEntity.UpdatedOn = containerBarcodeEntity.CreatedOn;

        return containerBarcodeEntity;
    }

    /// <summary>
    /// 深度查找生产条码
    /// </summary>
    /// <param name="manuContainerPackEntities"></param>
    /// <param name="siteId"></param>
    /// <returns></returns>
    private async Task<IEnumerable<ManuContainerPackEntity>> DeepSearchManuSfcAsync(IEnumerable<ManuContainerPackEntity> manuContainerPackEntities, long siteId)
    {
        var result = Enumerable.Empty<ManuContainerPackEntity>();

        var manuContainerPackCodes = manuContainerPackEntities.Select(m => m.LadeBarCode);

        var manuContainerBarcodeEntities = await _manuContainerBarcodeRepository.GetListAsync(new ManuContainerBarcodeQuery
        {
            BarCodes = manuContainerPackCodes,
            SiteId = siteId
        });
        var manuContainerBarcodeIds = manuContainerBarcodeEntities.Select(m => m.Id);

        var nextManuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
        {
            ContainerBarCodeIds = manuContainerBarcodeIds,
            SiteId = siteId
        });

        if (!nextManuContainerPackEntities.Any())
        {
            return result;
        }
        else if (nextManuContainerPackEntities.Any(m => m.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc))
        {
            result = nextManuContainerPackEntities;
        }
        else
        {
            result = await DeepSearchManuSfcAsync(nextManuContainerPackEntities, siteId);
        }

        return result;
    }

    /// <summary>
    /// 验证产品条码
    /// </summary>
    /// <param name="process"></param>
    /// <exception cref="CustomerValidationException"></exception>
    private void ValidatedByManuSfcPacked(ConfirmOnContainerPackPlateProcess process)
    {
        #region 检验条码状态

        if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Scrapping)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11404)).WithData("sfc", process.ManuSfcEntityByInsertion.SFC);
        }

        if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Invalid)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11411)).WithData("sfc", process.ManuSfcEntityByInsertion.SFC);
        }

        if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Locked)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11405)).WithData("sfc", process.ManuSfcEntityByInsertion.SFC);
        }

        if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Delete)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11412)).WithData("sfc", process.ManuSfcEntityByInsertion.SFC);
        }

        #endregion

        #region 包装容器是否打开

        if (process.ContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16740));
        }

        #endregion

        #region 包装容器是否允许包装此序列码

        process.InteContainerFreightEntity = process.InteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == process.ProcMaterialEntityByInsertion.Id);
        if (process.InteContainerFreightEntity == null)
        {
            process.InteContainerFreightEntity = process.InteContainerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == process.ProcMaterialEntityByInsertion.GroupId);
        }

        if (process.InteContainerFreightEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16780)).WithData("code", process.ManuSfcEntityByInsertion.SFC);
        }

        #endregion

        #region 包装容器是否超过容器最大装载数量                

        if (process.ContainerBarcodeEntity.Qty >= process.InteContainerFreightEntity.Maximum)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16743));
        }

        #endregion

        #region 包装容器是否存在混装

        if (process.ContainerPackEntities.Any(m => m.PackType == ManuContainerBarcodePackTypeEnum.Container))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16773));
        }

        if (process.ProcMaterialEntities.Any(m => !m.MaterialCode.Equals(process.ProcMaterialEntityByInsertion.MaterialCode)))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16773));
        }

        #endregion

        #region 配置验证

        #region 是否允许混工单

        if (!process.FacePlateContainerPackEntity.IsMixedWorkOrder)
        {
            if (process.ManuSfcInfoEntities.Any(m => m.WorkOrderId != process.PlanWorkOrderEntityByInsertion.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16762));
            }
        }

        #endregion

        #region 是否允许完成产品

        if (!process.FacePlateContainerPackEntity.IsAllowCompleteProduct)
        {
            if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Complete)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16764));
            }
        }

        #endregion

        #region 是否允许排队产品

        if (!process.FacePlateContainerPackEntity.IsAllowQueueProduct)
        {
            if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.lineUp)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16765));
            }
        }

        #endregion

        #region 是否允许活动产品

        if (!process.FacePlateContainerPackEntity.IsAllowActiveProduct)
        {
            if (process.ManuSfcEntityByInsertion.Status == Core.Enums.SfcStatusEnum.Activity)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16763));
            }
        }

        #endregion

        #region 是否区分版本

        if (!process.FacePlateContainerPackEntity.IsAllowDifferentMaterial)
        {
            if (process.ProcMaterialEntities.Any(m => m.Id != process.ProcMaterialEntityByInsertion.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16766));
            }
        }
        else
        {
            var _procMaterialEntity = process.ProcMaterialEntities.FirstOrDefault(m => m.MaterialCode.Equals(process.ProcMaterialEntityByInsertion.MaterialCode)
                && (!string.IsNullOrWhiteSpace(m.Version)
                && !m.Version.Equals(process.ProcMaterialEntityByInsertion.Version)));

            if (_procMaterialEntity != null)
            {
                #region 不同版本、同物料的容器规格，装载最大值和最小值要相等

                var insertContainerFreightEntity = process.InteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == process.ProcMaterialEntityByInsertion.Id);
                var compareContainerFreightEntity = process.InteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == _procMaterialEntity.Id);

                if (insertContainerFreightEntity != null && compareContainerFreightEntity != null)
                {
                    if (insertContainerFreightEntity.Maximum != compareContainerFreightEntity.Maximum || insertContainerFreightEntity.Minimum != compareContainerFreightEntity.Minimum)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16768));
                    }
                }

                #endregion
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 验证包装编码
    /// </summary>
    /// <param name="process"></param>
    /// <exception cref="CustomerValidationException"></exception>
    private void ValidatedByContainerPacked(ConfirmOnContainerPackPlateProcess process)
    {
        #region 被包装容器是否关闭

        if (process.ContainerBarcodeEntityByPacked.Status != ManuContainerBarcodeStatusEnum.Close)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16775));
        }

        #endregion

        #region 验证被包装容器是否存在已锁定条码

        if (process.ManuSfcEntitiesByPacked.Any(m => m.Status == Core.Enums.SfcStatusEnum.Locked))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16774));
        }

        #endregion        

        #region 包装容器是否打开

        if (process.ContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16740));
        }

        #endregion

        #region 包装容器是否允许包装此序列码

        process.InteContainerFreightEntity = process.InteContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == process.ContainerBarcodeEntityByPacked.ContainerId);
        if (process.InteContainerFreightEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16780)).WithData("code", process.ContainerBarcodeEntityByPacked.BarCode);
        }

        #endregion

        #region 包装容器是否超过容器最大装载数量                

        if (process.ContainerBarcodeEntity.Qty >= process.InteContainerFreightEntity.Maximum)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16743));
        }

        #endregion

        #region 包装容器是否存在混装

        if (process.ContainerPackEntities.Any(m => m.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16773));
        }

        if (process.ContainerBarcodeEntities.Any(m => m.ContainerId != process.ContainerBarcodeEntityByPacked.ContainerId))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16773));
        }

        #endregion

        #region 配置验证

        #region 是否允许混工单

        if (!process.FacePlateContainerPackEntity.IsMixedWorkOrder)
        {
            if (process.ManuSfcInfoEntitiesByPacked.DistinctBy(m => m.WorkOrderId).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16762));
            }
        }

        #endregion

        #region 是否允许完成产品

        if (!process.FacePlateContainerPackEntity.IsAllowCompleteProduct)
        {
            if (process.ManuSfcEntitiesByPacked.Any(m => m.Status == Core.Enums.SfcStatusEnum.Complete))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16764));
            }
        }

        #endregion

        #region 是否允许排队产品

        if (!process.FacePlateContainerPackEntity.IsAllowQueueProduct)
        {
            if (process.ManuSfcEntitiesByPacked.Any(m => m.Status == Core.Enums.SfcStatusEnum.lineUp))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16765));
            }
        }

        #endregion

        #region 是否允许活动产品

        if (!process.FacePlateContainerPackEntity.IsAllowActiveProduct)
        {
            if (process.ManuSfcEntitiesByPacked.Any(m => m.Status == Core.Enums.SfcStatusEnum.Activity))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16763));
            }
        }

        #endregion

        #region 是否区分版本

        if (!process.FacePlateContainerPackEntity.IsAllowDifferentMaterial)
        {
            if (process.ProcMaterialEntitiesByPacked.DistinctBy(m => m.MaterialCode).DistinctBy(m => m.Version).Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16766));
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 通过容器编码获取容器列表信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ManuFacePlateContainerOutputDto> GetContainerInfoListByContainerCodeAsync(ManuFacePlatePackDto input)
    {
        var result = new ManuFacePlateContainerOutputDto()
        {
            PackingContainerCode = input.PackContainerCode,
            ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>()
        };

        var siteId = _currentSite.SiteId.GetValueOrDefault();

        #region 容器条码信息

        ManuContainerBarcodeEntity manuContainerBarcodeEntity;

        if (string.IsNullOrWhiteSpace(input.PackContainerCode))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16779));
        }

        manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
            new ManuContainerBarcodeQuery
            {
                BarCode = input.PackContainerCode,
                SiteId = siteId
            });

        if (manuContainerBarcodeEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16746));
        }

        result.CurrentPackCount = manuContainerBarcodeEntity.Qty;
        result.PackingContainerCode = manuContainerBarcodeEntity.BarCode;
        result.PackingContainerId = manuContainerBarcodeEntity.Id;
        result.PackingContainerStatus = manuContainerBarcodeEntity.Status;

        #endregion

        #region 容器装载信息

        var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(
            new ManuContainerPackQuery
            {
                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                ProcedureId = input.ProcedureId,
                ResourceId = input.ResourceId,
                SiteId = siteId
            });

        var manuContainerPackManuSfcEntities = manuContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc);
        var manuContainerPackContainerEntities = manuContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.Container);

        #endregion

        #region 容器规格信息

        var inteContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = manuContainerBarcodeEntity.ContainerId,
            SiteId = siteId
        });

        #endregion

        #region 装载的是产品序列码

        if (manuContainerPackManuSfcEntities != null && manuContainerPackManuSfcEntities.Any())
        {
            var manuSfcs = manuContainerPackManuSfcEntities.Select(m => m.LadeBarCode);

            var manuSfcEntities = await _manuSfcRepository.GetListAsync(
            new ManuSfcQuery
            {
                SFCs = manuSfcs,
                SiteId = siteId
            });

            if (manuSfcEntities != null && manuSfcEntities.Any())
            {
                var manuSfcIds = manuSfcEntities.Select(m => m.Id).Distinct();
                var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcIds);

                #region 产品物料信息

                var productIds = manuSfcInfoEntities.Select(m => m.ProductId).Distinct();
                var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(productIds);

                #endregion

                #region 工单信息

                var planWorkOrderIds = manuSfcInfoEntities.Where(w => w.WorkOrderId != null).Select(m => m.WorkOrderId.GetValueOrDefault()).Distinct();
                var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(planWorkOrderIds);

                #endregion

                #region 容器规格信息

                var firstProcMaterialEntity = procMaterialEntities.FirstOrDefault();
                if (firstProcMaterialEntity != null)
                {
                    var inteContainerFreightEntity = inteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == firstProcMaterialEntity.Id);
                    if (inteContainerFreightEntity == null)
                    {
                        inteContainerFreightEntity = inteContainerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == firstProcMaterialEntity.GroupId);
                    }

                    if (inteContainerFreightEntity != null)
                    {
                        result.MaxPackCount = inteContainerFreightEntity.Maximum;
                        result.MinPackCount = inteContainerFreightEntity.Minimum;
                    }
                }

                #endregion

                foreach (var manuContainerPackEntity in manuContainerPackManuSfcEntities)
                {
                    var containerInfoOutputDto = new ManuFacePlateContainerInfoOutputDto
                    {
                        PackingId = manuContainerPackEntity.Id,
                        PackingContainerId = manuContainerBarcodeEntity.Id,
                        PackingContainerCode = manuContainerBarcodeEntity.BarCode
                    };

                    var _manuSfcEntity = manuSfcEntities.FirstOrDefault(manuSfcEntity => manuSfcEntity.SFC.Equals(manuContainerPackEntity.LadeBarCode));
                    if (_manuSfcEntity != null)
                    {
                        containerInfoOutputDto.SFC = _manuSfcEntity.SFC;

                        var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(m => m.SfcId == _manuSfcEntity.Id);
                        if (manuSfcInfoEntity != null)
                        {
                            var procMaterialEntity = procMaterialEntities.FirstOrDefault(m => m.Id == manuSfcInfoEntity.ProductId);
                            if (procMaterialEntity != null)
                            {
                                containerInfoOutputDto.MaterialId = procMaterialEntity.Id;
                                containerInfoOutputDto.MaterialCode = procMaterialEntity.MaterialCode;
                            }

                            var planWorkOrderEntity = planWorkOrderEntities.FirstOrDefault(m => m.Id == manuSfcInfoEntity.WorkOrderId);
                            if (planWorkOrderEntity != null)
                            {
                                containerInfoOutputDto.WorkOrderId = planWorkOrderEntity.Id;
                                containerInfoOutputDto.WorkOrderCode = planWorkOrderEntity.OrderCode;
                            }
                        }
                    }

                    result.ContainerInfoOutputDtos.Add(containerInfoOutputDto);
                }
            }
        }

        #endregion

        #region 装载的是容器

        if (manuContainerPackContainerEntities != null && manuContainerPackContainerEntities.Any())
        {
            foreach (var manuContainerPackContainerEntity in manuContainerPackContainerEntities)
            {
                result.ContainerInfoOutputDtos.Add(new ManuFacePlateContainerInfoOutputDto
                {
                    PackingContainerId = manuContainerBarcodeEntity.Id,
                    PackingContainerCode = manuContainerBarcodeEntity.BarCode,
                    PackingId = manuContainerPackContainerEntity.Id,
                    SFC = manuContainerPackContainerEntity.LadeBarCode
                });
            }
        }

        #endregion

        return result;
    }

    /// <summary>
    /// 装载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ManuFacePlateContainerOutputDto> PackContainerAsync(ManuFacePlatePackDto input)
    {
        await _validationsManuFacePlateConfirmByContainerCodeRules.ValidateAndThrowAsync(input);

        if (input.Sfcs == null || !input.Sfcs.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16772));
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();
        if (siteId == default)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10101));
        }

        var userName = _currentUser.UserName;

        var procedureId = input.ProcedureId.GetValueOrDefault();
        var resourceId = input.ResourceId.GetValueOrDefault();

        var result = new ManuFacePlateContainerOutputDto
        {
            ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>()
        };

        var process = new ConfirmOnContainerPackPlateProcess();

        var sfc = input.Sfcs.FirstOrDefault();
        if (sfc == null)
        {
            return result;
        }

        #region 假设是已装载过的条码

        var manuContainerPackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
        {
            LadeBarCode = sfc,
            SiteId = siteId
        });

        if (manuContainerPackEntity != null)
        {
            var containerBarCodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
            {
                Id = manuContainerPackEntity.ContainerBarCodeId,
                SiteId = siteId
            });

            result.PackingContainerId = containerBarCodeEntity.Id;
            result.PackingContainerCode = containerBarCodeEntity.BarCode;

            result.TipMessage = _localizationService.GetResource(nameof(ErrorCode.MES16781), sfc, containerBarCodeEntity.BarCode);

            return result;
        }

        #endregion

        #region 填充面板信息

        var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(
            new EntityByCodeQuery
            {
                Code = input.Code,
                Site = siteId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        process.FacePlateContainerPackEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id)
            ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        if (process.FacePlateContainerPackEntity.ContainerId == default)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES17258));
        }

        #endregion

        #region 填充容器信息

        if (process.FacePlateContainerPackEntity.ContainerId == default)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16776));
        }

        process.InteContainerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery
        {
            Id = process.FacePlateContainerPackEntity.ContainerId,
            SiteId = siteId
        });
        if (process.InteContainerInfoEntity.Status != Core.Enums.SysDataStatusEnum.Enable)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16782));
        }

        process.InteContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = process.InteContainerInfoEntity.Id,
            SiteId = siteId
        });

        #endregion

        #region 填充容器条码信息

        if (string.IsNullOrWhiteSpace(input.PackContainerCode))
        {
            process.ContainerBarcodeEntity = await CreatedContainerBarcodeEntityAsync(process.FacePlateContainerPackEntity.ContainerId, process.InteContainerInfoEntity.Code, userName, siteId);
            process.ExecuteMode = ExecuteModeEnum.NewContainerBarcode;
        }
        else
        {
            process.ContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
            {
                BarCode = input.PackContainerCode,
                SiteId = siteId
            });

            if (process.ContainerBarcodeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16777));
            }

            if (process.ContainerBarcodeEntity.Status == ManuContainerBarcodeStatusEnum.Close)
            {
                process.ContainerBarcodeEntity = await CreatedContainerBarcodeEntityAsync(process.ContainerBarcodeEntity.ContainerId, process.InteContainerInfoEntity.Code, userName, siteId);
                process.ExecuteMode = ExecuteModeEnum.NewContainerBarcode;
            }
            else
            {
                process.ContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
                {
                    ContainerBarCodeId = process.ContainerBarcodeEntity.Id,
                    SiteId = siteId
                });

                if (process.ContainerPackEntities.Any())
                {
                    var manuSfcPackEntities = process.ContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc);

                    var manuSfcPackEntitiesLadeBarCode = manuSfcPackEntities.Select(m => m.LadeBarCode);
                    process.ManuSfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                    {
                        SFCs = manuSfcPackEntitiesLadeBarCode,
                        SiteId = siteId
                    });
                    var manuSfcIds = process.ManuSfcEntities.Select(m => m.Id);

                    process.ManuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcIds);

                    var procIds = process.ManuSfcInfoEntities.Select(m => m.ProductId);
                    var workOrderIds = process.ManuSfcInfoEntities.Select(m => m.WorkOrderId.GetValueOrDefault());

                    process.ProcMaterialEntities = await _procMaterialRepository.GetByIdsAsync(procIds);
                    process.PlanWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

                    var containerPackEntities = process.ContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.Container);
                    var containerBarcodes = containerPackEntities.Select(m => m.LadeBarCode);

                    process.ContainerBarcodeEntities = await _manuContainerBarcodeRepository.GetListAsync(new ManuContainerBarcodeQuery
                    {
                        BarCodes = containerBarcodes,
                        SiteId = siteId
                    });
                }

                process.ExecuteMode = ExecuteModeEnum.IncrementContainerBarcodeQty;
            }
        }

        #endregion

        #region 填充生产条码信息

        process.ManuSfcEntityByInsertion = await _manuSfcRepository.GetOneAsync(new ManuSfcQuery
        {
            SFC = sfc,
            SiteId = siteId
        });

        if (process.ManuSfcEntityByInsertion != null)
        {
            process.ManuSfcInfoEntityByInsertion = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(process.ManuSfcEntityByInsertion.Id);
            process.ProcMaterialEntityByInsertion = await _procMaterialRepository.GetByIdAsync(process.ManuSfcInfoEntityByInsertion.ProductId);
            process.PlanWorkOrderEntityByInsertion = await _planWorkOrderRepository.GetByIdAsync(process.ManuSfcInfoEntityByInsertion.WorkOrderId.GetValueOrDefault());
        }

        #endregion

        #region 填充容器条码信息

        if (process.ManuSfcEntityByInsertion == null)
        {
            process.ContainerBarcodeEntityByPacked = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
            {
                BarCode = sfc,
                SiteId = siteId
            });

            if (process.ContainerBarcodeEntityByPacked == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16744));
            }

            var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
            {
                ContainerBarCodeId = process.ContainerBarcodeEntityByPacked.Id,
                SiteId = siteId
            });

            process.ContainerPackEntitiesByPacked = await DeepSearchManuSfcAsync(manuContainerPackEntities, siteId);

            var containerPackSfcs = process.ContainerPackEntitiesByPacked.Select(m => m.LadeBarCode);

            if (process.ContainerPackEntitiesByPacked.Any())
            {
                process.ManuSfcEntitiesByPacked = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                {
                    SFCs = containerPackSfcs,
                    SiteId = siteId
                });

                if (process.ManuSfcEntitiesByPacked.Any())
                {
                    var manuSfcIds = process.ManuSfcEntitiesByPacked.Select(m => m.Id);

                    process.ManuSfcInfoEntitiesByPacked = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcIds);

                    if (process.ManuSfcInfoEntitiesByPacked.Any())
                    {
                        var productIds = process.ManuSfcInfoEntitiesByPacked.Select(m => m.ProductId);
                        var workOrderIds = process.ManuSfcInfoEntitiesByPacked.Select(m => m.WorkOrderId.GetValueOrDefault());

                        process.ProcMaterialEntitiesByPacked = await _procMaterialRepository.GetByIdsAsync(productIds);
                        process.PlanWorkOrderEntitiesByPacked = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
                    }
                }
            }
        }

        #endregion

        #region 填充装箱信息

        if (process.ManuSfcEntityByInsertion != null)
        {
            ValidatedByManuSfcPacked(process);

            process.ContainerPackEntity = CreateContainerPackEntity(
                process.ContainerBarcodeEntity,
                ManuContainerBarcodePackTypeEnum.ManuSfc,
                procedureId,
                resourceId,
                sfc,
                userName,
                siteId);

            process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(
                process.ContainerPackEntity,
                userName,
                siteId,
                ManuContainerPackRecordOperateTypeEnum.Load);
        }
        else
        {
            ValidatedByContainerPacked(process);

            process.ContainerPackEntity = CreateContainerPackEntity(
                    process.ContainerBarcodeEntity,
                    ManuContainerBarcodePackTypeEnum.Container,
                    procedureId,
                    resourceId,
                    sfc,
                    userName,
                    siteId);

            process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(
                    process.ContainerPackEntity,
                    userName,
                    siteId,
                    ManuContainerPackRecordOperateTypeEnum.Load);
        }

        #endregion        

        #region 执行事务

        if (process.ExecuteMode != ExecuteModeEnum.None)
        {
            if (process.InteContainerFreightEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16778));
            }

            using var scope = TransactionHelper.GetTransactionScope();

            switch (process.ExecuteMode)
            {
                case ExecuteModeEnum.NewContainerBarcode:
                    await _manuContainerBarcodeRepository.InsertReAsync(process.ContainerBarcodeEntity);
                    break;

                case ExecuteModeEnum.IncrementContainerBarcodeQty:
                    var incrementResult = await _manuContainerBarcodeRepository.IncrementQtyAsync(
                        new IncrementQtyCommand
                        {
                            Id = process.ContainerBarcodeEntity.Id,
                            IncrementValue = 1,
                            MaxValue = process.InteContainerFreightEntity.Maximum.GetValueOrDefault()
                        });

                    if (incrementResult <= 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16755)).WithData("code", process.InteContainerInfoEntity.Code).WithData("sfc", sfc);
                    }
                    break;
            }

            var manuContainerPackInsertCount = await _manuContainerPackRepository.InsertIgnoreAsync(process.ContainerPackEntity);
            if (manuContainerPackInsertCount == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16769));
            }

            await _manuContainerPackRecordRepository.InsertAsync(process.ContainerPackRecordEntity);

            await _manuContainerBarcodeRepository.RefreshStatusAsync(new RefreshStatusCommand { Id = process.ContainerBarcodeEntity.Id, MaxValue = process.InteContainerFreightEntity.Maximum.GetValueOrDefault() });

            scope.Complete();

            result.ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>
            {
                new ManuFacePlateContainerInfoOutputDto() {
                    WorkOrderId = process.PlanWorkOrderEntityByInsertion?.Id,
                    WorkOrderCode = process.PlanWorkOrderEntityByInsertion?.OrderCode,
                    MaterialId = process.ProcMaterialEntityByInsertion?.Id,
                    MaterialCode = process.ProcMaterialEntityByInsertion?.MaterialCode,
                    PackingId = process.ContainerPackEntity?.Id,
                    PackingContainerId = process.ContainerBarcodeEntity?.ContainerId,
                    PackingContainerCode = process.ContainerBarcodeEntity?.BarCode,
                    SFC = sfc
                }
            };
        }

        #endregion

        result.PackingContainerId = process.ContainerBarcodeEntity?.Id;
        result.PackingContainerCode = process.ContainerBarcodeEntity?.BarCode;

        return result;
    }

    /// <summary>
    /// 移除装载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ManuFacePlateContainerRemoveOutputDto> RemoveContainerPackAsync(ManuFacePlateRemovePackedDto input)
    {
        if (input.PackedIds == null || !input.PackedIds.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10103));
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();

        var userName = _currentUser.UserName;

        var packedCount = input.PackedIds.Count();

        var result = new ManuFacePlateContainerRemoveOutputDto
        {
            ManuContainerPackIds = input.PackedIds
        };

        var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(
            new ManuContainerPackQuery
            {
                Ids = result.ManuContainerPackIds,
                SiteId = siteId
            });
        if (manuContainerPackEntities == null || !manuContainerPackEntities.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16754));
        }

        var theFirstManuContainerPackEntity = manuContainerPackEntities.FirstOrDefault();
        if (theFirstManuContainerPackEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16754));
        }

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(theFirstManuContainerPackEntity.ContainerBarCodeId.GetValueOrDefault());
        if (manuContainerBarcodeEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16754));
        }

        if(manuContainerBarcodeEntity.Status == ManuContainerBarcodeStatusEnum.Close)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16734));
        }

        var manuContainerPackRecordEntities = manuContainerPackEntities.Select(m =>
        {
            return CreateContainerPackRecordEntity(m, userName, siteId, ManuContainerPackRecordOperateTypeEnum.Remove);
        });

        using var scope = TransactionHelper.GetTransactionScope();

        var deleteCount = await _manuContainerPackRepository.DeleteTrueAsync(new DeleteCommand { Ids = input.PackedIds });
        if (deleteCount != packedCount)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16748));
        }

        //var updateOutermostContainerBarCodeAndDeepCount = await _manuContainerPackRepository.UpdateOutermostContainerBarCodeAndDeepAsync(updateOutermostContainerBarCodeAndDeepCommands);

        var insertCount = await _manuContainerPackRecordRepository.InsertsAsync(manuContainerPackRecordEntities);
        if (insertCount != packedCount)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16749));
        }

        await _manuContainerBarcodeRepository.RefreshQtyAsync(new RefreshQtyCommand { Id = manuContainerBarcodeEntity.Id });

        scope.Complete();

        return result;
    }

    /// <summary>
    /// 移除所有装载
    /// </summary>
    /// <returns></returns>
    public async Task<ManuFacePlateContainerRemoveOutputDto> RemoveAllContainerPackAsync(ManuFacePlateRemoveAllPackedDto input)
    {
        var result = new ManuFacePlateContainerRemoveOutputDto();

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(input.PackContainerId);
        if (manuContainerBarcodeEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16751));
        }

        if (manuContainerBarcodeEntity.Status == ManuContainerBarcodeStatusEnum.Close)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16734));
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();
        var userName = _currentUser.UserName;

        var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery { ContainerBarCodeId = manuContainerBarcodeEntity.Id, SiteId = siteId });
        if (manuContainerPackEntities == null || !manuContainerPackEntities.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16752));
        }

        var manuContainerPackIds = manuContainerPackEntities.Select(m => m.Id);
        var manuContainerPackCount = manuContainerPackIds.Count();

        result.ManuContainerPackIds = manuContainerPackIds;

        var manuContainerPackRecordEntities = manuContainerPackEntities.Select(m =>
        {
            return CreateContainerPackRecordEntity(m, userName, siteId, ManuContainerPackRecordOperateTypeEnum.Remove);
        });

        using var scope = TransactionHelper.GetTransactionScope();

        var deleteCount = await _manuContainerPackRepository.DeleteAllAsync(manuContainerBarcodeEntity.Id);
        if (deleteCount != manuContainerPackCount)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16753));
        }

        await _manuContainerPackRecordRepository.InsertsAsync(manuContainerPackRecordEntities);

        await _manuContainerBarcodeRepository.ClearQtyAsync(new ClearQtyCommand { Id = manuContainerBarcodeEntity.Id });

        scope.Complete();

        return result;
    }

    /// <summary>
    /// 打开容器
    /// </summary>
    /// <returns></returns>
    public async Task OpenPackContainerAsync(ManuFacePlateOpenContainerDto input)
    {
        var siteId = _currentSite.SiteId.GetValueOrDefault();

        #region 包装容器条码信息

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        {
            BarCode = input.packContainerCode,
            SiteId = siteId
        });

        if (manuContainerBarcodeEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16746));
        }

        #endregion

        #region 包装容器容器信息

        var containerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery
        {
            Id = manuContainerBarcodeEntity.ContainerId,
            SiteId = siteId
        });

        #endregion

        #region 包装容器规格信息

        var containerFreightEntities = await _inteContainerFreightRepository.GetListAsync(
            new InteContainerFreightQuery
            {
                ContainerId = manuContainerBarcodeEntity.ContainerId,
                SiteId = siteId
            });

        if (containerFreightEntities == null || !containerFreightEntities.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16741)).WithData("code", containerInfoEntity.Code);
        }

        #endregion

        #region 包装容器包装信息

        //var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
        //{
        //    ContainerBarCodeId = manuContainerBarcodeEntity.Id,
        //    SiteId = siteId
        //});

        #endregion

        #region 验证容器是否为最大包装数量

        //var firstManuContainerPackEntity = manuContainerPackEntities.FirstOrDefault();
        //if (firstManuContainerPackEntity != null)
        //{
        //    if (firstManuContainerPackEntity.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc)
        //    {
        //        var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
        //        {
        //            SFC = firstManuContainerPackEntity.LadeBarCode,
        //            SiteId = siteId
        //        });

        //        var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id);

        //        var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);

        //        var containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.MaterialId == procMaterialEntity.Id);
        //        if (containerFreightEntity == null)
        //        {
        //            containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == procMaterialEntity.GroupId);
        //        }

        //        if (containerFreightEntity == null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16760)).WithData("code", containerInfoEntity.Code);
        //        }

        //        if (manuContainerBarcodeEntity.Qty >= containerFreightEntity.Maximum)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16770)).WithData("code", manuSfcEntity.SFC);
        //        }
        //    }
        //    else if (firstManuContainerPackEntity.PackType == ManuContainerBarcodePackTypeEnum.Container)
        //    {
        //        var packedManuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
        //            new ManuContainerBarcodeQuery
        //            {
        //                BarCode = firstManuContainerPackEntity.LadeBarCode,
        //                SiteId = siteId
        //            });

        //        var containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.FreightContainerId == packedManuContainerBarcodeEntity.ContainerId);
        //        if (containerFreightEntity == null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16760)).WithData("code", containerInfoEntity.Code);
        //        }

        //        if (manuContainerBarcodeEntity.Qty >= containerFreightEntity.Maximum)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES16770)).WithData("code", packedManuContainerBarcodeEntity.BarCode);
        //        }
        //    }
        //}

        #endregion

        #region 执行事务

        using var scope = TransactionHelper.GetTransactionScope();

        var updateCount = await _manuContainerBarcodeRepository.ChangeContainerStatusAsync(
            new CloseContainerCommand
            {
                Id = manuContainerBarcodeEntity.Id,
                Status = ManuContainerBarcodeStatusEnum.Open,
                StatusCondition = ManuContainerBarcodeStatusEnum.Close
            });

        if (updateCount == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16771));
        }

        scope.Complete();

        #endregion
    }

    /// <summary>
    /// 关闭容器
    /// </summary>
    /// <returns></returns>
    public async Task ClosePackContainerAsync(ManuFacePlateCloseContainerDto input)
    {
        var siteId = _currentSite.SiteId.GetValueOrDefault();

        #region 包装容器条码信息

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        {
            BarCode = input.packContainerCode,
            SiteId = siteId
        });

        if (manuContainerBarcodeEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16746));
        }

        #endregion

        #region 包装容器容器信息

        var containerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery
        {
            Id = manuContainerBarcodeEntity.ContainerId,
            SiteId = siteId
        });

        #endregion

        #region 包装容器规格信息

        var containerFreightEntities = await _inteContainerFreightRepository.GetListAsync(
            new InteContainerFreightQuery
            {
                ContainerId = manuContainerBarcodeEntity.ContainerId,
                SiteId = siteId
            });

        if (containerFreightEntities == null || !containerFreightEntities.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16741)).WithData("code", containerInfoEntity.Code);
        }

        #endregion

        #region 包装容器包装信息

        var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
        {
            ContainerBarCodeId = manuContainerBarcodeEntity.Id,
            SiteId = siteId
        });

        #endregion

        #region 验证容器是否达到最小包装数量

        var firstManuContainerPackEntity = manuContainerPackEntities.FirstOrDefault() ?? throw new CustomerValidationException(nameof(ErrorCode.MES16759));

        if (firstManuContainerPackEntity.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc)
        {
            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
            {
                SFC = firstManuContainerPackEntity.LadeBarCode,
                SiteId = siteId
            });

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);

            var containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.MaterialId == procMaterialEntity.Id);
            if (containerFreightEntity == null)
            {
                containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == procMaterialEntity.GroupId);
            }

            if (containerFreightEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16760)).WithData("code", containerInfoEntity.Code);
            }

            if (manuContainerBarcodeEntity.Qty < containerFreightEntity.Minimum)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16761)).WithData("code", manuSfcEntity.SFC);
            }
        }
        else if (firstManuContainerPackEntity.PackType == ManuContainerBarcodePackTypeEnum.Container)
        {
            var packedManuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
                new ManuContainerBarcodeQuery
                {
                    BarCode = firstManuContainerPackEntity.LadeBarCode,
                    SiteId = siteId
                });

            var containerFreightEntity = containerFreightEntities.FirstOrDefault(m => m.FreightContainerId == packedManuContainerBarcodeEntity.ContainerId);
            if (containerFreightEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16760)).WithData("code", containerInfoEntity.Code);
            }

            if (manuContainerBarcodeEntity.Qty < containerFreightEntity.Minimum)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16761)).WithData("code", packedManuContainerBarcodeEntity.BarCode);
            }
        }

        #endregion

        using var scope = TransactionHelper.GetTransactionScope();

        var updateCount = await _manuContainerBarcodeRepository.ChangeContainerStatusAsync(
            new CloseContainerCommand
            {
                Id = manuContainerBarcodeEntity.Id,
                Status = ManuContainerBarcodeStatusEnum.Close,
                StatusCondition = ManuContainerBarcodeStatusEnum.Open
            });

        if (updateCount == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16758));
        }

        scope.Complete();

    }
}