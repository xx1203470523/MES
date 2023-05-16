/*
 *creator: Karl
 *
 *describe: 容器条码表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器条码表 服务
    /// </summary>
    public class ManuContainerBarcodeService : IManuContainerBarcodeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器条码表 仓储
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IInteContainerRepository _inteContainerRepository;
        // private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;
        // private readonly IManuSfcRepository _manuSfcRepository;
        // private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuContainerPackService _manuContainerPack;
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        private readonly IManuContainerPackRecordService _manuContainerPackRecordService;
        private readonly IManuFacePlateContainerPackRepository _manuFacePlateContainerPackRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        // private readonly AbstractValidator<ManuContainerBarcodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerBarcodeModifyDto> _validationModifyRules;
        private readonly AbstractValidator<CreateManuContainerBarcodeDto> _validationCreateManuContainerBarcodeRules;
        private readonly AbstractValidator<UpdateManuContainerBarcodeStatusDto> _validationUpdateStatusRules;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        //private readonly IManuCommonService _manuCommonService;

        public ManuContainerBarcodeService(ICurrentUser currentUser, ICurrentSite currentSite, IManuContainerBarcodeRepository manuContainerBarcodeRepository
            //, AbstractValidator<ManuContainerBarcodeCreateDto> validationCreateRules
            , AbstractValidator<ManuContainerBarcodeModifyDto> validationModifyRules
            , IManuContainerPackRepository manuContainerPackRepository
            , IInteContainerRepository ingiContainerRepository
            // , IManuContainerPackRecordRepository manuContainerPackRecordRepository
            // , IManuSfcRepository manuSfcRepository
            //, IManuSfcInfoRepository manuSfcInfoRepository
            , IInteCodeRulesRepository inteCodeRulesRepository
            , IManuGenerateBarcodeService manuGenerateBarcodeService
            , IProcMaterialRepository procMaterialRepository
            , IPlanWorkOrderRepository planWorkOrderRepository
            , IManuContainerPackService manuContainerPack
            , IManuFacePlateRepository manuFacePlateRepository
            , IManuFacePlateContainerPackRepository manuFacePlateContainerPackRepository
            , IProcProcedureRepository procProcedureRepository
            , IManuContainerPackRecordService manuContainerPackRecordService
            , AbstractValidator<CreateManuContainerBarcodeDto> validationCreateManuContainerBarcodeRules,
             AbstractValidator<UpdateManuContainerBarcodeStatusDto> validationUpdateStatusRules
             , IManuSfcProduceRepository manuSfcProduceRepository
            // IManuCommonService manuCommonService
            , IProcResourceRepository procResourceRepository,
           IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            // _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _manuContainerPackRepository = manuContainerPackRepository;
            _inteContainerRepository = ingiContainerRepository;
            // _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
            // _manuSfcRepository = manuSfcRepository;
            // _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuContainerPack = manuContainerPack;
            _manuContainerPackRecordService = manuContainerPackRecordService;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _validationCreateManuContainerBarcodeRules = validationCreateManuContainerBarcodeRules;
            _validationUpdateStatusRules = validationUpdateStatusRules;
            _manuFacePlateRepository = manuFacePlateRepository;
            _manuFacePlateContainerPackRepository = manuFacePlateContainerPackRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procProcedureRepository = procProcedureRepository;
            //_manuCommonService = manuCommonService;
            _procResourceRepository = procResourceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createManuContainerBarcodeDto"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeView> CreateManuContainerBarcodeAsync(CreateManuContainerBarcodeDto createManuContainerBarcodeDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateManuContainerBarcodeRules.ValidateAndThrowAsync(createManuContainerBarcodeDto);

            //DTO转换实体
            var manuContainerBarcodeEntity = createManuContainerBarcodeDto.ToEntity<ManuContainerBarcodeEntity>();
            manuContainerBarcodeEntity.Id = IdGenProvider.Instance.CreateId();
            manuContainerBarcodeEntity.CreatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.CreatedOn = HymsonClock.Now();
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
            manuContainerBarcodeEntity.SiteId = _currentSite.SiteId ?? 0;

            return await CreatePackage(createManuContainerBarcodeDto, manuContainerBarcodeEntity);

        }
        private async Task<ManuContainerBarcodeView> CreatePackage(CreateManuContainerBarcodeDto createManuContainerBarcodeDto, ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {
            //工序信息
            var procobj = await _procProcedureRepository.GetByIdAsync(createManuContainerBarcodeDto.ProcedureId);
            if (procobj == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES16714));
            //测试提出资源对应类型和工序对应类型不一致验证
            if (createManuContainerBarcodeDto.ResourceId.HasValue)
            {
                var resource = await _procResourceRepository.GetResByIdAsync(createManuContainerBarcodeDto.ResourceId.Value);
                if (resource == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16724));
                }
                if (procobj.ResourceTypeId != resource.ResTypeId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16725));
                }
            }

            //获取面板信息
            var facePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery {Code= createManuContainerBarcodeDto.FacePlateCode,Site=_currentSite.SiteId } );
            if (facePlateEntity == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES16705));
            var facePlateContainerPackEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(facePlateEntity.Id);
            facePlateContainerPackEntity.ProcedureId = createManuContainerBarcodeDto.ProcedureId;
            if (procobj.PackingLevel == (int)LevelEnum.One)
            {
                return await CreateFirstPackage(createManuContainerBarcodeDto, facePlateContainerPackEntity, manuContainerBarcodeEntity);
            }
            else if (procobj.PackingLevel == (int)LevelEnum.Two || procobj.PackingLevel == (int)LevelEnum.Three)
            {
                return await CreateSecondPackage(createManuContainerBarcodeDto, facePlateContainerPackEntity, manuContainerBarcodeEntity, procobj.PackingLevel.Value);
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16715));
            }
        }

        private async Task<ManuContainerBarcodeView> CreateFirstPackage(CreateManuContainerBarcodeDto createManuContainerBarcodeDto
            , ManuFacePlateContainerPackEntity facePlateContainerPackEntity, ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {

            //获取条码生产信息
            //var produceSFCobj = await _manuCommonService.GetProduceSFCAsync(createManuContainerBarcodeDto.BarCode);
            //var sfcProduceEntity = produceSFCobj.Item1;
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(createManuContainerBarcodeDto.BarCode);
            if (sfcProduceEntity != null)
            {
                //条码是否已报废
                if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes)
                    throw new CustomerValidationException(nameof(ErrorCode.MES16720));
                //是否允许活动产品
                if (sfcProduceEntity.Status == SfcProduceStatusEnum.Activity && !facePlateContainerPackEntity.IsAllowActiveProduct)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16711));
                }
                //是否允许完成产品
                //if (sfcProduceEntity.Status == SfcProduceStatusEnum.Complete && !facePlateContainerPackEntity.IsAllowCompleteProduct)
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES16712));
                //}
                //是否允许排队产品
                if (sfcProduceEntity.Status == SfcProduceStatusEnum.lineUp && !facePlateContainerPackEntity.IsAllowQueueProduct)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16713));
                }
            }
            else
            {
                //完成即入库
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId,
                    BarCode = createManuContainerBarcodeDto.BarCode
                });

                if (whMaterialInventoryEntity != null)
                {
                    if (!facePlateContainerPackEntity.IsAllowCompleteProduct)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16712));
                    }
                    sfcProduceEntity = new ManuSfcProduceEntity
                    {
                        WorkOrderId = 0,
                        SFC = createManuContainerBarcodeDto.BarCode,
                        ProductId = whMaterialInventoryEntity.MaterialId
                    };
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16701));
                }
            }
            // 获取锁状态
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                Sfc = sfcProduceEntity.SFC,
                BusinessType = ManuSfcProduceBusinessType.Lock
            });
            if (sfcProduceBusinessEntity != null)
            {
                sfcProduceBusinessEntity.VerifyProcedureLock(createManuContainerBarcodeDto.BarCode, facePlateContainerPackEntity.ProcedureId);
            }

            //获取物料信息
            var material = await _procMaterialRepository.GetByIdAsync(sfcProduceEntity.ProductId);
            if (material == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES10204));

            /*根据条码判定是否有包装记录
             * Y 返回 view 
             * N ，判定包装码是否为空，
             *        Y  根据条码查找打开着的包装码 返回view
             *        N  创建全新包装 返回这个包装码的view
             */
            var packQuery = new ManuContainerPackQuery
            {
                LadeBarCode = createManuContainerBarcodeDto.BarCode,
                SiteId = manuContainerBarcodeEntity.SiteId,
            };
            var foo = await _manuContainerPackRepository.GetByLadeBarCodeAsync(packQuery);
            if (foo != null)
            {
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId);
                throw new CustomerValidationException(nameof(ErrorCode.MES16721)).WithData("sfc", packQuery.LadeBarCode).WithData("barcode", barcodeobj?.BarCode ?? foo.ContainerBarCodeId.ToString());
            }
            else
            {
                //新条码&& 没有指定包装
                if (string.IsNullOrEmpty(createManuContainerBarcodeDto.ContainerCode))
                {
                    //查找相同产品ID及打开着的包装
                    //var barcodeobj = await _manuContainerBarcodeRepository.GetByMaterialCodeAsync(material.MaterialCode, (int)ManuContainerBarcodeStatusEnum.Open, 1);
                    //if (barcodeobj != null)
                    //{
                    //    if (barcodeobj.WorkOrderId != sfcProduceEntity.WorkOrderId)
                    //    {
                    //        if (!facePlateContainerPackEntity.IsMixedWorkOrder)
                    //        {
                    //            var planWorkOrder = await _planWorkOrderRepository.GetByIdsAsync(new[] { barcodeobj.WorkOrderId, sfcProduceEntity.WorkOrderId });
                    //            var barOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == barcodeobj.WorkOrderId)?.OrderCode ?? "";
                    //            var sfcOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == sfcProduceEntity.WorkOrderId)?.OrderCode ?? "";
                    //            throw new CustomerValidationException(nameof(ErrorCode.MES16706)).WithData("first", barOrderCode).WithData("second", sfcOrderCode);
                    //        }
                    //    }
                    //    //比较物料版本
                    //    if (material.Version != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                    //        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    //    var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                    //    var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                    //    if (inte.Maximum > packs.Count())
                    //    {
                    //        using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                    //        {
                    //            await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                    //            {
                    //                ResourceId = facePlateContainerPackEntity.ResourceId,
                    //                ProcedureId = barcodeobj.ProductId,
                    //                ContainerBarCodeId = barcodeobj.Id,
                    //                LadeBarCode = createManuContainerBarcodeDto.BarCode

                    //            });
                    //            await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                    //            {
                    //                ResourceId = facePlateContainerPackEntity.ResourceId,
                    //                ProcedureId = barcodeobj.ProductId,
                    //                ContainerBarCodeId = barcodeobj.Id,
                    //                OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                    //                LadeBarCode = createManuContainerBarcodeDto.BarCode

                    //            });
                    //            ts.Complete();
                    //        }
                    //        if (inte.Maximum == (packs.Count() + 1))
                    //        {
                    //            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                    //            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                    //        }
                    //        return await GetContainerPackView(sfcProduceEntity.WorkOrderId, material.Id, barcodeobj, true);
                    //    }
                    //    else
                    //    {
                    //        barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                    //        await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                    //        throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                    //        // return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material, facePlateContainerPackEntity);
                    //    }

                    //}
                    //else 
                    //{
                    //全新包装
                    return await CreateNewBarcode(manuContainerBarcodeEntity, sfcProduceEntity.ProductId, sfcProduceEntity.WorkOrderId, material, facePlateContainerPackEntity);
                    //}
                }
                else //新条码&& 指定包装
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  使用这个包装 
                     * N 创建全新包装
                     */

                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery { BarCode = createManuContainerBarcodeDto.ContainerCode, SiteId = manuContainerBarcodeEntity.SiteId });
                    if (barcodeobj.Status == (int)Core.Enums.Manufacture.ManuContainerBarcodeStatusEnum.Close || barcodeobj.IsDeleted == 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16722)).WithData("packId", barcodeobj.Id);
                    }

                    //物料不一样报错
                    if (material.MaterialCode != barcodeobj.MaterialCode)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16730));
                    }

                    if (barcodeobj.WorkOrderId != sfcProduceEntity.WorkOrderId)
                    {
                        if (!facePlateContainerPackEntity.IsMixedWorkOrder)
                        {
                            var planWorkOrder = await _planWorkOrderRepository.GetByIdsAsync(new[] { barcodeobj.WorkOrderId, sfcProduceEntity.WorkOrderId });
                            var barOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == barcodeobj.WorkOrderId)?.OrderCode ?? "";
                            var sfcOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == sfcProduceEntity.WorkOrderId)?.OrderCode ?? "";
                            throw new CustomerValidationException(nameof(ErrorCode.MES16706)).WithData("first", barOrderCode).WithData("second", sfcOrderCode);
                        }
                    }

                    //比较物料版本
                    if (material.Version != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    if ((facePlateContainerPackEntity.IsAllowDifferentMaterial && barcodeobj?.MaterialCode == material.MaterialCode) ||
                        ((!facePlateContainerPackEntity.IsAllowDifferentMaterial) && barcodeobj?.ProductId == sfcProduceEntity.ProductId))
                    {
                        //if (barcodeobj?.ProductId == sfcProduceEntity.ProductId)//相同包装
                        //{
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                        if (inte.Maximum > packs.Count())
                        {
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ResourceId = facePlateContainerPackEntity.ResourceId,
                                    ProcedureId = barcodeobj.ProductId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ResourceId = facePlateContainerPackEntity.ResourceId,
                                    ProcedureId = barcodeobj.ProductId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            if (inte.Maximum == (packs.Count() + 1))
                            {
                                barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                                await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            }
                            return await GetContainerPackView(sfcProduceEntity.WorkOrderId, material.Id, barcodeobj, true);
                        }
                        else
                        {
                            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                            // return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material, facePlateContainerPackEntity);
                        }
                    }
                    else
                    {
                        //不是相同包装，创建全新包装
                        return await CreateNewBarcode(manuContainerBarcodeEntity, sfcProduceEntity.ProductId, sfcProduceEntity.WorkOrderId, material, facePlateContainerPackEntity);
                    }
                }
            }
        }

        private async Task<ManuContainerBarcodeView> CreateSecondPackage(CreateManuContainerBarcodeDto createManuContainerBarcodeDto
            , ManuFacePlateContainerPackEntity facePlateContainerPackEntity, ManuContainerBarcodeEntity manuContainerBarcodeEntity, int level)
        {
            /*根据条码判定是否有包装记录
            * Y 返回 view 
            * N ，判定包装码是否为空，
            *        Y  根据条码查找打开着的包装码 返回view
            *        N  返回这个包装码的view
            */
            var packQuery = new ManuContainerPackQuery
            {
                LadeBarCode = createManuContainerBarcodeDto.BarCode,
                SiteId = manuContainerBarcodeEntity.SiteId,
            };
            var foo = await _manuContainerPackRepository.GetByLadeBarCodeAsync(packQuery);
            if (foo != null)
            {
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId);
                throw new CustomerValidationException(nameof(ErrorCode.MES16721)).WithData("sfc", packQuery.LadeBarCode).WithData("barcode", barcodeobj?.BarCode ?? foo.ContainerBarCodeId.ToString());
            }
            else
            {
                //子级包装对象
                var prebarcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery() { BarCode = createManuContainerBarcodeDto.BarCode, SiteId = manuContainerBarcodeEntity.SiteId });
                if (prebarcodeobj == null)
                    throw new CustomerValidationException(nameof(ErrorCode.MES16718));

                //判断容器是否已关闭，只有关闭的才能装箱
                if (prebarcodeobj.Status != (int)ManuContainerBarcodeStatusEnum.Close)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16729)).WithData("barcode", prebarcodeobj.BarCode);
                }

                if (prebarcodeobj.PackLevel == level && level == (int)LevelEnum.Two)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16727));
                }
                if (prebarcodeobj.PackLevel == level && level == (int)LevelEnum.Three)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16728));
                }

                //新条码&& 没有指定包装
                if (string.IsNullOrEmpty(createManuContainerBarcodeDto.ContainerCode))
                {
                    //查找相同产品ID及打开着的包装
                    //var barcodeobj = await _manuContainerBarcodeRepository.GetByMaterialCodeAsync(prebarcodeobj.MaterialCode, (int)ManuContainerBarcodeStatusEnum.Open, level);
                    //if (barcodeobj != null)
                    //{
                    //    if (barcodeobj.WorkOrderId != prebarcodeobj.WorkOrderId)
                    //    {
                    //        if (!facePlateContainerPackEntity.IsMixedWorkOrder)
                    //        {
                    //            var planWorkOrder = await _planWorkOrderRepository.GetByIdsAsync(new[] { barcodeobj.WorkOrderId, prebarcodeobj.WorkOrderId });
                    //            var barOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == barcodeobj.WorkOrderId)?.OrderCode ?? "";
                    //            var sfcOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == prebarcodeobj.WorkOrderId)?.OrderCode ?? "";
                    //            throw new CustomerValidationException(nameof(ErrorCode.MES16706)).WithData("first", barOrderCode).WithData("second", sfcOrderCode);
                    //        }
                    //    }
                    //    //比较物料版本
                    //    if (prebarcodeobj.MaterialVersion != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                    //        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    //    var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                    //    var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                    //    if (inte.Maximum > packs.Count())
                    //    {
                    //        using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                    //        {
                    //            await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                    //            {
                    //                ResourceId = facePlateContainerPackEntity.ResourceId,
                    //                ProcedureId = barcodeobj.ProductId,
                    //                ContainerBarCodeId = barcodeobj.Id,
                    //                LadeBarCode = createManuContainerBarcodeDto.BarCode

                    //            });
                    //            await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                    //            {
                    //                ResourceId = facePlateContainerPackEntity.ResourceId,
                    //                ProcedureId = barcodeobj.ProductId,
                    //                ContainerBarCodeId = barcodeobj.Id,
                    //                OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                    //                LadeBarCode = createManuContainerBarcodeDto.BarCode

                    //            });
                    //            ts.Complete();
                    //        }
                    //        if (inte.Maximum == (packs.Count() + 1))
                    //        {
                    //            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                    //            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                    //        }
                    //        return await GetContainerPackView(barcodeobj.WorkOrderId, barcodeobj.ProductId, barcodeobj);

                    //    }
                    //    else
                    //    {
                    //        barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                    //        await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                    //        throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                    //        //return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material, facePlateContainerPackEntity);
                    //    }

                    //}
                    //else
                    //{
                        //全新包装
                        //获取物料信息
                        var material = await _procMaterialRepository.GetByIdAsync(prebarcodeobj.ProductId);
                        return await CreateNewBarcode(manuContainerBarcodeEntity, prebarcodeobj.ProductId, prebarcodeobj.WorkOrderId, material, facePlateContainerPackEntity, level);
                    //}
                }
                else //新条码&& 指定包装
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  使用这个包装 
                     * N 创建全新包装
                     */

                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery { BarCode = createManuContainerBarcodeDto.ContainerCode, SiteId = _currentSite.SiteId ?? 0 });
                    //比较物料版本
                    if (prebarcodeobj.MaterialVersion != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    //物料不一样报错
                    if (barcodeobj.MaterialCode != prebarcodeobj.MaterialCode)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16730));
                    }

                    if (barcodeobj.WorkOrderId != prebarcodeobj.WorkOrderId)
                    {
                        if (!facePlateContainerPackEntity.IsMixedWorkOrder)
                        {
                            var planWorkOrder = await _planWorkOrderRepository.GetByIdsAsync(new[] { barcodeobj.WorkOrderId, prebarcodeobj.WorkOrderId });
                            var barOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == barcodeobj.WorkOrderId)?.OrderCode ?? "";
                            var sfcOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == prebarcodeobj.WorkOrderId)?.OrderCode ?? "";
                            throw new CustomerValidationException(nameof(ErrorCode.MES16706)).WithData("first", barOrderCode).WithData("second", sfcOrderCode);
                        }
                    }

                    if ((facePlateContainerPackEntity.IsAllowDifferentMaterial && barcodeobj?.MaterialCode == prebarcodeobj.MaterialCode) ||
                        ((!facePlateContainerPackEntity.IsAllowDifferentMaterial) && barcodeobj?.ProductId == prebarcodeobj.ProductId))
                    //if (barcodeobj?.ProductId == prebarcodeobj.ProductId)//相同包装
                    {
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                        if (inte.Maximum > packs.Count())
                        {
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ResourceId = facePlateContainerPackEntity.ResourceId,
                                    ProcedureId = barcodeobj.ProductId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ResourceId = facePlateContainerPackEntity.ResourceId,
                                    ProcedureId = barcodeobj.ProductId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            if (inte.Maximum == (packs.Count() + 1))
                            {
                                barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                                await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            }

                            return await GetContainerPackView(barcodeobj.WorkOrderId, barcodeobj.ProductId, barcodeobj);
                        }
                        else
                        {
                            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                            // return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material, facePlateContainerPackEntity);
                        }
                    }
                    else
                    {
                        //不是相同包装，创建全新包装
                        // var workorder = await _planWorkOrderRepository.GetByIdAsync(prebarcodeobj.WorkOrderId);
                        //获取物料信息
                        var material = await _procMaterialRepository.GetByIdAsync(prebarcodeobj.ProductId);
                        return await CreateNewBarcode(manuContainerBarcodeEntity, prebarcodeobj.ProductId, prebarcodeobj.WorkOrderId, material, facePlateContainerPackEntity, level);
                    }
                }
            }
        }

        private async Task<ManuContainerBarcodeView> CreateNewBarcode(ManuContainerBarcodeEntity manuContainerBarcodeEntity,
           long ProductId, long workorderId,
           ProcMaterialEntity material, ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity, int level = 1)
        {
            manuContainerBarcodeEntity.WorkOrderId = workorderId;
            manuContainerBarcodeEntity.MaterialVersion = material.Version ?? "9999—Unknow";
            manuContainerBarcodeEntity.MaterialCode = material.MaterialCode ?? "";
            manuContainerBarcodeEntity.PackLevel = level;
            bool isFirstPackage = level == (int)LevelEnum.One ? true : false;

            //判定  是物料-包装规格   OR 物料组-包装规格
            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
            {
                DefinitionMethod = DefinitionMethodEnum.Material,
                MaterialId = ProductId,
                MaterialGroupId = 0,
                Status = SysDataStatusEnum.Enable,
                Level = (LevelEnum)level
            });
            //物料-包装规格
            if (entityByRelation != null)
            {
                manuContainerBarcodeEntity.ContainerId = entityByRelation.Id;
                manuContainerBarcodeEntity.ProductId = ProductId;

                //包装等级转换 程序内控制传入
                var packType = (CodeRulePackTypeEnum)level;
                //根据编码类型，包装等级查询编码规则
                var inteCodeRulesResult = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery
                {
                    ProductId = ProductId,
                    CodeType = CodeRuleCodeTypeEnum.PackagingSeqCode,
                    PackType = packType
                });
                var inteCodeRulesEntity = inteCodeRulesResult.FirstOrDefault();
                if (inteCodeRulesEntity == null || inteCodeRulesEntity.ProductId != ProductId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", material?.MaterialCode ?? "");
                }
                var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
                {
                    CodeRuleId = inteCodeRulesEntity.Id,
                    Count = 1
                });
                string barcode = manuContainerBarcodeEntity.BarCode;
                manuContainerBarcodeEntity.BarCode = barcodeList.First();
                //创建包装
                using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                {
                    await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);
                    await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                    {
                        ResourceId = manuFacePlateContainerPackEntity.ResourceId,
                        ProcedureId = manuContainerBarcodeEntity.ProductId,
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        LadeBarCode = barcode

                    });
                    await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                    {
                        ResourceId = manuFacePlateContainerPackEntity.ResourceId,
                        ProcedureId = manuContainerBarcodeEntity.ProductId,
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                        LadeBarCode = barcode

                    });
                    ts.Complete();
                }

                return await GetContainerPackView(workorderId, material.Id, manuContainerBarcodeEntity, isFirstPackage, entityByRelation);

            }
            else //物料组-包装规格
            {
                if (material.GroupId != 0)
                {
                    var entityByRelation1 = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
                    {
                        DefinitionMethod = DefinitionMethodEnum.MaterialGroup,
                        MaterialId = ProductId,
                        MaterialGroupId = material.GroupId,
                        Status = SysDataStatusEnum.Enable,
                        Level = (LevelEnum)level
                    });
                    if (entityByRelation1 != null)
                    {
                        manuContainerBarcodeEntity.ContainerId = entityByRelation1.Id;
                        manuContainerBarcodeEntity.ProductId = ProductId;
                        var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
                        {
                            CodeRuleId = manuContainerBarcodeEntity.Id,
                            Count = 1
                        });
                        manuContainerBarcodeEntity.BarCode = barcodeList.First();
                        //入库
                        using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                        {
                            await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);
                            await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                            {
                                ResourceId = manuFacePlateContainerPackEntity.ResourceId,
                                ProcedureId = manuContainerBarcodeEntity.ProductId,
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode
                            });
                            await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                            {
                                ResourceId = manuFacePlateContainerPackEntity.ResourceId,
                                ProcedureId = manuContainerBarcodeEntity.ProductId,
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode

                            });
                            ts.Complete();
                        }


                        return await GetContainerPackView(workorderId, material.Id, manuContainerBarcodeEntity, isFirstPackage, entityByRelation1);
                    }
                    else
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16719));
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16719));
                }
            }
        }

        /// <summary>
        /// 获取包装清单
        /// </summary>
        /// <param name="workorderId"></param>
        /// <param name="materialId"></param>
        /// <param name="barcodeobj"></param>
        /// <param name="isFirstPackage"></param>
        /// <param name="inte"></param>
        /// <returns></returns>
        private async Task<ManuContainerBarcodeView> GetContainerPackView(long workorderId, long materialId, ManuContainerBarcodeEntity barcodeobj, bool isFirstPackage = false, InteContainerEntity inte = null)
        {
            if (inte == null)
                inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
            var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);//实际绑定集合

            var containerPackEntities = new List<ManuContainerPackEntity>();
            var packBarCodesEntities = new List<ManuContainerBarcodeEntity>();
            if (!isFirstPackage)
            {
                var barCodes = packs.Select(x => x.LadeBarCode).ToArray();
                packBarCodesEntities = (await _manuContainerBarcodeRepository.GetByCodesAsync(new ManuContainerBarcodeQuery
                {
                    BarCodes = barCodes,
                    SiteId = _currentSite.SiteId ?? 0
                })).ToList();

                if (packBarCodesEntities.Any())
                {
                    var packBarCodeIds = packBarCodesEntities.Select(x => x.Id).ToArray();
                    containerPackEntities = (await _manuContainerPackRepository.GetByContainerBarCodeIdsAsync(packBarCodeIds, _currentSite.SiteId ?? 0)).ToList();
                }
            }

            //转换ID为编码
            var procMateria = await _procMaterialRepository.GetByIdAsync(materialId);

            PlanWorkOrderEntity planWorkOrder = new PlanWorkOrderEntity();
            if (workorderId > 0)
            {
                planWorkOrder = await _planWorkOrderRepository.GetByIdAsync(workorderId);
            }

            ManuContainerBarcodeView view = new ManuContainerBarcodeView()
            {
                manuContainerBarcodeEntity = barcodeobj,
                inteContainerEntity = inte,
                manuContainerPacks = packs.Select<ManuContainerPackEntity, ManuContainerPackDto>(m =>
                {
                    var barCodeId = packBarCodesEntities.FirstOrDefault(x => x.BarCode == m.LadeBarCode)?.Id ?? 0;
                    var count = isFirstPackage ? 1 : containerPackEntities.Count(x => x.ContainerBarCodeId == barCodeId);
                    return new ManuContainerPackDto()
                    {
                        ContainerBarCodeId = m.ContainerBarCodeId,
                        CreatedBy = m.CreatedBy,
                        CreatedOn = m.CreatedOn,
                        Id = m.Id,
                        BarCode = barcodeobj.BarCode,
                        LadeBarCode = m.LadeBarCode,
                        //MaterialCode = materialId.ToString(),
                        //SiteId = m.SiteId,
                        //WorkOrderCode = workorderId.ToString(),
                        MaterialCode = procMateria?.MaterialCode ?? string.Empty,//如果关联结果删除直接返回空
                        SiteId = m.SiteId,
                        WorkOrderCode = planWorkOrder?.OrderCode ?? string.Empty,
                        Count = count
                    };
                }).ToList()
            };

            return view;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerBarcodeAsync(long id)
        {
            await _manuContainerBarcodeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuContainerBarcodeAsync(long[] ids)
        {
            return await _manuContainerBarcodeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerBarcodePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerBarcodeDto>> GetPagedListAsync(ManuContainerBarcodePagedQueryDto manuContainerBarcodePagedQueryDto)
        {
            var manuContainerBarcodePagedQuery = manuContainerBarcodePagedQueryDto.ToQuery<ManuContainerBarcodePagedQuery>();
            manuContainerBarcodePagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedInfoAsync(manuContainerBarcodePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuContainerBarcodeDto> manuContainerBarcodeDtos = PrepareManuContainerBarcodeDtos(pagedInfo);
            return new PagedInfo<ManuContainerBarcodeDto>(manuContainerBarcodeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuContainerBarcodeDto> PrepareManuContainerBarcodeDtos(PagedInfo<ManuContainerBarcodeQueryView> pagedInfo)
        {
            var manuContainerBarcodeDtos = new List<ManuContainerBarcodeDto>();
            foreach (var manuContainerBarcodeView in pagedInfo.Data)
            {
                var manuContainerBarcodeDto = manuContainerBarcodeView.ToModel<ManuContainerBarcodeDto>();
                manuContainerBarcodeDtos.Add(manuContainerBarcodeDto);
            }

            return manuContainerBarcodeDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerBarcodeModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerBarcodeAsync(ManuContainerBarcodeModifyDto manuContainerBarcodeModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerBarcodeModifyDto);

            //DTO转换实体
            var manuContainerBarcodeEntity = manuContainerBarcodeModifyDto.ToEntity<ManuContainerBarcodeEntity>();
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerBarcodeRepository.UpdateAsync(manuContainerBarcodeEntity);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateManuContainerBarcodeStatusDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerBarcodeStatusAsync(UpdateManuContainerBarcodeStatusDto updateManuContainerBarcodeStatusDto)
        {
            // 判断是否有获取到站点码 
            if (!_currentSite.SiteId.HasValue || _currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            var containerBarcode = await _manuContainerBarcodeRepository.GetByIdAsync(updateManuContainerBarcodeStatusDto.Id);
            if (containerBarcode == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16726));
            }

            //验证DTO
            await _validationUpdateStatusRules.ValidateAndThrowAsync(updateManuContainerBarcodeStatusDto);
            //关闭操作必须要装箱数量达到最小包装数
            if (updateManuContainerBarcodeStatusDto.Status == 2)
            {
                var container = await _inteContainerRepository.GetByIdAsync(containerBarcode.ContainerId);
                //查询已包装数
                var containerPacks = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(containerBarcode.Id, _currentSite.SiteId.Value);
                if (containerPacks.Count() < container.Minimum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16723));
                }
            }

            //DTO转换实体
            var manuContainerBarcodeEntity = new ManuContainerBarcodeEntity();
            manuContainerBarcodeEntity.Id = updateManuContainerBarcodeStatusDto.Id;
            manuContainerBarcodeEntity.Status = updateManuContainerBarcodeStatusDto.Status;
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
            await _manuContainerBarcodeRepository.UpdateStatusAsync(manuContainerBarcodeEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByIdAsync(long id)
        {
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(id);
            if (manuContainerBarcodeEntity != null)
            {
                return manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
            }
            return null;
        }

        /// <summary>
        /// 根据编码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByBarCodeAsync(string barCode)
        {
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery
            {
                BarCode = barCode,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (manuContainerBarcodeEntity != null)
            {
                //if (manuContainerBarcodeEntity.Status == (int)ManuContainerBarcodeStatusEnum.Close)
                //{
                //    return null;
                //}
                var containerEntity = await _inteContainerRepository.GetByIdAsync(manuContainerBarcodeEntity.ContainerId);

                var barcodeDto = manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
                barcodeDto.Maximum = containerEntity?.Maximum ?? 0;
                barcodeDto.Minimum = containerEntity?.Minimum ?? 0;
                return barcodeDto;
            }
            return null;
        }
    }
}
