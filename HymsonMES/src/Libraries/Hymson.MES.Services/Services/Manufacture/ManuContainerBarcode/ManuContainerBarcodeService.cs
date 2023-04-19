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
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
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
        private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuContainerPackService _manuContainerPack;
        private readonly IManuContainerPackRecordService _manuContainerPackRecordService;

        private readonly AbstractValidator<ManuContainerBarcodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerBarcodeModifyDto> _validationModifyRules;
        private readonly AbstractValidator<CreateManuContainerBarcodeDto> _validationCreateManuContainerBarcodeRules;
        private readonly AbstractValidator<UpdateManuContainerBarcodeStatusDto> _validationUpdateStatusRules;

        public ManuContainerBarcodeService(ICurrentUser currentUser, ICurrentSite currentSite, IManuContainerBarcodeRepository manuContainerBarcodeRepository, AbstractValidator<ManuContainerBarcodeCreateDto> validationCreateRules
            , AbstractValidator<ManuContainerBarcodeModifyDto> validationModifyRules
            , IManuContainerPackRepository manuContainerPackRepository
            , IInteContainerRepository ingiContainerRepository
            , IManuContainerPackRecordRepository manuContainerPackRecordRepository
            , IManuSfcRepository manuSfcRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
            , IInteCodeRulesRepository inteCodeRulesRepository
            , IManuGenerateBarcodeService manuGenerateBarcodeService
            , IProcMaterialRepository procMaterialRepository
            , IPlanWorkOrderRepository planWorkOrderRepository
            , IManuContainerPackService manuContainerPack
            , IManuContainerPackRecordService manuContainerPackRecordService
            , AbstractValidator<CreateManuContainerBarcodeDto> validationCreateManuContainerBarcodeRules,
            AbstractValidator<UpdateManuContainerBarcodeStatusDto> validationUpdateStatusRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _manuContainerPackRepository = manuContainerPackRepository;
            _inteContainerRepository = ingiContainerRepository;
            _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuContainerPack = manuContainerPack;
            _manuContainerPackRecordService = manuContainerPackRecordService;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _validationCreateManuContainerBarcodeRules = validationCreateManuContainerBarcodeRules;
            _validationUpdateStatusRules = validationUpdateStatusRules;
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

            //获取工单信息
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(createManuContainerBarcodeDto.BarCode);
            if (sfcEntity == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES16701));
            }
            var sfcinfo = await _manuSfcInfoRepository.GetBySFCAsync(sfcEntity.SFC);
            if (sfcinfo == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES16701));
            }
            var workorder = await _planWorkOrderRepository.GetByIdAsync(sfcinfo.WorkOrderId);
            //获取物料信息
            var material = await _procMaterialRepository.GetByIdAsync(sfcinfo.ProductId);
            if (material == null)
                throw new ValidationException(nameof(ErrorCode.MES10204));
            /*根据条码判定是否有包装记录
             * Y 返回 view 
             * N ，判定包装码是否为空，
             *        Y  根据条码查找打开着的包装码 返回view
             *        N  返回这个包装码的view
             */
            var foo = await _manuContainerPackRepository.GetByLadeBarCodeAsync(createManuContainerBarcodeDto.BarCode);
            if (foo != null)
            {
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId);
                return await GetContainerPackView(workorder, material, barcodeobj);
            }
            else
            {
                //新条码&& 没有指定包装
                if (string.IsNullOrEmpty(createManuContainerBarcodeDto.ContainerCode))
                {
                    //查找相同产品ID及打开着的包装
                    var barcodeobj = await _manuContainerBarcodeRepository.GetByProductIdAsync(sfcinfo.ProductId, (int)ManuContainerBarcodeStatusEnum.Open);
                    if (barcodeobj != null)
                    {
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id);
                        if (inte.Maximum > packs.Count())
                        {
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            return await GetContainerPackView(workorder, material, barcodeobj);
                        }
                        else
                        {
                            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material);
                        }

                    }
                    else //全新包装
                    {
                        return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material);
                    }
                }
                else //新条码&& 指定包装
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  使用这个包装 
                     * N 创建全新包装
                     */

                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(createManuContainerBarcodeDto.ContainerCode);
                    if (barcodeobj?.ProductId == sfcinfo.ProductId)//相同包装
                    {
                        var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                        var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id);
                        if (inte.Maximum > packs.Count())
                        {
                            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                            {
                                await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                                {
                                    ContainerBarCodeId = barcodeobj.Id,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                                {
                                    ContainerBarCodeId = barcodeobj.Id,
                                    OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                    LadeBarCode = createManuContainerBarcodeDto.BarCode

                                });
                                ts.Complete();
                            }
                            return await GetContainerPackView(workorder, material, barcodeobj);
                        }
                        else
                        {
                            barcodeobj.Status = (int)ManuContainerBarcodeStatusEnum.Close;

                            await _manuContainerBarcodeRepository.UpdateAsync(barcodeobj);
                            return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material);
                        }
                    }
                    else //不是相同包装，创建全新包装
                    {
                        return await CreateNewBarcode(manuContainerBarcodeEntity, sfcinfo, workorder, material);
                    }
                }
            }

        }
        /// <summary>
        /// 创建全新包装，返回包装清单
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <param name="sfcinfo"></param>
        /// <param name="workorder"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        private async Task<ManuContainerBarcodeView> CreateNewBarcode(ManuContainerBarcodeEntity manuContainerBarcodeEntity, ManuSfcInfoEntity sfcinfo, PlanWorkOrderEntity workorder, ProcMaterialEntity material)
        {
            //判定  是物料-包装规格   OR 物料组-包装规格
            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
            {
                DefinitionMethod = DefinitionMethodEnum.Material,
                MaterialId = sfcinfo.ProductId,
                MaterialGroupId = 0
            });
            //物料-包装规格
            if (entityByRelation != null)
            {
                manuContainerBarcodeEntity.ContainerId = entityByRelation.Id;
                manuContainerBarcodeEntity.ProductId = sfcinfo.ProductId;
                var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(sfcinfo.ProductId);
                if (inteCodeRulesEntity == null)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16501)).WithData("product", material.MaterialCode);
                }
                var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
                {
                    CodeRuleId = inteCodeRulesEntity.Id,
                    Count = 1
                });
                manuContainerBarcodeEntity.BarCode = barcodeList.First();
                //创建包装
                using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                {
                    await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);
                    await _manuContainerPack.CreateManuContainerPackAsync(new ManuContainerPackCreateDto()
                    {
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        LadeBarCode = manuContainerBarcodeEntity.BarCode

                    });
                    await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                    {
                        ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                        OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                        LadeBarCode = manuContainerBarcodeEntity.BarCode

                    });
                    ts.Complete();
                }

                return await GetContainerPackView(workorder, material, manuContainerBarcodeEntity, entityByRelation);

            }
            else //物料组-包装规格
            {
                if (material.GroupId != 0)
                {
                    var entityByRelation1 = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
                    {
                        DefinitionMethod = DefinitionMethodEnum.MaterialGroup,
                        MaterialId = sfcinfo.ProductId,
                        MaterialGroupId = material.GroupId
                    });
                    if (entityByRelation1 != null)
                    {
                        manuContainerBarcodeEntity.ContainerId = entityByRelation1.Id;
                        manuContainerBarcodeEntity.ProductId = sfcinfo.ProductId;
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
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode
                            });
                            await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(new ManuContainerPackRecordCreateDto()
                            {
                                ContainerBarCodeId = manuContainerBarcodeEntity.Id,
                                OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Load,
                                LadeBarCode = manuContainerBarcodeEntity.BarCode

                            });
                            ts.Complete();
                        }


                        return await GetContainerPackView(workorder, material, manuContainerBarcodeEntity, entityByRelation1);
                    }
                    else
                    {
                        throw new ValidationException(nameof(ErrorCode.MES16703));
                    }
                }
                else
                {
                    throw new ValidationException(nameof(ErrorCode.MES10219));
                }
            }
        }
        /// <summary>
        /// 获取包装清单
        /// </summary>
        /// <param name="workorder"></param>
        /// <param name="material"></param>
        /// <param name="barcodeobj"></param>
        /// <param name="inte"></param>
        /// <returns></returns>
        private async Task<ManuContainerBarcodeView> GetContainerPackView(PlanWorkOrderEntity workorder, ProcMaterialEntity material, ManuContainerBarcodeEntity barcodeobj, InteContainerEntity inte = null)
        {
            if (inte == null)
                inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
            var packs = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id);//实际绑定集合
            ManuContainerBarcodeView view = new ManuContainerBarcodeView()
            {
                manuContainerBarcodeEntity = barcodeobj,
                inteContainerEntity = inte,
                manuContainerPacks = packs.Select<ManuContainerPackEntity, ManuContainerPackDto>(m =>
                {
                    return new ManuContainerPackDto()
                    {
                        ContainerBarCodeId = m.ContainerBarCodeId,
                        CreatedBy = m.CreatedBy,
                        CreatedOn = m.CreatedOn,
                        Id = m.Id,
                        LadeBarCode = m.LadeBarCode,
                        MaterialCode = material.MaterialCode,
                        SiteId = m.SiteId,
                        WorkOrderCode = workorder.OrderCode,
                        Count = packs.Count()
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
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationUpdateStatusRules.ValidateAndThrowAsync(updateManuContainerBarcodeStatusDto);

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
    }
}
