using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Services.Services.Equipment.EquEquipment
{
    /// <summary>
    /// 业务处理层（设备注册）
    /// @author Czhipu
    /// @date 2022-11-08
    /// </summary>
    public class EquEquipmentService : IEquEquipmentService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EquEquipmentSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储（设备关联API）
        /// </summary>
        private readonly IEquEquipmentLinkApiRepository _equEquipmentLinkApiRepository;

        /// <summary>
        /// 仓储（设备关联硬件）
        /// </summary>
        private readonly IEquEquipmentLinkHardwareRepository _equEquipmentLinkHardwareRepository;

        /// <summary>
        /// 仓储（设备Token）
        /// </summary>
        private readonly IEquEquipmentTokenRepository _equEquipmentTokenRepository;

        private readonly JwtOptions _jwtOptions;

        private readonly IEquEquipmentVerifyRepository _equEquipmentVerifyRepository;

        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EquEquipmentVerifyCreateDto> _verifyValidationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="equEquipmentLinkApiRepository"></param>
        /// <param name="equEquipmentLinkHardwareRepository"></param>
        /// <param name="equEquipmentTokenRepository"></param>
        /// <param name="jwtOptions"></param>
        /// <param name="equEquipmentVerifyRepository"></param>
        /// <param name="verifyValidationRules"></param>
        /// <param name="procResourceRepository"></param>
        public EquEquipmentService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquEquipmentSaveDto> validationSaveRules,
            IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentLinkApiRepository equEquipmentLinkApiRepository,
            IEquEquipmentLinkHardwareRepository equEquipmentLinkHardwareRepository,
            IEquEquipmentTokenRepository equEquipmentTokenRepository, IOptions<JwtOptions> jwtOptions, IEquEquipmentVerifyRepository equEquipmentVerifyRepository, AbstractValidator<EquEquipmentVerifyCreateDto> verifyValidationRules, IProcResourceRepository procResourceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentLinkApiRepository = equEquipmentLinkApiRepository;
            _equEquipmentLinkHardwareRepository = equEquipmentLinkHardwareRepository;
            _equEquipmentTokenRepository = equEquipmentTokenRepository;
            _jwtOptions = jwtOptions.Value;
            _equEquipmentVerifyRepository = equEquipmentVerifyRepository;
            _verifyValidationRules = verifyValidationRules;
            _procResourceRepository = procResourceRepository;
        }


        /// <summary>
        /// 添加（设备注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(EquEquipmentSaveDto parm)
        {
            #region 参数处理
            // 验证DTO
            parm.EquipmentCode = parm.EquipmentCode.ToTrimSpace();
            parm.EquipmentCode = parm.EquipmentCode.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // DTO转换实体
            var entity = parm.ToEntity<EquEquipmentEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            if (entity.QualTime.HasValue && entity.EntryDate.HasValue)
            {
                entity.ExpireDate = entity.EntryDate.Value.AddMonths(entity.QualTime.Value);
            }

            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (parm.ApiLinks != null && parm.ApiLinks.Any())
            {
                foreach (var item in parm.ApiLinks)
                {
                    EquEquipmentLinkApiEntity linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                    linkApi.EquipmentId = entity.Id;
                    linkApi.CreatedBy = entity.CreatedBy;
                    linkApi.UpdatedBy = entity.UpdatedBy;
                    linkApiList.Add(linkApi);
                }
            }

            // 绑定硬件
            List<EquEquipmentLinkHardwareEntity> linkHardwareList = new();
            if (parm.HardwareLinks != null && parm.HardwareLinks.Any())
            {
                foreach (var item in parm.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardware.CreatedBy = entity.CreatedBy;
                    linkHardware.UpdatedBy = entity.UpdatedBy;
                    linkHardwareList.Add(linkHardware);
                }
            }

            //绑定验证
            List<EquEquipmentVerifyEntity> verifyList = new();
            if (parm.Verifys != null && parm.Verifys.Any())
            {
                //验证验证参数
                foreach (var item in parm.Verifys)
                {
                    await _verifyValidationRules.ValidateAndThrowAsync(item);
                }

                foreach (var item in parm.Verifys)
                {
                    EquEquipmentVerifyEntity verify = new EquEquipmentVerifyEntity();
                    verify.EquipmentId = entity.Id;
                    verify.Account = item.Account;
                    verify.Password = item.Password;
                    verify.AccountType = item.AccountType;

                    verify.Id = IdGenProvider.Instance.CreateId();
                    verify.SiteId = _currentSite.SiteId ?? 0;
                    verify.CreatedBy = _currentUser.UserName;
                    verify.CreatedOn = HymsonClock.Now();
                    verify.UpdatedBy = _currentUser.UserName;
                    verify.UpdatedOn = HymsonClock.Now();
                    verifyList.Add(verify);
                }
            }

            #endregion

            #region 参数校验
            // 编码唯一性验证
            var checkEntity = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.EquipmentCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12600)).WithData("Code", entity.EquipmentCode);
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equEquipmentRepository.InsertAsync(entity);
                rows += await _equEquipmentLinkApiRepository.InsertRangeAsync(linkApiList);
                rows += await _equEquipmentLinkHardwareRepository.InsertRangeAsync(linkHardwareList);
                rows += await _equEquipmentVerifyRepository.InsertsAsync(verifyList);
                trans.Complete();
            }
            return entity.Id;
        }

        /// <summary>
        /// 修改（设备注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquEquipmentSaveDto parm)
        {
            #region 参数处理
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // DTO转换实体
            var entity = parm.ToEntity<EquEquipmentEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            if (entity.QualTime.HasValue && entity.EntryDate.HasValue)
            {
                entity.ExpireDate = entity.EntryDate.Value.AddMonths(entity.QualTime.Value);
            }

            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (parm.ApiLinks != null && parm.ApiLinks.Any())
            {
                foreach (var item in parm.ApiLinks)
                {
                    EquEquipmentLinkApiEntity linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                    linkApi.EquipmentId = entity.Id;
                    linkApi.CreatedBy = entity.CreatedBy;
                    linkApi.UpdatedBy = entity.UpdatedBy;
                    linkApiList.Add(linkApi);
                }
            }

            // 绑定硬件
            List<EquEquipmentLinkHardwareEntity> linkHardwareList = new();
            if (parm.HardwareLinks != null && parm.HardwareLinks.Any())
            {
                foreach (var item in parm.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardware.CreatedBy = entity.CreatedBy;
                    linkHardware.UpdatedBy = entity.UpdatedBy;
                    linkHardwareList.Add(linkHardware);
                }
            }

            //绑定验证
            List<EquEquipmentVerifyEntity> verifyList = new();
            if (parm.Verifys != null && parm.Verifys.Any())
            {
                //验证验证参数
                foreach (var item in parm.Verifys)
                {
                    await _verifyValidationRules.ValidateAndThrowAsync(item);
                }

                foreach (var item in parm.Verifys)
                {
                    EquEquipmentVerifyEntity verify = new EquEquipmentVerifyEntity();
                    verify.EquipmentId = entity.Id;
                    verify.Account = item.Account;
                    verify.Password = item.Password;
                    verify.AccountType = item.AccountType;

                    verify.Id = IdGenProvider.Instance.CreateId();
                    verify.SiteId = _currentSite.SiteId ?? 0;
                    verify.CreatedBy = _currentUser.UserName;
                    verify.CreatedOn = HymsonClock.Now();
                    verify.UpdatedBy = _currentUser.UserName;
                    verify.UpdatedOn = HymsonClock.Now();
                    verifyList.Add(verify);
                }
            }
            #endregion

            #region 参数校验
            var modelOrigin = await _equEquipmentRepository.GetByIdAsync(entity.Id);
            if (modelOrigin == null) throw new CustomerValidationException(nameof(ErrorCode.MES12603));
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 需要检查更新的字段
                rows += await _equEquipmentRepository.UpdateAsync(entity);

                // 绑定API数据
                rows += await _equEquipmentLinkApiRepository.DeletesAsync(entity.Id);
                rows += await _equEquipmentLinkApiRepository.InsertRangeAsync(linkApiList);

                // 绑定硬件数据
                rows += await _equEquipmentLinkHardwareRepository.DeletesAsync(entity.Id);
                rows += await _equEquipmentLinkHardwareRepository.InsertRangeAsync(linkHardwareList);

                rows += await _equEquipmentVerifyRepository.DeletesByEquipmentIdsAsync(new long[] { entity.Id });
                rows += await _equEquipmentVerifyRepository.InsertsAsync(verifyList);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equEquipmentLinkApiRepository.DeletesAsync(idsArr);
                rows += await _equEquipmentLinkHardwareRepository.DeletesAsync(idsArr);
                rows += await _equEquipmentVerifyRepository.DeletesByEquipmentIdsAsync(idsArr);
                rows += await _equEquipmentRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsArr,
                    UserId = _currentUser.UserName,
                    DeleteOn = HymsonClock.Now()
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentListDto>> GetPagedListAsync(EquEquipmentPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equEquipmentRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentListDto>());

            return new PagedInfo<EquEquipmentListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentDictionaryDto>> GetEquEquipmentDictionaryAsync()
        {
            var dics = new List<EquEquipmentDictionaryDto> { };
            var list = await _equEquipmentRepository.GetBaseListAsync(new EntityBySiteIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0
            });

            var equipmentTypeDic = list.ToLookup(g => g.EquipmentType);
            foreach (var item in equipmentTypeDic)
            {
                if (!item.Key.HasValue) continue;

                dics.Add(new EquEquipmentDictionaryDto
                {
                    EquipmentType = item.Key.Value,
                    Equipments = item.Select(s => new EquEquipmentBaseDto
                    {
                        Id = s.Id,
                        EquipmentCode = s.EquipmentCode,
                        EquipmentName = s.EquipmentName
                    })
                });
            }

            return dics;
        }

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentDto> GetDetailAsync(long id)
        {
            var equipmentDto = (await _equEquipmentRepository.GetByIdAsync(id)).ToModel<EquEquipmentDto>();

            if (equipmentDto != null)
            {
                //查询关联的资源
                var resources = await _procResourceRepository.GetByEquipmentIdsAsync(new Data.Repositories.Process.Resource.ProcResourceByEquipmentIdsQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    EquipmentIds = new long[] { equipmentDto.Id }
                });
                equipmentDto.ResourceCodes = string.Join(",", resources.Select(x => x.ResCode));
            }

            return equipmentDto??new EquEquipmentDto();
        }

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiBaseDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto parm)
        {
            // TODO 
            var pagedQuery = parm.ToQuery<EquEquipmentLinkApiPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equEquipmentLinkApiRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentLinkApiBaseDto>());
            return new PagedInfo<EquEquipmentLinkApiBaseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询设备关联硬件列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkHardwareBaseDto>> GetEquimentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto pagedQueryDto)
        {
            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkHardwarePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equEquipmentLinkHardwareRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentLinkHardwareBaseDto>());
            return new PagedInfo<EquEquipmentLinkHardwareBaseDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="apiLinks"></param>
        /// <param name="hardwareLinks"></param>
        /// <returns></returns>
        public static (IEnumerable<EquEquipmentLinkApiEntity> Apis, IEnumerable<EquEquipmentLinkHardwareEntity> Hardwares) ConvertToTupleList(EquEquipmentEntity entity, List<EquEquipmentLinkApiCreateDto> apiLinks, List<EquEquipmentLinkHardwareCreateDto> hardwareLinks)
        {
            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (apiLinks != null && apiLinks.Any())
            {
                foreach (var item in apiLinks)
                {
                    EquEquipmentLinkApiEntity linkApi = item.ToEntity<EquEquipmentLinkApiEntity>();
                    linkApi.EquipmentId = entity.Id;
                    linkApi.CreatedBy = entity.CreatedBy;
                    linkApi.UpdatedBy = entity.UpdatedBy;
                    linkApiList.Add(linkApi);
                }
            }

            // 绑定硬件
            List<EquEquipmentLinkHardwareEntity> linkHardwareList = new();
            if (hardwareLinks != null && hardwareLinks.Any())
            {
                foreach (var item in hardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardware.CreatedBy = entity.CreatedBy;
                    linkHardware.UpdatedBy = entity.UpdatedBy;
                    linkHardwareList.Add(linkHardware);
                }
            }

            return (linkApiList, linkHardwareList);
        }

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="EquipmentId"></param>
        /// <returns></returns>
        public async Task<string> CreateEquEquipmentTokenAsync(long EquipmentId)
        {
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(EquipmentId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12604));
            long siteId = 0;
            var equipmentModel = new EquipmentModel
            {
                FactoryId = _currentSite.SiteId ?? siteId,
                Id = equEquipmentEntity.Id,
                Name = equEquipmentEntity.EquipmentName,
                Code = equEquipmentEntity.EquipmentCode,
                SiteId = _currentSite.SiteId ?? siteId,
            };
            var token = JwtHelper.GenerateJwtToken(equipmentModel, _jwtOptions);

            var expirationTime = HymsonClock.Now().AddMinutes(_jwtOptions.ExpiresMinutes);
            var equEquipmentTokenEntity = await _equEquipmentTokenRepository.GetByEquipmentIdAsync(EquipmentId);
            if (equEquipmentTokenEntity != null)
            {
                equEquipmentTokenEntity.UpdatedBy = _currentUser.UserName;
                equEquipmentTokenEntity.UpdatedOn = HymsonClock.Now();
                equEquipmentTokenEntity.Token = token;
                equEquipmentTokenEntity.ExpirationTime = expirationTime;
                await _equEquipmentTokenRepository.UpdateAsync(equEquipmentTokenEntity);
            }
            else
            {
                //DTO转换实体 
                equEquipmentTokenEntity = new EquEquipmentTokenEntity
                {
                    EquipmentId = EquipmentId,
                    Token = token,
                    ExpirationTime = expirationTime,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? siteId
                };
                await _equEquipmentTokenRepository.InsertAsync(equEquipmentTokenEntity);
            }
            return equEquipmentTokenEntity.Token;
        }

        /// <summary>
        /// 查找Token
        /// </summary>
        /// <param name="EquipmentId"></param>
        /// <returns></returns>
        public async Task<string> GetEquEquipmentTokenAsync(long EquipmentId)
        {
            var equEquipmentTokenEntity = await _equEquipmentTokenRepository.GetByEquipmentIdAsync(EquipmentId);
            string token;
            if (equEquipmentTokenEntity == null)
            {
                token = await CreateEquEquipmentTokenAsync(EquipmentId);
            }
            else
            {
                token = equEquipmentTokenEntity.Token;
                if (HymsonClock.Now() > equEquipmentTokenEntity.ExpirationTime)
                {
                    token = await CreateEquEquipmentTokenAsync(EquipmentId);
                }
            }
            return token;
        }

        /// <summary>
        /// 根据设备ID查询对应的验证
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentVerifyDto>> GetEquipmentVerifyByEquipmentIdAsync(long equipmentId)
        {
            var verifyEntitys = await _equEquipmentVerifyRepository.GetEquipmentVerifyByEquipmentIdAsync(equipmentId);

            var verifyDtos = new List<EquEquipmentVerifyDto>();
            foreach (var item in verifyEntitys)
            {
                verifyDtos.Add(item.ToModel<EquEquipmentVerifyDto>());
            }
            return verifyDtos;
        }

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<string> CreateEquTokenAsync(long siteId)
        {
            EquQuery query = new EquQuery();
            query.SiteId = siteId;
            var equList = await _equEquipmentRepository.GetBySiteIdAsync(query);
            foreach (var item in equList)
            {
                await GetEquEquipmentTokenAsync(item.Id);
            }

            return "Ok";
        }

        #region 这里是供其他业务层调用的方法，个人觉得应该直接在其他业务层调用各业务仓储层
        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns></returns>
        public async Task<EquEquipmentDto> GetByEquipmentCodeAsync(string equipmentCode)
        {
            return (await _equEquipmentRepository.GetByEquipmentCodeAsync(new EntityByCodeQuery
            {
                Site = _currentSite.SiteId ?? 0,
                Code = equipmentCode.ToUpper()
            })).ToModel<EquEquipmentDto>();
        }

        /// <summary>
        /// 根据设备id+接口类型获取接口地址
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiDto> GetApiForEquipmentidAndType(long equipmentId, string apiType)
        {
            return (await _equEquipmentLinkApiRepository.GetByEquipmentIdAsync(equipmentId, apiType)).ToModel<EquEquipmentLinkApiDto>();
        }

        /// <summary>
        /// 根据硬件编码硬件类型获取设备
        /// </summary>
        /// <param name="hardwareCode"></param>
        /// <param name="hardwareType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkHardwareDto> GetLinkHardwareForCodeAndTypeAsync(string hardwareCode, string hardwareType)
        {
            return (await _equEquipmentLinkHardwareRepository.GetByHardwareCodeAsync(hardwareCode, hardwareType)).ToModel<EquEquipmentLinkHardwareDto>();
        }
        #endregion



    }
}