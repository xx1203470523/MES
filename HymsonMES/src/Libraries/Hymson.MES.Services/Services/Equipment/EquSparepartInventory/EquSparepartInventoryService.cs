/*
 *creator: Karl
 *
 *describe: 备件库存    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.EquSparepartInventory;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.EquSparepartInventory;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Services.Dtos.EquSparepartInventory;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.EquSparepartInventory
{
    /// <summary>
    /// 备件库存 服务
    /// </summary>
    public class EquSparepartInventoryService : IEquSparepartInventoryService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 备件 仓储
        /// </summary>
        private readonly IEquSparePartsRepository _equSparePartsRepository;
        /// <summary>
        /// 备件库存 仓储
        /// </summary>
        private readonly IEquSparepartInventoryRepository _equSparepartInventoryRepository;
        private readonly IEquSparepartRecordRepository _equSparepartRecordRepository;
        private readonly IEquSparePartsGroupRepository _equSparePartsGroupRepository;
        private readonly IInteWorkCenterRepository _InteWorkCenterRepository;
        private readonly AbstractValidator<EquSparepartInventoryCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquSparepartInventoryModifyDto> _validationModifyRules;

        public EquSparepartInventoryService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSparepartInventoryRepository equSparepartInventoryRepository, AbstractValidator<EquSparepartInventoryCreateDto> validationCreateRules, AbstractValidator<EquSparepartInventoryModifyDto> validationModifyRules, IEquSparepartRecordRepository equSparepartRecordRepository, IEquSparePartsRepository equSparePartsRepository, IInteWorkCenterRepository InteWorkCenterRepository, IEquSparePartsGroupRepository equSparePartsGroupRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSparepartInventoryRepository = equSparepartInventoryRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _equSparepartRecordRepository = equSparepartRecordRepository;
            _equSparePartsRepository = equSparePartsRepository;
            _InteWorkCenterRepository = InteWorkCenterRepository;
            _equSparePartsGroupRepository = equSparePartsGroupRepository;
        }


        /// <summary>
        /// 获取出库选择备件信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<IEnumerable<GetOutboundChooseEquSparePartsDto>> GetOutboundChooseEquSparePartsAsync(GetOutboundChooseEquSparePartsParamDto param)
        {
            if (param == null || param.SparepartIds == null || !param.SparepartIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17905));
            }


            var siteId = _currentSite.SiteId ?? 0;
            var equSpareParts = await _equSparePartsRepository.GetByIdsAsync(param.SparepartIds.ToArray());
            if (equSpareParts == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17901));
            }

            var equSparepartInventorys = await _equSparepartInventoryRepository.GetEquSparepartInventoryEntitiesAsync(new EquSparepartInventoryQuery { SparepartIds = param.SparepartIds, SiteId = siteId });
            if (equSparepartInventorys == null || !equSparepartInventorys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17911));
            }

            var equSparePartsGroups = new List<EquSparePartsGroupEntity>();
            var sparePartTypeIds = equSpareParts.Where(it => it.SparePartTypeId > 0).Select(it => it.SparePartTypeId ?? 0).ToArray();
            if (sparePartTypeIds != null && sparePartTypeIds.Any())
            {
                equSparePartsGroups = (List<EquSparePartsGroupEntity>)await _equSparePartsGroupRepository.GetByIdsAsync(sparePartTypeIds);
            }


            List<GetOutboundChooseEquSparePartsDto> listDto = new();
            foreach (var item in equSpareParts)
            {
                var equSparepartInventory = equSparepartInventorys.Where(it => it.SparepartId == item.Id).FirstOrDefault();
                if (equSparepartInventory == null)
                {
                    continue;
                }
                if (equSparepartInventory.Qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17912));
                }
                GetOutboundChooseEquSparePartsDto dto = new()
                {
                    SparepartId = item.Id,
                    SparepartCode = item.Code,
                    SparepartName = item.Name,
                    Specifications = item.Specifications,
                    Qty = equSparepartInventory.Qty,

                };
                var equSparePartsGroup = equSparePartsGroups.Where(it => it.Id == item.SparePartTypeId).FirstOrDefault();
                if (equSparePartsGroup == null)
                {
                    continue;
                }
                if (equSparePartsGroup != null)
                {
                    dto.SparepartGroupId = equSparePartsGroup.Id;
                    dto.SparepartGroupCode = equSparePartsGroup.Code;
                    dto.SparepartGroupName = equSparePartsGroup.Name;
                }
                listDto.Add(dto);
            }
            return listDto;
        }



        /// <summary>
        /// 获取备件信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<GetEquSparePartsDto> GetEquSparePartsAsync(GetEquSparePartsParamDto param)
        {
            if (string.IsNullOrWhiteSpace(param.SparepartCode))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17905));
            }
            if (param.VerifyQty == 0)
            {
                if (param.Qty <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17906));
                }
            }

            var siteId = _currentSite.SiteId ?? 0;
            var equSpareParts = await _equSparePartsRepository.GetByCodeAsync(new EntityByCodeQuery { Code = param.SparepartCode, Site = _currentSite.SiteId ?? 0 });
            if (equSpareParts == null || equSpareParts.Status != DisableOrEnableEnum.Enable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17907)).WithData("Code", param.SparepartCode);
            }

            var equSparepartInventory = await _equSparepartInventoryRepository.GetBySparepartIdAsync(new EquSparepartInventoryQuery { SparepartId = equSpareParts.Id, SiteId = siteId });
            decimal inventoryQty = 0;
            if (equSparepartInventory != null)
            {
                //VerifyQty(new VerifyQtyParamDto
                //{
                //    SparepartQty = equSpareParts.Qty,
                //    InventoryQty = equSparepartInventory.Qty,
                //    OperateQty = param.Qty,
                //    OperateType = EquOperationTypeEnum.Inbound,
                //    SparepartCode = param.SparepartCode
                //});
                var thisQty = equSpareParts.Qty - equSparepartInventory.Qty;
                if (param.Qty > thisQty)
                {
                    if (equSpareParts.Qty == 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17908));
                    }
                    throw new CustomerValidationException(nameof(ErrorCode.MES17909));
                }
                inventoryQty = equSparepartInventory.Qty;
            }
            else
            {
                if (equSpareParts.Qty < param.Qty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17908));
                }
            }

            GetEquSparePartsDto dto = new()
            {
                SparepartId = equSpareParts.Id,
                SparepartCode = equSpareParts.Code,
                SparepartName = equSpareParts.Name,
                Specifications = equSpareParts.Specifications,
                Qty = param.Qty,
                InventoryQty = inventoryQty,
                SparepartQty = equSpareParts.Qty

            };


            if (equSpareParts.SparePartTypeId.HasValue)
            {
                var equSparePartsGroup = await _equSparePartsGroupRepository.GetByIdAsync(equSpareParts.SparePartTypeId ?? 0);
                if (equSparePartsGroup != null)
                {
                    dto.SparepartGroupId = equSparePartsGroup.Id;
                    dto.SparepartGroupCode = equSparePartsGroup.Code;
                    dto.SparepartGroupName = equSparePartsGroup.Name;
                }
            }

            return dto;
        }

        /// <summary>
        /// 备件库存操作（出入库） 
        /// </summary>
        /// <param name="equSparepartInventoryDto"></param>
        /// <returns></returns>
        public async Task OperationEquSparepartInventoryAsync(OperationEquSparepartInventoryDto equSparepartInventoryDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            var siteId = _currentSite.SiteId ?? 0;
            var sparepartIds = equSparepartInventoryDto.Param.Select(it => it.SparepartId);
            var equSpareparts = await _equSparePartsRepository.GetByReIdsAsync(sparepartIds.ToArray());
            if (equSpareparts == null || !equSpareparts.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17901));
            }

            var equSparepartInventorList = await _equSparepartInventoryRepository.GetEquSparepartInventoryEntitiesAsync(new EquSparepartInventoryQuery
            {
                SiteId = siteId,
                SparepartIds = sparepartIds
            });

            var inteWorkCenter = await _InteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery { Code = equSparepartInventoryDto.WorkCenterCode, Site = siteId });

            List<EquSparepartInventoryEntity> equSparepartInventoryAddList = new();
            List<EquSparepartInventoryEntity> equSparepartInventoryUpdateList = new();
            List<EquSparepartRecordEntity> equSparepartRecordEntityList = new();
            List<UpdateSparePartsQtyEntity> updateSparePartsQtyEntityList = new();
            List<long> sparePartIds = new();
            List<decimal> qty = new();

            foreach (var item in equSparepartInventoryDto.Param)
            {
                var equSparepartInventor = equSparepartInventorList.Where(it => it.SparepartId == item.SparepartId).FirstOrDefault();

                switch (equSparepartInventoryDto.OperationType)
                {
                    case EquOperationTypeEnum.Inbound:
                        if (equSparepartInventor == null)
                        {
                            var equSparepartInventoryEntity = new EquSparepartInventoryEntity
                            {
                                SparepartId = item.SparepartId,
                                Qty = item.Qty,
                                Remark = item.Remark,
                                Id = IdGenProvider.Instance.CreateId(),
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName,
                                CreatedOn = HymsonClock.Now(),
                                UpdatedOn = HymsonClock.Now(),
                                SiteId = siteId,
                            };
                            equSparepartInventoryAddList.Add(equSparepartInventoryEntity);
                        }
                        else
                        {
                            equSparepartInventor.Qty += item.Qty;
                            equSparepartInventor.UpdatedBy = _currentUser.UserName;
                            equSparepartInventor.UpdatedOn = HymsonClock.Now();
                            equSparepartInventoryUpdateList.Add(equSparepartInventor);
                        }
                        break;
                    case EquOperationTypeEnum.Outbound:
                        if (equSparepartInventor == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES17902)).WithData("Code", item.SparepartCode);
                        }
                        if (equSparepartInventor.Qty <= 0)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES17906)).WithData("Code", item.SparepartCode);
                        }
                        if (equSparepartInventor.Qty < item.Qty)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES17910)).WithData("Code", item.SparepartCode);
                        }
                        equSparepartInventor.Qty -= item.Qty;
                        equSparepartInventor.UpdatedBy = _currentUser.UserName;
                        equSparepartInventor.UpdatedOn = HymsonClock.Now();
                        equSparepartInventoryUpdateList.Add(equSparepartInventor);
                        break;
                    default:
                        throw new CustomerValidationException(nameof(ErrorCode.MES17904)).WithData("OperationType", equSparepartInventoryDto.OperationType);
                }

                var equSparepart = equSpareparts.Where(it => it.Id == item.SparepartId).FirstOrDefault()
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17903)).WithData("Code", item.SparepartCode);
                //var equSparepartRecordEntity = equSparepart.ToEntity<EquSparepartRecordEntity>();

                var equSparepartRecord = new EquSparepartRecordEntity
                {
                    SparepartId = equSparepart.Id, // 备件ID
                    Code = equSparepart.Code, // 备件编码
                    Name = equSparepart.Name, // 备件名称
                    SparePartTypeId = equSparepart.SparePartTypeId, // 备件类型ID
                    Type = EquipmentPartTypeEnum.SparePart, // 备件类型
                    IsStandard = equSparepart.IsStandard,// == YesOrNoEnum.Yes ? TrueOrFalseEnum.Yes : TrueOrFalseEnum.No, // 是否标准件
                    Status = DisableOrEnableEnum.Enable, // 状态
                    Remark = equSparepart.Remark ?? "", // 备注
                    SiteId = siteId, // 站点ID
                    Manufacturer = equSparepart.Manufacturer ?? "", // 制造商
                    DrawCode = equSparepart.DrawCode ?? "", // 图纸编号
                    Specifications = equSparepart.Specifications, // 规格
                    Position = equSparepart.Position ?? "", // 位置
                    IsCritical = equSparepart.IsCritical,// == YesOrNoEnum.Yes ? TrueOrFalseEnum.Yes : TrueOrFalseEnum.No, // 是否关键
                    Qty = equSparepart.Qty.ParseToInt(), // 数量
                    OperationType = equSparepartInventoryDto.OperationType, // 操作类型
                    OperationQty = item.Qty.ParseToInt(), // 操作数量
                    WorkCenterCode = equSparepartInventoryDto.WorkCenterCode ?? "",
                    WorkCenterId = inteWorkCenter?.Id,
                    Recipients = equSparepartInventoryDto.Recipients ?? "",

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    // ProcMaterialId = 5000, // 处理材料ID（已注释）
                    // UnitId = 20, // 单位ID（已注释）
                    // IsKey = TrueOrFalseEnum.True, // 是否关键（已注释）
                    // BluePrintNo = "BP-X2000", // 蓝图编号（已注释）
                    // Brand = equSparepart.Brand, // 品牌（已注释）
                    // ManagementMode = equSparepart.ManagementMode ?? "", // 管理模式（已注释）
                    // SupplierId = 200, // 供应商ID（已注释）
                    // EquipmentId = 4001 // 设备ID（已注释）
                };
                equSparepartRecordEntityList.Add(equSparepartRecord);

                sparePartIds.Add(equSparepart.Id);
                qty.Add(item.Qty);
            }

            UpdateSparePartsQtyEntity updateSparePartsQtyEntity = new()
            {
                SparePartIds = sparePartIds,
                Qty = qty,
            };

            using var trans = TransactionHelper.GetTransactionScope();

            //TODO 这里需要更新的话  是有问题的  无法闭环 
            //if (equSparepartInventoryDto.OperationType == EquOperationTypeEnum.Inbound)
            //{
            //    await _equSparePartsRepository.UpdateAddQtyAsync(updateSparePartsQtyEntity);
            //}
            //else
            //{
            //    await _equSparePartsRepository.UpdateMinusQtyAsync(updateSparePartsQtyEntity);
            //}

            await _equSparepartRecordRepository.InsertsAsync(equSparepartRecordEntityList);
            await _equSparepartInventoryRepository.InsertsAsync(equSparepartInventoryAddList);
            await _equSparepartInventoryRepository.UpdatesAsync(equSparepartInventoryUpdateList);

            trans.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquSparepartInventoryAsync(long id)
        {
            await _equSparepartInventoryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSparepartInventoryAsync(long[] ids)
        {
            return await _equSparepartInventoryRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equSparepartInventoryPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartInventoryPageDto>> GetPagedListAsync(EquSparepartInventoryPagedQueryDto equSparepartInventoryPagedQueryDto)
        {
            var equSparepartInventoryPagedQuery = equSparepartInventoryPagedQueryDto.ToQuery<EquSparepartInventoryPagedQuery>();
            equSparepartInventoryPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparepartInventoryRepository.GetPagedInfoAsync(equSparepartInventoryPagedQuery);

            //实体到DTO转换 装载数据
            List<EquSparepartInventoryPageDto> equSparepartInventoryDtos = PrepareEquSparepartInventoryDtos(pagedInfo);
            return new PagedInfo<EquSparepartInventoryPageDto>(equSparepartInventoryDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquSparepartInventoryPageDto> PrepareEquSparepartInventoryDtos(PagedInfo<EquSparepartInventoryPageView> pagedInfo)
        {
            var equSparepartInventoryDtos = new List<EquSparepartInventoryPageDto>();
            foreach (var equSparepartInventoryEntity in pagedInfo.Data)
            {
                var equSparepartInventoryDto = equSparepartInventoryEntity.ToModel<EquSparepartInventoryPageDto>();
                equSparepartInventoryDtos.Add(equSparepartInventoryDto);
            }

            return equSparepartInventoryDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSparepartInventoryDto"></param>
        /// <returns></returns>
        public async Task ModifyEquSparepartInventoryAsync(EquSparepartInventoryModifyDto equSparepartInventoryModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equSparepartInventoryModifyDto);

            //DTO转换实体
            var equSparepartInventoryEntity = equSparepartInventoryModifyDto.ToEntity<EquSparepartInventoryEntity>();
            equSparepartInventoryEntity.UpdatedBy = _currentUser.UserName;
            equSparepartInventoryEntity.UpdatedOn = HymsonClock.Now();

            await _equSparepartInventoryRepository.UpdateAsync(equSparepartInventoryEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparepartInventoryDto> QueryEquSparepartInventoryByIdAsync(long id)
        {
            var equSparepartInventoryEntity = await _equSparepartInventoryRepository.GetByIdAsync(id);
            if (equSparepartInventoryEntity != null)
            {
                return equSparepartInventoryEntity.ToModel<EquSparepartInventoryDto>();
            }
            return null;
        }


        #region 帮助
        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="dto"></param>
        private static void VerifyQty(VerifyQtyParamDto dto)
        {
            switch (dto.OperateType)
            {
                case EquOperationTypeEnum.Inbound:

                    var thisQty = dto.SparepartQty - dto.InventoryQty;
                    if (dto.OperateQty > thisQty)
                    {
                        if (dto.SparepartQty == 1)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES17908));
                        }
                        throw new CustomerValidationException(nameof(ErrorCode.MES17909));
                    }
                    break;
                case EquOperationTypeEnum.Outbound:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
