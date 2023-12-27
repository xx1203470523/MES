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
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Security.Policy;

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
        /// 包装容器条码
        /// </summary>
        public ManuContainerBarcodeEntity ContainerBarcodeEntity { get; set; }

        /// <summary>
        /// 包装容器装载信息
        /// </summary>
        public ManuContainerPackEntity ContainerPackEntity { get; set; }

        /// <summary>
        /// 包装容器装载记录信息
        /// </summary>
        public ManuContainerPackRecordEntity ContainerPackRecordEntity { get; set; }

        /// <summary>
        /// 包装容器容器信息
        /// </summary>
        public InteContainerInfoEntity InteContainerInfoEntity { get; set; }

        /// <summary>
        /// 包装容器容器规格
        /// </summary>
        public InteContainerFreightEntity InteContainerFreightEntity { get; set; }

        /// <summary>
        /// 生产工单
        /// </summary>
        public PlanWorkOrderEntity PlanWorkOrderEntity { get; set; }

        /// <summary>
        /// 生产产品
        /// </summary>
        public ProcMaterialEntity ProcMaterialEntity { get; set; }

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
        //ManuContainerPackEntity? containerBarcodePackEntity,
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

        //if (containerBarcodePackEntity != null)
        //{
        //    if (containerBarcodePackEntity.OutermostContainerBarCodeId.HasValue)
        //    {
        //        containerPackEntity.OutermostContainerBarCodeId = containerBarcodePackEntity.OutermostContainerBarCodeId;
        //    }

        //    if (containerBarcodePackEntity.Deep.HasValue)
        //    {
        //        containerPackEntity.Deep = containerBarcodePackEntity.Deep + 1;
        //    }
        //}

        return containerPackEntity;
    }

    /// <summary>
    /// 容器条码实体创建
    /// </summary>
    /// <param name="containerEntityId"></param>
    /// <param name="containerCode"></param>
    /// <param name="userName"></param>
    /// <param name="siteId"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    private async Task<ManuContainerBarcodeEntity> CreateContainerBarcodeEntity(
        long containerEntityId,
        string containerCode,
        string userName,
        long siteId)
    {
        var inteCodeRuleEntity = await _inteCodeRulesRepository.GetOneAsync(new InteCodeRulesReQuery
        {
            ContainerInfoId = containerEntityId,
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
            ContainerId = containerEntityId,
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
    /// 规则检验
    /// </summary>
    /// <param name="manuFacePlateContainerPackEntity"></param>
    /// <param name="manuSfcEntity"></param>
    /// <param name="manuContainerBarcodeEntity"></param>
    /// <param name="planWorkOrderEntity"></param>
    /// <param name="procMaterialEntity"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    private async Task ValidatedTheRules(
        ManuFacePlateContainerPackEntity? manuFacePlateContainerPackEntity,
        ManuSfcEntity? manuSfcEntity,
        ManuContainerBarcodeEntity? manuContainerBarcodeEntity,
        PlanWorkOrderEntity? planWorkOrderEntity,
        ProcMaterialEntity? procMaterialEntity)
    {
        if (manuFacePlateContainerPackEntity == null)
        {
            return;
        }

        if (manuSfcEntity == null)
        {
            return;
        }

        if (manuContainerBarcodeEntity == null)
        {
            return;
        }

        if (planWorkOrderEntity == null)
        {
            return;
        }

        if (procMaterialEntity == null)
        {
            return;
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();

        var inteContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = manuContainerBarcodeEntity.ContainerId,
            SiteId = _currentSite.SiteId
        });

        var manuContainerPackEntities = await _manuContainerPackRepository.GetListAsync(new ManuContainerPackQuery
        {
            ContainerBarCodeId = manuContainerBarcodeEntity.Id,
            SiteId = siteId
        });
        var manuContainerPackSFCs = manuContainerPackEntities.Select(m => m.LadeBarCode);

        var manuSfcEntities = await _manuSfcRepository.GetBySFCsAsync(
            new EntityBySFCsQuery
            {
                SFCs = manuContainerPackSFCs,
                SiteId = siteId
            });
        var manuSfcIds = manuSfcEntities.Select(m => m.Id);

        var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSfcIds);
        var manuSfcInfoIds = manuSfcInfoEntities.Select(m => m.ProductId);

        var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(manuSfcInfoIds);

        #region 是否允许混工单

        if (!manuFacePlateContainerPackEntity.IsMixedWorkOrder)
        {
            if (manuSfcInfoEntities != null && manuSfcInfoEntities.Any())
            {
                if (manuSfcInfoEntities.Any(m => m.WorkOrderId != planWorkOrderEntity.Id))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16762));
                }
            }
        }

        #endregion

        #region 是否允许完成产品

        if (!manuFacePlateContainerPackEntity.IsAllowCompleteProduct)
        {
            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Complete)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16764));
            }
        }

        #endregion

        #region 是否允许排队产品

        if (!manuFacePlateContainerPackEntity.IsAllowQueueProduct)
        {
            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.lineUp)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16765));
            }
        }

        #endregion

        #region 是否允许活动产品

        if (!manuFacePlateContainerPackEntity.IsAllowActiveProduct)
        {
            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Activity)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16763));
            }
        }

        #endregion

        #region 是否区分版本



        if (manuFacePlateContainerPackEntity.IsAllowDifferentMaterial)
        {
            if (inteContainerFreightEntities != null && procMaterialEntities != null)
            {
                var _procMaterialEntity = procMaterialEntities.FirstOrDefault(m => m.MaterialCode.Equals(procMaterialEntity.MaterialCode) && (!string.IsNullOrWhiteSpace(m.Version) && !m.Version.Equals(procMaterialEntity.Version)));
                if (_procMaterialEntity != null)
                {
                    var inteContainerFreightEntity = inteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == procMaterialEntity.Id);
                    var compareInteContainerFreightEntity = inteContainerFreightEntities.FirstOrDefault(m => m.MaterialId == _procMaterialEntity.Id);

                    if (inteContainerFreightEntity != null && compareInteContainerFreightEntity != null)
                    {
                        if (inteContainerFreightEntity.Maximum != compareInteContainerFreightEntity.Maximum || inteContainerFreightEntity.Minimum != compareInteContainerFreightEntity.Minimum)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES16768));
                        }
                    }
                }
            }
        }
        else
        {
            if (procMaterialEntities != null)
            {
                if (procMaterialEntities.Any(m => string.IsNullOrWhiteSpace(m.Version) || (!string.IsNullOrWhiteSpace(m.Version) && !m.Version.Equals(procMaterialEntity.Version))))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16766));
                }
            }
        }

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

        #region 面板信息

        var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(
            new EntityByCodeQuery
            {
                Code = input.Code,
                Site = siteId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        var manuFacePlateInfoEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id)
            ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        #endregion

        #region 容器条码信息

        ManuContainerBarcodeEntity manuContainerBarcodeEntity;

        if (string.IsNullOrWhiteSpace(input.PackContainerCode))
        {
            manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
            new ManuContainerBarcodeQuery
            {
                ContainerId = manuFacePlateInfoEntity.ContainerId,
                SiteId = siteId
            });
        }
        else
        {
            manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
            new ManuContainerBarcodeQuery
            {
                BarCode = input.PackContainerCode,
                SiteId = siteId
            });
        }

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

        var manuContainerPackByManuSfcLadeBarCodes = manuContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc).Select(m => m.LadeBarCode);
        var manuContainerPackByContainerLadeBarCodes = manuContainerPackEntities.Where(m => m.PackType == ManuContainerBarcodePackTypeEnum.Container).Select(m => m.LadeBarCode);

        #endregion

        #region 容器规格信息

        var inteContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = manuContainerBarcodeEntity.ContainerId,
            SiteId = siteId
        });

        #endregion

        #region 装载的是产品序列码

        if (manuContainerPackByManuSfcLadeBarCodes != null && manuContainerPackByManuSfcLadeBarCodes.Any())
        {
            var manuSfcEntities = await _manuSfcRepository.GetListAsync(
            new ManuSfcQuery
            {
                SFCs = manuContainerPackByManuSfcLadeBarCodes,
                SiteId = siteId
            });

            if (manuSfcEntities != null && manuSfcEntities.Any())
            {
                var manuSfcIds = manuSfcEntities.Select(m => m.Id).Distinct();
                var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSfcIds);

                #region 产品物料信息

                var productIds = manuSfcInfoEntities.Select(m => m.ProductId).Distinct();
                var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(productIds);

                #endregion

                #region 工单信息

                var planWorkOrderIds = manuSfcInfoEntities.Select(m => m.WorkOrderId).Distinct();
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

                foreach (var manuContainerPackEntity in manuContainerPackEntities)
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

        if (manuContainerPackByContainerLadeBarCodes != null && manuContainerPackByContainerLadeBarCodes.Any())
        {
            var manuContainerBarcodeEntities = await _manuContainerBarcodeRepository.GetListAsync(
                new ManuContainerBarcodeQuery
                {
                    BarCodes = manuContainerPackByContainerLadeBarCodes,
                    SiteId = siteId
                });

            #region 容器规格信息

            var firstManuContainerBarcodeEntity = manuContainerBarcodeEntities.FirstOrDefault();
            if (firstManuContainerBarcodeEntity != null)
            {
                var inteContainerFreightEntity = inteContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == firstManuContainerBarcodeEntity.ContainerId);
                if (inteContainerFreightEntity != null)
                {
                    result.MaxPackCount = inteContainerFreightEntity.Maximum;
                    result.MinPackCount = inteContainerFreightEntity.Minimum;
                }
            }

            #endregion

            foreach (var manuContainerPackEntity in manuContainerPackEntities)
            {
                var containerInfoOutputDto = new ManuFacePlateContainerInfoOutputDto
                {
                    PackingId = manuContainerPackEntity.Id,
                    PackingContainerId = manuContainerBarcodeEntity.Id,
                    PackingContainerCode = manuContainerBarcodeEntity.BarCode
                };

                var _manuContainerBarcodeEntity = manuContainerBarcodeEntities.FirstOrDefault(manuContainerBarcodeEntity => manuContainerBarcodeEntity.BarCode.Equals(manuContainerPackEntity.LadeBarCode));
                if (_manuContainerBarcodeEntity != null)
                {
                    containerInfoOutputDto.SFC = _manuContainerBarcodeEntity.BarCode;
                }

                result.ContainerInfoOutputDtos.Add(containerInfoOutputDto);
            }
        }

        #endregion

        return result;
    }

    /// <summary>
    /// 容器装载
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

        #region 获取面板实体和面板信息实体

        var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(
            new EntityByCodeQuery
            {
                Code = input.Code,
                Site = siteId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        var manuFacePlateInfoEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id)
            ?? throw new CustomerValidationException(nameof(ErrorCode.MES17209));

        #endregion

        #region 获取面板绑定容器的容器特性

        var manuFacePlateContainerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(
            new InteContainerInfoQuery
            {
                Id = manuFacePlateInfoEntity.ContainerId,
                SiteId = siteId
            });

        process.InteContainerInfoEntity = manuFacePlateContainerInfoEntity;

        var manuFacePlateContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = manuFacePlateInfoEntity.ContainerId,
            SiteId = siteId
        });

        #endregion

        #region 获取面板容器条码实体

        var manuFacePlateContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
            new ManuContainerBarcodeQuery
            {
                ContainerId = manuFacePlateInfoEntity.ContainerId,
                SiteId = siteId
            });

        #endregion

        #region 获取生产序列码实体

        var sfc = input.Sfcs.FirstOrDefault();
        if (sfc == null)
        {
            return result;
        }

        var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
        {
            SFC = sfc,
            SiteId = siteId
        });

        #endregion

        if (manuSfcEntity != null)
        {
            #region 检验条码状态

            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11404)).WithData("sfc", manuSfcEntity.SFC);
            }

            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Invalid)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11411)).WithData("sfc", manuSfcEntity.SFC);
            }

            if (manuSfcEntity.Status == Core.Enums.SfcStatusEnum.Locked)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11405)).WithData("sfc", manuSfcEntity.SFC);
            }

            #endregion

            #region 获取序列码详细信息实体和序列码产品信息实体

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16915));

            process.ProcMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);
            process.PlanWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId);

            #endregion

            if (input.PackContainerId.HasValue && input.PackContainerId != 0)
            {
                #region 包装容器容器信息

                process.InteContainerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery
                {
                    Id = input.PackContainerId.GetValueOrDefault(),
                    SiteId = siteId
                });

                #endregion

                #region 包装容器条码信息

                var packContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
                    new ManuContainerBarcodeQuery
                    {
                        ContainerId = input.PackContainerId,
                        SiteId = siteId
                    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16746));

                #endregion

                #region 包装容器包装信息

                var packContainerBarcodePackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
                {
                    LadeBarCode = packContainerBarcodeEntity.BarCode,
                    SiteId = siteId
                });

                #endregion

                #region 配置验证

                await ValidatedTheRules(
                    manuFacePlateInfoEntity,
                    manuSfcEntity,
                    packContainerBarcodeEntity,
                    process.PlanWorkOrderEntity,
                    process.ProcMaterialEntity);

                #endregion

                #region 包装容器是否打开

                if (packContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16740));
                }

                #endregion

                #region 包装容器是否允许包装此序列码

                var packContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
                {
                    ContainerId = packContainerBarcodeEntity.ContainerId,
                    SiteId = siteId
                });

                if (packContainerFreightEntities == null || !packContainerFreightEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16742));
                }

                var inteContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.MaterialId == process.ProcMaterialEntity.Id);
                if (inteContainerFreightEntity == null)
                {
                    inteContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == process.ProcMaterialEntity.GroupId);
                }

                if (inteContainerFreightEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16742));
                }

                #endregion

                #region 包装容器是否超过容器最大装载数量                

                if (packContainerBarcodeEntity.Qty >= inteContainerFreightEntity.Maximum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16743));
                }

                #endregion

                process.ContainerBarcodeEntity = packContainerBarcodeEntity;
                process.InteContainerFreightEntity = inteContainerFreightEntity;

                process.ContainerPackEntity = CreateContainerPackEntity(packContainerBarcodeEntity,
                    //packContainerBarcodePackEntity,
                    ManuContainerBarcodePackTypeEnum.ManuSfc,
                    procedureId,
                    resourceId,
                    sfc,
                    userName,
                    siteId);
                process.ContainerPackRecordEntity = CreateContainerPackRecordEntity(process.ContainerPackEntity, userName, siteId, ManuContainerPackRecordOperateTypeEnum.Load);

                process.ExecuteMode = ExecuteModeEnum.IncrementContainerBarcodeQty;
            }
            else
            {
                #region 产品序列码的包装信息

                var sfcContainerPackEntity = await _manuContainerPackRepository.GetOneAsync(
                    new ManuContainerPackQuery
                    {
                        LadeBarCode = sfc,
                        SiteId = siteId
                    });

                #endregion

                if (sfcContainerPackEntity != null)
                {
                    var sfcContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(
                        new ManuContainerBarcodeQuery
                        {
                            Id = sfcContainerPackEntity.ContainerBarCodeId,
                            SiteId = siteId
                        });

                    if (sfcContainerBarcodeEntity.ContainerId == manuFacePlateInfoEntity.ContainerId)
                    {
                        #region 显示界面                        

                        result.ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>
                        {
                            new ManuFacePlateContainerInfoOutputDto()
                            {
                                WorkOrderId = process.PlanWorkOrderEntity.Id,
                                WorkOrderCode = process.PlanWorkOrderEntity.OrderCode,
                                MaterialId = process.ProcMaterialEntity.Id,
                                MaterialCode = process.ProcMaterialEntity.MaterialCode,
                                PackingId = sfcContainerPackEntity.Id,
                                PackingContainerId = sfcContainerBarcodeEntity.Id,
                                PackingContainerCode = sfcContainerBarcodeEntity.BarCode,
                                SFC = sfc
                            }
                        };

                        result.TipMessage = _localizationService.GetResource(nameof(ErrorCode.MES16769));

                        #endregion
                    }
                    else
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16721)).WithData("sfc", sfc).WithData("barcode", sfcContainerBarcodeEntity.BarCode);
                    }
                }
                else
                {
                    process.ContainerBarcodeEntity = manuFacePlateContainerBarcodeEntity;
                    if (process.ContainerBarcodeEntity == null)
                    {
                        process.ContainerBarcodeEntity = await CreateContainerBarcodeEntity(manuFacePlateInfoEntity.ContainerId, process.InteContainerInfoEntity.Code, userName, siteId);
                        process.ExecuteMode = ExecuteModeEnum.NewContainerBarcode;
                    }
                    else
                    {
                        process.ExecuteMode = ExecuteModeEnum.IncrementContainerBarcodeQty;
                    }

                    #region 配置验证

                    await ValidatedTheRules(
                        manuFacePlateInfoEntity,
                        manuSfcEntity,
                        process.ContainerBarcodeEntity,
                        process.PlanWorkOrderEntity,
                        process.ProcMaterialEntity);

                    #endregion

                    #region 容器是否打开

                    if (process.ContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16740));
                    }

                    #endregion

                    #region 包装容器是否允许包装此序列码

                    var inteContainerFreightEntity = manuFacePlateContainerFreightEntities.FirstOrDefault(m => m.MaterialId == process.ProcMaterialEntity.Id);
                    if (inteContainerFreightEntity == null)
                    {
                        inteContainerFreightEntity = manuFacePlateContainerFreightEntities.FirstOrDefault(m => m.MaterialGroupId == process.ProcMaterialEntity.GroupId);
                    }

                    if (inteContainerFreightEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16742));
                    }

                    process.InteContainerFreightEntity = inteContainerFreightEntity;

                    #endregion

                    #region 包装容器是否超过容器最大装载数量                

                    if (process.ContainerBarcodeEntity.Qty >= inteContainerFreightEntity.Maximum)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16743));
                    }

                    #endregion

                    process.ContainerPackEntity = CreateContainerPackEntity(
                        process.ContainerBarcodeEntity,
                        //manuFacePlateContainerBarcodePackEntity,
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
            }
        }
        else
        {
            #region 被包装容器的条码信息

            var packedContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
            {
                BarCode = sfc,
                SiteId = siteId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16744));

            #endregion

            #region 被包装容器是否打开

            if (packedContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16740));
            }

            #endregion

            if (input.PackContainerId.HasValue && input.PackContainerId != 0)
            {
                #region 包装容器容器信息

                process.InteContainerInfoEntity = await _inteContainerInfoRepository.GetOneAsync(new InteContainerInfoQuery
                {
                    Id = input.PackContainerId.GetValueOrDefault(),
                    SiteId = siteId
                });

                #endregion

                #region 包装容器条码信息

                var packContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
                {
                    ContainerId = input.PackContainerId,
                    SiteId = siteId
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16746));

                #endregion

                #region 包装容器包装信息

                var packContainerBarcodePackEntity = await _manuContainerPackRepository.GetOneAsync(new ManuContainerPackQuery
                {
                    LadeBarCode = packContainerBarcodeEntity.BarCode,
                    SiteId = siteId
                });

                #endregion

                #region 配置验证

                await ValidatedTheRules(
                        manuFacePlateInfoEntity,
                        manuSfcEntity,
                        packContainerBarcodeEntity,
                        process.PlanWorkOrderEntity,
                        process.ProcMaterialEntity);

                #endregion

                #region 包装容器是否打开

                if (packContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16740));
                }

                #endregion

                #region 包装容器是否允许包装此序列码

                var packContainerFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
                {
                    ContainerId = packContainerBarcodeEntity.ContainerId,
                    SiteId = siteId
                });

                if (packContainerFreightEntities == null || !packContainerFreightEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16741));
                }

                var packContainerFreightEntity = packContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == packedContainerBarcodeEntity.ContainerId);
                if (packContainerFreightEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16742));
                }

                #endregion

                #region 包装容器是否超过容器最大装载数量                

                if (packContainerBarcodeEntity.Qty >= packContainerFreightEntity.Maximum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16743));
                }

                #endregion

                process.ContainerBarcodeEntity = packContainerBarcodeEntity;
                process.InteContainerFreightEntity = packContainerFreightEntity;

                process.ContainerPackEntity = CreateContainerPackEntity(
                    packContainerBarcodeEntity,
                    //packContainerBarcodePackEntity,
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

                process.ExecuteMode = ExecuteModeEnum.IncrementContainerBarcodeQty;
            }
            else
            {
                process.ContainerBarcodeEntity = manuFacePlateContainerBarcodeEntity;
                if (process.ContainerBarcodeEntity == null)
                {
                    process.ContainerBarcodeEntity = await CreateContainerBarcodeEntity(manuFacePlateInfoEntity.ContainerId, process.InteContainerInfoEntity.Code, userName, siteId);
                    process.ExecuteMode = ExecuteModeEnum.NewContainerBarcode;
                }
                else
                {
                    process.ExecuteMode = ExecuteModeEnum.IncrementContainerBarcodeQty;
                }
                
                #region 配置验证

                await ValidatedTheRules(
                    manuFacePlateInfoEntity,
                    manuSfcEntity,
                    process.ContainerBarcodeEntity,
                    process.PlanWorkOrderEntity,
                    process.ProcMaterialEntity);

                #endregion

                #region 容器是否打开

                if (process.ContainerBarcodeEntity.Status != ManuContainerBarcodeStatusEnum.Open)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16740));
                }

                #endregion

                #region 包装容器是否允许包装此序列码

                var packContainerFreightEntity = manuFacePlateContainerFreightEntities.FirstOrDefault(m => m.FreightContainerId == packedContainerBarcodeEntity.ContainerId);
                if (packContainerFreightEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16742));
                }

                process.InteContainerFreightEntity = packContainerFreightEntity;

                #endregion

                #region 包装容器是否超过容器最大装载数量                

                if (process.ContainerBarcodeEntity.Qty >= packContainerFreightEntity.Maximum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16743));
                }

                #endregion

                process.ContainerPackEntity = CreateContainerPackEntity(
                    process.ContainerBarcodeEntity,
                    //manuFacePlateContainerBarcodePackEntity,
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
        }

        #region 执行事务

        if (process.ExecuteMode != ExecuteModeEnum.None)
        {
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

            await _manuContainerPackRepository.InsertAsync(process.ContainerPackEntity);

            await _manuContainerPackRecordRepository.InsertAsync(process.ContainerPackRecordEntity);

            await _manuContainerBarcodeRepository.RefreshStatusAsync(new RefreshStatusCommand { Id = process.ContainerBarcodeEntity.Id, MaxValue = process.InteContainerFreightEntity.Maximum.GetValueOrDefault() });

            scope.Complete();

            result.ContainerInfoOutputDtos = new List<ManuFacePlateContainerInfoOutputDto>
            {
                new ManuFacePlateContainerInfoOutputDto() {
                    WorkOrderId = process.PlanWorkOrderEntity?.Id,
                    WorkOrderCode = process.PlanWorkOrderEntity?.OrderCode,
                    MaterialId = process.ProcMaterialEntity?.Id,
                    MaterialCode = process.ProcMaterialEntity?.MaterialCode,
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

        var firstManuContainerPackEntity = manuContainerPackEntities.FirstOrDefault();

        if (manuContainerPackEntities == null || !manuContainerPackEntities.Any())
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16754));
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

        if (firstManuContainerPackEntity != null)
        {
            await _manuContainerBarcodeRepository.RefreshQtyAsync(new RefreshQtyCommand { Id = firstManuContainerPackEntity.ContainerBarCodeId.GetValueOrDefault() });
        }

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
        if (!input.PackContainerId.HasValue || input.PackContainerId == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16750));
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();

        var packContainerId = input.PackContainerId.GetValueOrDefault();

        #region 包装容器条码信息

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        {
            Id = packContainerId,
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

        #region 验证容器是否为最大包装数量

        var firstManuContainerPackEntity = manuContainerPackEntities.FirstOrDefault();
        if (firstManuContainerPackEntity != null)
        {
            if (firstManuContainerPackEntity.PackType == ManuContainerBarcodePackTypeEnum.ManuSfc)
            {
                var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery
                {
                    SFC = firstManuContainerPackEntity.LadeBarCode,
                    SiteId = siteId
                });

                var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id);

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

                if (manuContainerBarcodeEntity.Qty >= containerFreightEntity.Maximum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16770)).WithData("code", manuSfcEntity.SFC);
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

                if (manuContainerBarcodeEntity.Qty >= containerFreightEntity.Maximum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16770)).WithData("code", packedManuContainerBarcodeEntity.BarCode);
                }
            }            
        }

        #endregion

        #region 执行事务

        using var scope = TransactionHelper.GetTransactionScope();

        var updateCount = await _manuContainerBarcodeRepository.ChangeContainerStatusAsync(
            new CloseContainerCommand
            {
                Id = packContainerId,
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
        if (!input.PackContainerId.HasValue || input.PackContainerId == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES16750));
        }

        var siteId = _currentSite.SiteId.GetValueOrDefault();

        var packContainerId = input.PackContainerId.GetValueOrDefault();

        #region 包装容器条码信息

        var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetOneAsync(new ManuContainerBarcodeQuery
        {
            Id = packContainerId,
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

            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(manuSfcEntity.Id);

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
                Id = packContainerId,
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