/*
 *creator: Karl
 *
 *describe: 载具类型维护    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
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

        public InteVehicleTypeService(ICurrentUser currentUser, ICurrentSite currentSite, IInteVehicleTypeRepository inteVehicleTypeRepository, AbstractValidator<InteVehicleTypeCreateDto> validationCreateRules, AbstractValidator<InteVehicleTypeModifyDto> validationModifyRules, IInteVehicleTypeVerifyRepository inteVehicleTypeVerifyRepository, AbstractValidator<InteVehicleTypeVerifyCreateDto> validationInteVehicleTypeVerifyCreateRules, IProcMaterialRepository procMaterialRepository, IProcMaterialGroupRepository procMaterialGroupRepository, IInteVehicleRepository inteVehicleRepository)
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
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteVehicleTypeCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteVehicleTypeAsync(InteVehicleTypeCreateDto inteVehicleTypeCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteVehicleTypeCreateDto);

            //DTO转换实体
            var inteVehicleTypeEntity = inteVehicleTypeCreateDto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.Id= IdGenProvider.Instance.CreateId();
            inteVehicleTypeEntity.CreatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.CreatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity = await _inteVehicleTypeRepository.GetByCodeAsync( new InteVehicleTypeCodeQuery 
            { 
                Code =inteVehicleTypeEntity.Code.Trim(),
                SiteId=_currentSite.SiteId??0 
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18502));
            }

            #region 处理 载具类型验证数据
            var inteVehicleTypeVerifyEntitys=new List<InteVehicleTypeVerifyEntity>();
            if (inteVehicleTypeCreateDto.VehicleTypeVerifyList.Any()) 
            {
                foreach (var item in inteVehicleTypeCreateDto.VehicleTypeVerifyList)
                {
                    //验证数据
                    await _validationInteVehicleTypeVerifyCreateRules.ValidateAndThrowAsync(item);

                    inteVehicleTypeVerifyEntitys.Add(new InteVehicleTypeVerifyEntity()
                    {
                        VehicleTypeId= inteVehicleTypeEntity.Id,
                        Type=item.Type,
                        VerifyId=item.VerifyId,

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

            using (TransactionScope ts = new TransactionScope())
            {
                //入库
                await _inteVehicleTypeRepository.InsertAsync(inteVehicleTypeEntity);

                if (inteVehicleTypeVerifyEntitys.Any())
                    await _inteVehicleTypeVerifyRepository.InsertsAsync(inteVehicleTypeVerifyEntitys);

                ts.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteVehicleTypeAsync(long id)
        {
            await _inteVehicleTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteVehicleTypeAsync(long[] ids)
        {
            //校验数据 ：判断哪个载具使用了载具类型
            var vehicleEntitys= await _inteVehicleRepository.GetByVehicleTypeIdsAsync(new InteVehicleVehicleTypeIdsQuery() { SiteId = _currentSite.SiteId ?? 0, VehicleTypeIds = ids });
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
        private static List<InteVehicleTypeDto> PrepareInteVehicleTypeDtos(PagedInfo<InteVehicleTypeEntity>   pagedInfo)
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
        /// 修改
        /// </summary>
        /// <param name="inteVehicleTypeModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteVehicleTypeAsync(InteVehicleTypeModifyDto inteVehicleTypeModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteVehicleTypeModifyDto);

            //DTO转换实体
            var inteVehicleTypeEntity = inteVehicleTypeModifyDto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();

            #region 处理 载具类型验证数据
            var inteVehicleTypeVerifyEntitys = new List<InteVehicleTypeVerifyEntity>();
            if (inteVehicleTypeModifyDto.VehicleTypeVerifyList.Any())
            {
                foreach (var item in inteVehicleTypeModifyDto.VehicleTypeVerifyList)
                {
                    //验证数据
                    await _validationInteVehicleTypeVerifyCreateRules.ValidateAndThrowAsync(item);

                    inteVehicleTypeVerifyEntitys.Add(new InteVehicleTypeVerifyEntity()
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

            using (TransactionScope ts = new TransactionScope())
            {
                await _inteVehicleTypeRepository.UpdateAsync(inteVehicleTypeEntity);

                //先删除
                await _inteVehicleTypeVerifyRepository.DeletesTrueByVehicleTypeIdAsync(new long[] { inteVehicleTypeEntity.Id });
                if (inteVehicleTypeVerifyEntitys.Any())
                    await _inteVehicleTypeVerifyRepository.InsertsAsync(inteVehicleTypeVerifyEntitys);

                ts.Complete();
            }

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
                //批量查询物料
                //var materialIds = inteVehicleTypeEntitys.Where(x => x.Type == VehicleTypeVerifyTypeEnum.Material).Select(x => x.VerifyId).Distinct().ToArray();
                //var materials = new List<ProcMaterialEntity>();
                //if(materialIds.Any())
                //    materials = (await _procMaterialRepository.GetByIdsAsync(materialIds)).ToList();

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
