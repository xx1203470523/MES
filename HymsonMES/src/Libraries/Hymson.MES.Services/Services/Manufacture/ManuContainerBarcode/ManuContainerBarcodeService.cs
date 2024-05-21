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
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
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

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly AbstractValidator<ManuContainerBarcodeModifyDto> _validationModifyRules;
        private readonly AbstractValidator<CreateManuContainerBarcodeDto> _validationCreateManuContainerBarcodeRules;
        private readonly AbstractValidator<UpdateManuContainerBarcodeStatusDto> _validationUpdateStatusRules;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuContainerBarcodeRepository"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="ingiContainerRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuContainerPack"></param>
        /// <param name="manuFacePlateRepository"></param>
        /// <param name="manuFacePlateContainerPackRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuContainerPackRecordService"></param>
        /// <param name="validationCreateManuContainerBarcodeRules"></param>
        /// <param name="validationUpdateStatusRules"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        public ManuContainerBarcodeService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository
            , AbstractValidator<ManuContainerBarcodeModifyDto> validationModifyRules
            , IManuContainerPackRepository manuContainerPackRepository
            , IInteContainerRepository ingiContainerRepository
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
             , IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository procResourceRepository
            , IManuSfcRepository manuSfcRepository,
           IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _validationModifyRules = validationModifyRules;
            _manuContainerPackRepository = manuContainerPackRepository;
            _inteContainerRepository = ingiContainerRepository;
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
            _manuSfcStepRepository = manuSfcStepRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createManuContainerBarcodeDto"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeView> CreateManuContainerBarcodeAsync(CreateManuContainerBarcodeDto createManuContainerBarcodeDto)
        {
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

            return await CreatePackageAsync(createManuContainerBarcodeDto, manuContainerBarcodeEntity);

        }
        private async Task<ManuContainerBarcodeView> CreatePackageAsync(CreateManuContainerBarcodeDto createManuContainerBarcodeDto, ManuContainerBarcodeEntity manuContainerBarcodeEntity)
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
            var facePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery { Code = createManuContainerBarcodeDto.FacePlateCode, Site = _currentSite.SiteId });
            if (facePlateEntity == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES16783));
            var facePlateContainerPackEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(facePlateEntity.Id);
            facePlateContainerPackEntity.ProcedureId = createManuContainerBarcodeDto.ProcedureId;
            if (procobj.PackingLevel == (int)LevelEnum.One)
            {
                return await CreateFirstPackageAsync(createManuContainerBarcodeDto, facePlateContainerPackEntity, manuContainerBarcodeEntity);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createManuContainerBarcodeDto"></param>
        /// <param name="facePlateContainerPackEntity"></param>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<ManuContainerBarcodeView> CreateFirstPackageAsync(CreateManuContainerBarcodeDto createManuContainerBarcodeDto
            , ManuFacePlateContainerPackEntity facePlateContainerPackEntity, ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {

            //获取条码生产信息
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = createManuContainerBarcodeDto.BarCode
            });
            if (sfcProduceEntity != null)
            {
                //条码是否已报废
                if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes)
                    throw new CustomerValidationException(nameof(ErrorCode.MES16720));
                //是否允许活动产品
                if (sfcProduceEntity.Status == SfcStatusEnum.Activity && !facePlateContainerPackEntity.IsAllowActiveProduct)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16711));
                }
                //是否允许排队产品
                if (sfcProduceEntity.Status == SfcStatusEnum.lineUp && !facePlateContainerPackEntity.IsAllowQueueProduct)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16713));
                }
                sfcProduceEntity.ResourceId = createManuContainerBarcodeDto.ResourceId;
                sfcProduceEntity.ProcedureId = createManuContainerBarcodeDto.ProcedureId;
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
                    var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        SFC = createManuContainerBarcodeDto.BarCode,
                        Type = SfcTypeEnum.Produce
                    });
                    sfcProduceEntity = new ManuSfcProduceEntity
                    {
                        SFC = createManuContainerBarcodeDto.BarCode,
                        ProductId = whMaterialInventoryEntity.MaterialId,
                        WorkOrderId = 0,
                        WorkCenterId = 0,
                        ProductBOMId = 0,
                        EquipmentId = 0,
                        Qty = sfcEntity?.Qty ?? 0,
                        Status = SfcStatusEnum.Complete,
                        ResourceId = createManuContainerBarcodeDto.ResourceId,
                        ProcedureId = createManuContainerBarcodeDto.ProcedureId,
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
                SiteId = _currentSite.SiteId ?? 0,
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
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId.GetValueOrDefault());
                throw new CustomerValidationException(nameof(ErrorCode.MES16721)).WithData("sfc", packQuery.LadeBarCode).WithData("barcode", barcodeobj?.BarCode ?? foo.ContainerBarCodeId.ToString());
            }
            else
            {
                //新条码&& 没有指定包装
                if (string.IsNullOrEmpty(createManuContainerBarcodeDto.ContainerCode))
                {
                    #region 查找相同产品ID及打开着的包装
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
                    #endregion
                    //全新包装
                    var sfcStepEntity = CreateSFCStepEntity(sfcProduceEntity);
                    return await CreateNewBarcodeAsync(manuContainerBarcodeEntity, sfcProduceEntity.ProductId, sfcProduceEntity.WorkOrderId, material, createManuContainerBarcodeDto, (int)LevelEnum.One, sfcStepEntity);
                }
                else //新条码&& 指定包装
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  使用这个包装 
                     * N 创建全新包装
                     */

                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery { BarCode = createManuContainerBarcodeDto.ContainerCode, SiteId = manuContainerBarcodeEntity.SiteId });
                    //容器跟选择的工序等级不匹配
                    if (barcodeobj.PackLevel != (int)LevelEnum.One)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16731));
                    }
                    if (barcodeobj.Status == ManuContainerBarcodeStatusEnum.Close || barcodeobj.IsDeleted == 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16722)).WithData("packId", barcodeobj.Id);
                    }

                    //物料不一样报错
                    if (material.MaterialCode != barcodeobj.MaterialCode)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16730));
                    }

                    if (barcodeobj.WorkOrderId != sfcProduceEntity.WorkOrderId && !facePlateContainerPackEntity.IsMixedWorkOrder)
                    {
                        var planWorkOrder = await _planWorkOrderRepository.GetByIdsAsync(new[] { barcodeobj.WorkOrderId, sfcProduceEntity.WorkOrderId });
                        var barOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == barcodeobj.WorkOrderId)?.OrderCode ?? "";
                        var sfcOrderCode = planWorkOrder.FirstOrDefault(x => x.Id == sfcProduceEntity.WorkOrderId)?.OrderCode ?? "";
                        throw new CustomerValidationException(nameof(ErrorCode.MES16706)).WithData("first", barOrderCode).WithData("second", sfcOrderCode);
                    }

                    //比较物料版本
                    if (material.Version != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    if ((facePlateContainerPackEntity.IsAllowDifferentMaterial && barcodeobj?.MaterialCode == material.MaterialCode) ||
                        ((!facePlateContainerPackEntity.IsAllowDifferentMaterial) && barcodeobj?.ProductId == sfcProduceEntity.ProductId))
                    {
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                        if (inte.Maximum > packs.Count())
                        {
                            var sfcStepEntity = CreateSFCStepEntity(sfcProduceEntity);
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                // 记录step信息
                                await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                    ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode
                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                    ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            if (inte.Maximum == (packs.Count() + 1))
                            {
                                barcodeobj.Status = ManuContainerBarcodeStatusEnum.Close;

                                await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            }
                            return await GetContainerPackViewAsync(sfcProduceEntity.WorkOrderId, material.Id, barcodeobj, true);
                        }
                        else
                        {
                            barcodeobj.Status = ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                        }
                    }
                    else
                    {
                        var sfcStepEntity = CreateSFCStepEntity(sfcProduceEntity);
                        //不是相同包装，创建全新包装
                        return await CreateNewBarcodeAsync(manuContainerBarcodeEntity, sfcProduceEntity.ProductId, sfcProduceEntity.WorkOrderId, material, createManuContainerBarcodeDto, (int)LevelEnum.One, sfcStepEntity);
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
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId.GetValueOrDefault());
                throw new CustomerValidationException(nameof(ErrorCode.MES16721)).WithData("sfc", packQuery.LadeBarCode).WithData("barcode", barcodeobj?.BarCode ?? foo.ContainerBarCodeId.ToString());
            }
            else
            {
                //子级包装对象
                var prebarcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery() { BarCode = createManuContainerBarcodeDto.BarCode, SiteId = manuContainerBarcodeEntity.SiteId });
                if (prebarcodeobj == null)
                    throw new CustomerValidationException(nameof(ErrorCode.MES16718));

                //判断容器是否已关闭，只有关闭的才能装箱
                if (prebarcodeobj.Status != ManuContainerBarcodeStatusEnum.Close)
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
                    #region 查找相同产品ID及打开着的包装
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
                    #endregion
                    //全新包装
                    //获取物料信息
                    var material = await _procMaterialRepository.GetByIdAsync(prebarcodeobj.ProductId);
                    return await CreateNewBarcodeAsync(manuContainerBarcodeEntity, prebarcodeobj.ProductId, prebarcodeobj.WorkOrderId, material, createManuContainerBarcodeDto, level);
                    //}
                }
                else //新条码&& 指定包装
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  使用这个包装 
                     * N 创建全新包装
                     */

                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(new ManuContainerBarcodeQuery { BarCode = createManuContainerBarcodeDto.ContainerCode, SiteId = _currentSite.SiteId ?? 0 });

                    //容器跟选择的工序等级不匹配
                    if (barcodeobj.PackLevel != level)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16731));
                    }

                    //比较物料版本
                    if (prebarcodeobj.MaterialVersion != barcodeobj.MaterialVersion && !facePlateContainerPackEntity.IsAllowDifferentMaterial)
                        throw new CustomerValidationException(nameof(ErrorCode.MES16716));

                    //物料不一样报错
                    if (barcodeobj.MaterialCode != prebarcodeobj.MaterialCode)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16730));
                    }

                    if ((facePlateContainerPackEntity.IsAllowDifferentMaterial && barcodeobj?.MaterialCode == prebarcodeobj.MaterialCode) ||
                        ((!facePlateContainerPackEntity.IsAllowDifferentMaterial) && barcodeobj?.ProductId == prebarcodeobj.ProductId))
                    {
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);
                        if (inte.Maximum > packs.Count())
                        {
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                    ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                    ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            if (inte.Maximum == (packs.Count() + 1))
                            {
                                barcodeobj.Status = ManuContainerBarcodeStatusEnum.Close;

                                await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            }

                            return await GetContainerPackViewAsync(barcodeobj.WorkOrderId, barcodeobj.ProductId, barcodeobj);
                        }
                        else
                        {
                            barcodeobj.Status = ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            throw new CustomerValidationException(nameof(ErrorCode.MES16717));
                        }
                    }
                    else
                    {
                        //获取物料信息
                        var material = await _procMaterialRepository.GetByIdAsync(prebarcodeobj.ProductId);
                        return await CreateNewBarcodeAsync(manuContainerBarcodeEntity, prebarcodeobj.ProductId, prebarcodeobj.WorkOrderId, material, createManuContainerBarcodeDto, level);
                    }
                }
            }
        }

        private async Task<ManuContainerBarcodeView> CreateNewBarcodeAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity,
           long productId, long workorderId,
           ProcMaterialEntity material, CreateManuContainerBarcodeDto createManuContainerBarcodeDto, int level = 1, ManuSfcStepEntity? sfcStepEntity = null)
        {
            manuContainerBarcodeEntity.WorkOrderId = workorderId;
            manuContainerBarcodeEntity.MaterialVersion = material.Version ?? "9999—Unknow";
            manuContainerBarcodeEntity.MaterialCode = material.MaterialCode ?? "";
            manuContainerBarcodeEntity.PackLevel = level;
            bool isFirstPackage = level == (int)LevelEnum.One;

            //判定  是物料-包装规格   OR 物料组-包装规格
            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
            {
                DefinitionMethod = DefinitionMethodEnum.Material,
                MaterialId = productId,
                MaterialGroupId = 0,
                Status = SysDataStatusEnum.Enable,
                Level = (LevelEnum)level
            });
            //物料-包装规格
            if (entityByRelation != null)
            {
                manuContainerBarcodeEntity.ContainerId = entityByRelation.Id;
                manuContainerBarcodeEntity.ProductId = productId;

                //包装等级转换 程序内控制传入
                var packType = (CodeRulePackTypeEnum)level;
                string barcode = manuContainerBarcodeEntity.BarCode;
                manuContainerBarcodeEntity.BarCode = await GenerateBarCode(productId, material, packType);

                // 创建包装
                var rows = 0;
                using (TransactionScope trans = TransactionHelper.GetTransactionScope())
                {
                    if (level == 1 && sfcStepEntity != null)
                    {
                        // 记录step信息
                        await _manuSfcStepRepository.InsertAsync(sfcStepEntity);
                    }
                    rows += await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);

                    // 未保存到数据，事务回滚
                    if (rows <= 0)
                    {
                        trans.Dispose();
                        throw new CustomerValidationException(nameof(ErrorCode.MES16721))
                            .WithData("sfc", sfcStepEntity!.SFC ?? "")
                            .WithData("barcode", barcode);
                    }

                    await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                    {
                        ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                        ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        LadeBarCode = barcode
                    });
                    await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                    {
                        ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                        ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                        LadeBarCode = barcode
                    });
                    trans.Complete();
                }

                return await GetContainerPackViewAsync(workorderId, material.Id, manuContainerBarcodeEntity, isFirstPackage, entityByRelation);

            }
            else //物料组-包装规格
            {
                if (material.GroupId != 0)
                {
                    var entityByRelation1 = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
                    {
                        DefinitionMethod = DefinitionMethodEnum.MaterialGroup,
                        MaterialId = productId,
                        MaterialGroupId = material.GroupId,
                        Status = SysDataStatusEnum.Enable,
                        Level = (LevelEnum)level
                    });
                    if (entityByRelation1 != null)
                    {
                        manuContainerBarcodeEntity.ContainerId = entityByRelation1.Id;
                        manuContainerBarcodeEntity.ProductId = productId;
                        //包装等级转换 程序内控制传入
                        var packType = (CodeRulePackTypeEnum)level;
                        manuContainerBarcodeEntity.BarCode = await GenerateBarCode(productId, material, packType);
                        //入库
                        var rows = 0;
                        using (TransactionScope trans = TransactionHelper.GetTransactionScope())
                        {
                            if (level == 1 && sfcStepEntity != null)
                            {
                                // 记录step信息
                                await _manuSfcStepRepository.InsertAsync(sfcStepEntity);
                            }
                            rows += await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);

                            // 未保存到数据，事务回滚
                            if (rows <= 0)
                            {
                                trans.Dispose();
                                throw new CustomerValidationException(nameof(ErrorCode.MES16721))
                                    .WithData("sfc", sfcStepEntity!.SFC ?? "")
                                    .WithData("barcode", manuContainerBarcodeEntity.BarCode);
                            }

                            await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                            {
                                ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode
                            });
                            await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                            {
                                ResourceId = createManuContainerBarcodeDto.ResourceId ?? 0,
                                ProcedureId = createManuContainerBarcodeDto.ProcedureId,
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode

                            });
                            trans.Complete();
                        }

                        return await GetContainerPackViewAsync(workorderId, material.Id, manuContainerBarcodeEntity, isFirstPackage, entityByRelation1);
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
        /// 生成容器编码
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="material"></param>
        /// <param name="packType"></param>
        /// <returns></returns>
        private async Task<string> GenerateBarCode(long productId, ProcMaterialEntity material, CodeRulePackTypeEnum packType)
        {
            //根据编码类型，包装等级查询编码规则
            var inteCodeRulesResult = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProductId = productId,
                CodeType = CodeRuleCodeTypeEnum.PackagingSeqCode,
                PackType = packType
            });
            var inteCodeRulesEntity = inteCodeRulesResult.FirstOrDefault();
            if (inteCodeRulesEntity == null || inteCodeRulesEntity.ProductId != productId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16735)).WithData("product", material?.MaterialCode ?? "");
            }
            var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                CodeRuleId = inteCodeRulesEntity.Id,
                Count = 1
            });
            return barcodeList.First();
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
        private async Task<ManuContainerBarcodeView> GetContainerPackViewAsync(long workorderId, long materialId, ManuContainerBarcodeEntity barcodeobj, bool isFirstPackage = false, InteContainerEntity? inte = null)
        {
            if (inte == null)
                inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
            var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id, barcodeobj.SiteId);//实际绑定集合

            var containerPackEntities = new List<ManuContainerPackEntity>();
            var packBarCodesEntities = new List<ManuContainerBarcodeEntity>();
            var sfcDatas = new List<ManuSfcProduceEntity>();
            var planOrders = new List<PlanWorkOrderEntity>();
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
            else
            {
                var query = new ManuSfcProduceQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = packs.Select(x => x.LadeBarCode).ToArray() };
                var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(query);
                sfcDatas = sfcProduceEntities.ToList();
                var orderIds = sfcDatas.Select(x => x.WorkOrderId).ToArray();
                planOrders = (await _planWorkOrderRepository.GetByIdsAsync(orderIds)).ToList();
            }

            //转换ID为编码
            var procMateria = await _procMaterialRepository.GetByIdAsync(materialId);

            ManuContainerBarcodeView view = new ManuContainerBarcodeView()
            {
                manuContainerBarcodeEntity = barcodeobj,
                inteContainerEntity = inte,
                manuContainerPacks = packs.Select<ManuContainerPackEntity, ManuContainerPackDto>(m =>
                {
                    var barCode = packBarCodesEntities.FirstOrDefault(x => x.BarCode == m.LadeBarCode);
                    var barCodeId = barCode?.Id ?? 0;
                    var count = isFirstPackage ? 1 : containerPackEntities.Count(x => x.ContainerBarCodeId == barCodeId);

                    var workOrderId = 0L;
                    if (isFirstPackage)
                    {
                        workOrderId = sfcDatas.FirstOrDefault(x => x.SFC == m.LadeBarCode)?.WorkOrderId ?? 0;
                    }

                    return new ManuContainerPackDto()
                    {
                        ContainerBarCodeId = m.ContainerBarCodeId,
                        CreatedBy = m.CreatedBy,
                        CreatedOn = m.CreatedOn,
                        Id = m.Id,
                        BarCode = barcodeobj.BarCode,
                        LadeBarCode = m.LadeBarCode,
                        MaterialCode = procMateria?.MaterialCode ?? string.Empty,//如果关联结果删除直接返回空
                        SiteId = m.SiteId,
                        WorkOrderCode = isFirstPackage ? planOrders.FirstOrDefault(x => x.Id == workOrderId)?.OrderCode ?? "" : "",
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
            var containerBarcode = await _manuContainerBarcodeRepository.GetByIdAsync(updateManuContainerBarcodeStatusDto.Id);
            if (containerBarcode == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16726));
            }

            // 验证DTO
            await _validationUpdateStatusRules.ValidateAndThrowAsync(updateManuContainerBarcodeStatusDto);
            // 关闭操作必须要装箱数量达到最小包装数
            if (updateManuContainerBarcodeStatusDto.Status == ManuContainerBarcodeStatusEnum.Close)
            {
                var container = await _inteContainerRepository.GetByIdAsync(containerBarcode.ContainerId);
                // 查询已包装数
                var containerPacks = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(containerBarcode.Id, _currentSite.SiteId!.Value);
                if (containerPacks.Count() < container.Minimum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16723));
                }
            }

            // DTO转换实体
            var manuContainerBarcodeEntity = new ManuContainerBarcodeEntity();
            manuContainerBarcodeEntity.Id = updateManuContainerBarcodeStatusDto.Id;
            manuContainerBarcodeEntity.Status = updateManuContainerBarcodeStatusDto.Status;
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();

            var rows = await _manuContainerBarcodeRepository.UpdateStatusAsync(manuContainerBarcodeEntity);
            if (rows <= 0)
            {
                string errorCode = updateManuContainerBarcodeStatusDto.Status == ManuContainerBarcodeStatusEnum.Open ? nameof(ErrorCode.MES16733) : nameof(ErrorCode.MES16734);
                throw new CustomerValidationException(errorCode);
            }
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
            return new ManuContainerBarcodeDto();
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
                var containerEntity = await _inteContainerRepository.GetByIdAsync(manuContainerBarcodeEntity.ContainerId);

                var barcodeDto = manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
                barcodeDto.Maximum = containerEntity?.Maximum ?? 0;
                barcodeDto.Minimum = containerEntity?.Minimum ?? 0;
                return barcodeDto;
            }
            return new ManuContainerBarcodeDto();
        }

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc)
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrderId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.ProductBOMId,
                ProcessRouteId = sfc.ProcessRouteId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.Package,
                CurrentStatus = sfc.Status,
                Remark = "容器包装",
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = _currentUser.UserName ?? ""
            };
        }
    }
}
