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
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Options;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using Microsoft.Extensions.Options;
using Minio.DataModel;
using System.Data.SqlTypes;

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

        private readonly IEquEquipmentTheoryRepository _equEquipmentTheoryRepository;

        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="equEquipmentLinkApiRepository"></param>
        /// <param name="equEquipmentLinkHardwareRepository"></param>
        /// <param name="equEquipmentTokenRepository"></param>
        /// <param name="equEquipmentTheoryRepository"></param>
        /// <param name="jwtOptions"></param>
        public EquEquipmentService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquEquipmentSaveDto> validationSaveRules,
            IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentLinkApiRepository equEquipmentLinkApiRepository,
            IEquEquipmentLinkHardwareRepository equEquipmentLinkHardwareRepository,
            IEquEquipmentTokenRepository equEquipmentTokenRepository,
            IEquEquipmentTheoryRepository equEquipmentTheoryRepository,
            IOptions<JwtOptions> jwtOptions)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentLinkApiRepository = equEquipmentLinkApiRepository;
            _equEquipmentLinkHardwareRepository = equEquipmentLinkHardwareRepository;
            _equEquipmentTokenRepository = equEquipmentTokenRepository;
            _equEquipmentTheoryRepository = equEquipmentTheoryRepository;
            _jwtOptions = jwtOptions.Value;
        }


        /// <summary>
        /// 添加（设备注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquEquipmentSaveDto createDto)
        {
            #region 参数处理
            // 验证DTO
            createDto.EquipmentCode = createDto.EquipmentCode.ToTrimSpace();
            createDto.EquipmentCode = createDto.EquipmentCode.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            if (string.IsNullOrEmpty(createDto.EntryDate) == true) createDto.EntryDate = SqlDateTime.MinValue.Value.ToString();

            // DTO转换实体
            var entity = createDto.ToEntity<EquEquipmentEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 123456;

            if (entity.QualTime > 0 && entity.EntryDate > SqlDateTime.MinValue.Value) entity.ExpireDate = entity.EntryDate.AddMonths(entity.QualTime);

            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (createDto.ApiLinks != null && createDto.ApiLinks.Any() == true)
            {
                foreach (var item in createDto.ApiLinks)
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
            if (createDto.HardwareLinks != null && createDto.HardwareLinks.Any() == true)
            {
                foreach (var item in createDto.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardware.CreatedBy = entity.CreatedBy;
                    linkHardware.UpdatedBy = entity.UpdatedBy;
                    linkHardwareList.Add(linkHardware);
                }
            }
            #endregion

            #region 参数校验
            // 编码唯一性验证
            var checkEntity = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.EquipmentCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12600)).WithData("Code", entity.EquipmentCode);
            #endregion

            EquEquipmentTheoryCreateCommand theoryCreateCommand = new();
            //新增理论产出数
            if (createDto.TheoryOutPutQty != null)
            {
                theoryCreateCommand.Id = IdGenProvider.Instance.CreateId();
                theoryCreateCommand.CreatedBy = _currentUser.UserName;
                theoryCreateCommand.CreatedOn = HymsonClock.Now();
                theoryCreateCommand.UpdatedBy = _currentUser.UserName;
                theoryCreateCommand.UpdatedOn = HymsonClock.Now();
                theoryCreateCommand.SiteId = _currentSite.SiteId ?? 123456;
                theoryCreateCommand.EquipmentCode = createDto.EquipmentCode;
                theoryCreateCommand.OutputQty = createDto.TheoryOutPutQty;
                theoryCreateCommand.TheoryOutputQty = createDto.TheoryOutPutQty;
                theoryCreateCommand.TheoryOnTime = createDto.TheoryOnTime;
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equEquipmentRepository.InsertAsync(entity);
                rows += await _equEquipmentLinkApiRepository.InsertRangeAsync(linkApiList);
                rows += await _equEquipmentLinkHardwareRepository.InsertRangeAsync(linkHardwareList);
                rows += await _equEquipmentTheoryRepository.InsertAsync(theoryCreateCommand);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改（设备注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquEquipmentSaveDto modifyDto)
        {
            #region 参数处理
            if (string.IsNullOrEmpty(modifyDto.EntryDate) == true) modifyDto.EntryDate = SqlDateTime.MinValue.Value.ToString();
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquEquipmentEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            if (entity.QualTime > 0 && entity.EntryDate > SqlDateTime.MinValue.Value) entity.ExpireDate = entity.EntryDate.AddMonths(entity.QualTime);

            // 绑定Api
            List<EquEquipmentLinkApiEntity> linkApiList = new();
            if (modifyDto.ApiLinks != null && modifyDto.ApiLinks.Any() == true)
            {
                foreach (var item in modifyDto.ApiLinks)
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
            if (modifyDto.HardwareLinks != null && modifyDto.HardwareLinks.Any() == true)
            {
                foreach (var item in modifyDto.HardwareLinks)
                {
                    EquEquipmentLinkHardwareEntity linkHardware = item.ToEntity<EquEquipmentLinkHardwareEntity>();
                    linkHardware.EquipmentId = entity.Id;
                    linkHardware.CreatedBy = entity.CreatedBy;
                    linkHardware.UpdatedBy = entity.UpdatedBy;
                    linkHardwareList.Add(linkHardware);
                }
            }

            EquEquipmentTheoryUpdateCommand theoryUpdateCommand = new();
            //新增理论产出数
            if (modifyDto.TheoryOutPutQty != null)
            {
                theoryUpdateCommand.Id = IdGenProvider.Instance.CreateId();
                theoryUpdateCommand.CreatedBy = _currentUser.UserName;
                theoryUpdateCommand.CreatedOn = HymsonClock.Now();
                theoryUpdateCommand.UpdatedBy = _currentUser.UserName;
                theoryUpdateCommand.UpdatedOn = HymsonClock.Now();
                theoryUpdateCommand.SiteId = _currentSite.SiteId ?? 123456;
                theoryUpdateCommand.EquipmentCode = modifyDto.EquipmentCode;
                theoryUpdateCommand.OutputQty = modifyDto.TheoryOutPutQty;
                theoryUpdateCommand.TheoryOutputQty = modifyDto.TheoryOutPutQty;
                theoryUpdateCommand.TheoryOnTime = modifyDto.TheoryOnTime;
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

                rows += await _equEquipmentTheoryRepository.InsertOrUpdateAsync(theoryUpdateCommand);
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
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;

            var equipmentTheoryEntities = await _equEquipmentTheoryRepository.GetListAsync(new() { EquipmentCodes = pagedQueryDto.EquipmentCodes });

            var pagedInfo = await _equEquipmentRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentListDto>());
            foreach (var item in dtos)
            {
                var theoryEntity = equipmentTheoryEntities.FirstOrDefault(a => a.EquipmentCode == item.EquipmentCode);

                if (theoryEntity != null)
                {
                    item.TheoryOutputQty = theoryEntity.TheoryOutputQty.GetValueOrDefault();
                    item.TheoryOnTime = theoryEntity?.TheoryOnTime ?? 0;
                }
            }

            return new PagedInfo<EquEquipmentListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询列表（设备注册）
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentDictionaryDto>> GetEquEquipmentDictionaryAsync()
        {
            var dics = new List<EquEquipmentDictionaryDto> { };
            //TODO SiteId
            var list = await _equEquipmentRepository.GetBaseListAsync();
            var equipmentTypeDic = list.ToLookup(g => g.EquipmentType);
            foreach (var item in equipmentTypeDic)
            {
                if (item.Key.HasValue == false) continue;

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
            var result = (await _equEquipmentRepository.GetByIdAsync(id)).ToModel<EquEquipmentDto>();

            var equipmentTheoryEntity = await _equEquipmentTheoryRepository.GetOneAsync(new() {  EquipmentCode = result .EquipmentCode});

            result.TheoryOutputQty = equipmentTheoryEntity?.TheoryOutputQty.GetValueOrDefault();
            result.TheoryOnTime = equipmentTheoryEntity?.TheoryOnTime ?? 0;

            return result;
        }

        /// <summary>
        /// 查询设备关联API列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiBaseDto>> GetEquimentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto pagedQueryDto)
        {
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkApiRepository.Queryable()
                 .OrderByDescending(x => x.UpdatedOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkApiDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     ApiUrl = x.ApiUrl,
                     ApiType = x.ApiType,
                     Remark = x.Remark,
                     CreatedBy = x.CreatedBy,
                     CreatedOn = x.CreatedOn,
                     UpdatedBy = x.UpdatedBy,
                     UpdatedOn = x.UpdatedOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkApiPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
            /*
            //搜索条件查询语法参考Sqlsugar
            var response = await _equEquipmentLinkHardwareRepository.Queryable()
                 .OrderByDescending(x => x.UpdatedOn)
                 .Where((x) => x.EquipmentId == parm.EquipmentId && !x.IsDeleted)
                 .Select(x => new QueryEquipmentLinkHardwareDto
                 {
                     Id = x.Id,
                     SiteCode = x.SiteCode,
                     EquipmentId = x.EquipmentId,
                     HardwareCode = x.HardwareCode,
                     HardwareType = x.HardwareType,
                     Remark = x.Remark,
                     CreatedBy = x.CreatedBy,
                     CreatedOn = x.CreatedOn,
                     UpdatedBy = x.UpdatedBy,
                     UpdatedOn = x.UpdatedOn
                 })
                 .ToPageAsync(parm);

            return response;
            */

            // TODO 
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentLinkHardwarePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
            if (apiLinks != null && apiLinks.Any() == true)
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
            if (hardwareLinks != null && hardwareLinks.Any() == true)
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(EquipmentId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12604));
            var equipmentModel = new EquipmentModel
            {
                FactoryId = _currentSite.SiteId ?? 123456,
                Id = equEquipmentEntity.Id,
                Name = equEquipmentEntity.EquipmentName,
                Code = equEquipmentEntity.EquipmentCode,
                SiteId = _currentSite.SiteId ?? 123456,
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
                    SiteId = _currentSite.SiteId ?? 123456
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



        #region 这里是供其他业务层调用的方法，个人觉得应该直接在其他业务层调用各业务仓储层

        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> GetByEquipmentEntityCodeAsync(string equipmentCode)
        {
            return await _equEquipmentRepository.GetByEquipmentCodeAsync(new EntityByCodeQuery
            {
                Site = _currentSite.SiteId ?? 123456,
                Code = equipmentCode.ToUpper()
            });
        }

        /// <summary>
        /// 查询设备（单个）
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns></returns>
        public async Task<EquEquipmentDto> GetByEquipmentCodeAsync(string equipmentCode)
        {
            return (await _equEquipmentRepository.GetByEquipmentCodeAsync(new EntityByCodeQuery
            {
                Site = _currentSite.SiteId ?? 123456,
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