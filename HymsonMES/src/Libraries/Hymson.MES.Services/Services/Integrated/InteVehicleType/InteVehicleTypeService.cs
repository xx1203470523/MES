using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 载具类型维护 服务
    /// </summary>
    public class InteVehicleTypeService : IInteVehicleTypeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 载具类型维护 仓储
        /// </summary>
        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;
        private readonly AbstractValidator<InteVehicleTypeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteVehicleTypeModifyDto> _validationModifyRules;

        private readonly IInteVehicleTypeVerifyRepository _inteVehicleTypeVerifyRepository;
        private readonly AbstractValidator<InteVehicleTypeVerifyCreateDto> _validationInteVehicleTypeVerifyCreateRules;

        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;
        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="inteVehicleTypeRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="inteVehicleTypeVerifyRepository"></param>
        /// <param name="validationInteVehicleTypeVerifyCreateRules"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procMaterialGroupRepository"></param>
        /// <param name="inteVehicleRepository"></param>
        /// <param name="inteVehicleFreightRepository"></param>
        public InteVehicleTypeService(ICurrentUser currentUser, ICurrentSite currentSite, IInteVehicleTypeRepository inteVehicleTypeRepository, AbstractValidator<InteVehicleTypeCreateDto> validationCreateRules, AbstractValidator<InteVehicleTypeModifyDto> validationModifyRules, IInteVehicleTypeVerifyRepository inteVehicleTypeVerifyRepository, AbstractValidator<InteVehicleTypeVerifyCreateDto> validationInteVehicleTypeVerifyCreateRules, IProcMaterialRepository procMaterialRepository, IProcMaterialGroupRepository procMaterialGroupRepository, IInteVehicleRepository inteVehicleRepository, IInteVehicleFreightRepository inteVehicleFreightRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _inteVehicleTypeVerifyRepository = inteVehicleTypeVerifyRepository;
            _validationInteVehicleTypeVerifyCreateRules = validationInteVehicleTypeVerifyCreateRules;
            _procMaterialRepository = procMaterialRepository;
            _procMaterialGroupRepository = procMaterialGroupRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<long> CreateInteVehicleTypeAsync(InteVehicleTypeCreateDto dto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(dto);

            // DTO转换实体
            var inteVehicleTypeEntity = dto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.Id = IdGenProvider.Instance.CreateId();
            inteVehicleTypeEntity.CreatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.CreatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.SiteId = _currentSite.SiteId ?? 0;

            // 验证是否编码唯一
            var entity = await _inteVehicleTypeRepository.GetByCodeAsync(new InteVehicleTypeCodeQuery
            {
                Code = inteVehicleTypeEntity.Code.Trim(),
                SiteId = _currentSite.SiteId ?? 0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18502));
            }

            #region 处理 载具类型验证数据
            List<InteVehicleTypeVerifyEntity> detailEntities = new();
            if (dto.VehicleTypeVerifyList != null && dto.VehicleTypeVerifyList.Any())
            {
                foreach (var item in dto.VehicleTypeVerifyList)
                {
                    //验证数据
                    await _validationInteVehicleTypeVerifyCreateRules.ValidateAndThrowAsync(item);

                    detailEntities.Add(new InteVehicleTypeVerifyEntity()
                    {
                        VehicleTypeId = inteVehicleTypeEntity.Id,
                        Type = item.Type,
                        VerifyId = item.VerifyId,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion

            using var ts = TransactionHelper.GetTransactionScope();
            await _inteVehicleTypeRepository.InsertAsync(inteVehicleTypeEntity);
            if (detailEntities.Any())
            {
                await _inteVehicleTypeVerifyRepository.InsertsAsync(detailEntities);
            }
            ts.Complete();
            return inteVehicleTypeEntity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ModifyInteVehicleTypeAsync(InteVehicleTypeModifyDto dto)
        {
            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(dto);

            // DTO转换实体
            var inteVehicleTypeEntity = dto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();

            #region 处理 载具类型验证数据
            List<InteVehicleTypeVerifyEntity> detailEntities = new();
            if (dto.VehicleTypeVerifyList != null && dto.VehicleTypeVerifyList.Any())
            {
                foreach (var item in dto.VehicleTypeVerifyList)
                {
                    // 验证数据
                    await _validationInteVehicleTypeVerifyCreateRules.ValidateAndThrowAsync(item);
                    detailEntities.Add(new InteVehicleTypeVerifyEntity()
                    {
                        VehicleTypeId = inteVehicleTypeEntity.Id,
                        Type = item.Type,
                        VerifyId = item.VerifyId,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion

            // 验证 载具（已使用该载具类型）绑定了产品序列码，则无法修改载具类型
            var useInteVehicleEntities = await _inteVehicleRepository.GetByVehicleTypeIdsAsync(new InteVehicleVehicleTypeIdsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                VehicleTypeIds = new long[] { dto.Id }
            });
            if (useInteVehicleEntities.Any())
            {
                var freights = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(useInteVehicleEntities.Select(x => x.Id).ToArray());
                if (freights.Any(x => x.Qty > 0))
                {
                    var exceedingQty = freights.FirstOrDefault(x => x.Qty > 0)?.Qty ?? 0; // 获取第一个数量大于零的已存放的数量值
                    throw new CustomerValidationException(nameof(ErrorCode.MES18518)).WithData("exceedingQty", exceedingQty);
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            await _inteVehicleTypeRepository.UpdateAsync(inteVehicleTypeEntity);
            await _inteVehicleTypeVerifyRepository.DeletesTrueByVehicleTypeIdAsync(new long[] { inteVehicleTypeEntity.Id });
            if (detailEntities.Any())
            {
                await _inteVehicleTypeVerifyRepository.InsertsAsync(detailEntities);
            }
            ts.Complete();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteVehicleTypeAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _inteVehicleTypeRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status == DisableOrEnableEnum.Enable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            // 校验数据 ：判断哪个载具使用了载具类型
            var vehicleEntitys = await _inteVehicleRepository.GetByVehicleTypeIdsAsync(new InteVehicleVehicleTypeIdsQuery() { SiteId = _currentSite.SiteId ?? 0, VehicleTypeIds = ids });
            if (vehicleEntitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18517));
            }

            return await _inteVehicleTypeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteVehicleTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleTypeDto>> GetPagedListAsync(InteVehicleTypePagedQueryDto inteVehicleTypePagedQueryDto)
        {
            var inteVehicleTypePagedQuery = inteVehicleTypePagedQueryDto.ToQuery<InteVehicleTypePagedQuery>();
            inteVehicleTypePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteVehicleTypeRepository.GetPagedInfoAsync(inteVehicleTypePagedQuery);

            //实体到DTO转换 装载数据
            List<InteVehicleTypeDto> inteVehicleTypeDtos = PrepareInteVehicleTypeDtos(pagedInfo);
            return new PagedInfo<InteVehicleTypeDto>(inteVehicleTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteVehicleTypeDto> PrepareInteVehicleTypeDtos(PagedInfo<InteVehicleTypeEntity> pagedInfo)
        {
            var inteVehicleTypeDtos = new List<InteVehicleTypeDto>();
            foreach (var inteVehicleTypeEntity in pagedInfo.Data)
            {
                var inteVehicleTypeDto = inteVehicleTypeEntity.ToModel<InteVehicleTypeDto>();
                inteVehicleTypeDtos.Add(inteVehicleTypeDto);
            }

            return inteVehicleTypeDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleTypeDto> QueryInteVehicleTypeByIdAsync(long id)
        {
            var inteVehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(id);
            if (inteVehicleTypeEntity != null)
            {
                return inteVehicleTypeEntity.ToModel<InteVehicleTypeDto>();
            }
            throw new CustomerValidationException(nameof(ErrorCode.MES18501));
        }

        /// <summary>
        /// 获取载具类型验证根据vehicleTypeId查询
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeVerifyDto>> QueryInteVehicleTypeVerifyByVehicleTypeIdAsync(long vehicleTypeId)
        {
            var inteVehicleTypeEntitys = await _inteVehicleTypeVerifyRepository.GetInteVehicleTypeVerifyEntitiesByVehicleTyleIdAsync(new long[] { vehicleTypeId });

            var inteVehicleTypeVerifyDtos = new List<InteVehicleTypeVerifyDto>();
            #region 处理数据
            if (inteVehicleTypeEntitys.Any())
            {
                var materials = await _procMaterialRepository.GetByIdsAsync(inteVehicleTypeEntitys.Where(x => x.Type == VehicleTypeVerifyTypeEnum.Material).Select(x => x.VerifyId).Distinct().ToArray());

                //批量查询物料组
                var materialGroups = await _procMaterialGroupRepository.GetByIdsAsync(inteVehicleTypeEntitys.Where(x => x.Type == VehicleTypeVerifyTypeEnum.MaterialGroup).Select(x => x.VerifyId).Distinct().ToArray());

                foreach (var item in inteVehicleTypeEntitys)
                {
                    var inteVehicleTypeVerifyDto = item.ToModel<InteVehicleTypeVerifyDto>();
                    if (item.Type == VehicleTypeVerifyTypeEnum.Material)
                    {
                        var material = materials.FirstOrDefault(x => x.Id == item.VerifyId);
                        inteVehicleTypeVerifyDto.VerifyCode = material?.MaterialCode;
                        inteVehicleTypeVerifyDto.Version = material?.Version;
                        inteVehicleTypeVerifyDto.Name = material?.MaterialName;
                    }
                    else
                    {
                        var materialGroup = materialGroups.FirstOrDefault(x => x.Id == item.VerifyId);
                        inteVehicleTypeVerifyDto.VerifyCode = materialGroup?.GroupCode;
                        inteVehicleTypeVerifyDto.Version = "";
                        inteVehicleTypeVerifyDto.Name = materialGroup?.GroupName;
                    }

                    inteVehicleTypeVerifyDtos.Add(inteVehicleTypeVerifyDto);
                }
            }

            #endregion

            return inteVehicleTypeVerifyDtos;
        }
    }
}
