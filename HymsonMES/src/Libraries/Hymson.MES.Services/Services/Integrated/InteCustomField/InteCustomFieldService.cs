using FluentValidation;

using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.Attributes;
using Org.BouncyCastle.Utilities;
using System.Linq;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（自定义字段） 
    /// </summary>
    public class InteCustomFieldService : IInteCustomFieldService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<InteCustomFieldSaveDto> _validationSaveRules;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<InteCustomFieldInternationalizationDto> _internationalizationDtovalidationRules;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<InteCustomFieldBusinessEffectuateDto> _businessEffectuateDtovalidationRules;

        /// <summary>
        /// 仓储接口（自定义字段）
        /// </summary>
        private readonly IInteCustomFieldRepository _inteCustomFieldRepository;

        private readonly IInteCustomFieldInternationalizationRepository _inteCustomFieldInternationalizationRepository;

        private readonly IInteCustomFieldBusinessEffectuateRepository _inteCustomFieldBusinessEffectuateRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteCustomFieldRepository"></param>
        /// <param name="inteCustomFieldInternationalizationRepository"></param>
        /// <param name="internationalizationDtovalidationRules"></param>
        /// <param name="businessEffectuateDtovalidationRules"></param>
        /// <param name="inteCustomFieldBusinessEffectuateRepository"></param>
        public InteCustomFieldService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteCustomFieldSaveDto> validationSaveRules, 
            IInteCustomFieldRepository inteCustomFieldRepository, IInteCustomFieldInternationalizationRepository inteCustomFieldInternationalizationRepository, AbstractValidator<InteCustomFieldInternationalizationDto> internationalizationDtovalidationRules, AbstractValidator<InteCustomFieldBusinessEffectuateDto> businessEffectuateDtovalidationRules, IInteCustomFieldBusinessEffectuateRepository inteCustomFieldBusinessEffectuateRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteCustomFieldRepository = inteCustomFieldRepository;
            _inteCustomFieldInternationalizationRepository = inteCustomFieldInternationalizationRepository;
            _internationalizationDtovalidationRules = internationalizationDtovalidationRules;
            _businessEffectuateDtovalidationRules = businessEffectuateDtovalidationRules;
            _inteCustomFieldBusinessEffectuateRepository = inteCustomFieldBusinessEffectuateRepository;
        }

        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <param name="saveDtos"></param>
        /// <returns></returns>
        public async Task AddOrUpdateAsync(IEnumerable<InteCustomFieldSaveDto> saveDtos)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            if (saveDtos == null || !saveDtos.Any()) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15602));
            }

            foreach (var item in saveDtos)
            {
                // 验证DTO
                await _validationSaveRules.ValidateAndThrowAsync(item);

                if (item.Languages != null) 
                {
                    foreach (var language in item.Languages)
                    {
                        await _internationalizationDtovalidationRules.ValidateAndThrowAsync(language);
                    }

                    //验证语言类型只能有一行数据
                    if (item.Languages.GroupBy(x => x.LanguageType).ToDictionary(g => g.Key, g => g.ToArray()).Any(x => x.Value.Count() > 1)) 
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15620));
                    }
                }
            }

            //验证是否有其他的业务类型
            var businessTypes= saveDtos.Select(x => x.BusinessType).Distinct();
            if (businessTypes.Count() > 1) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15601));
            }
            var currentBusinessType= businessTypes.FirstOrDefault();

            //验证重复
            var duplicateNames = saveDtos.GroupBy(x => x.Name)
                                     .Where(group => group.Count() > 1)
                                     .Select(group => group.Key);
            if (duplicateNames.Any()) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15603)).WithData("name", string.Join(",",duplicateNames));
            }

            //找到之前的字段信息
            var oldInteCustomFields = await _inteCustomFieldRepository.GetEntitiesByBusinessTypeAsync(new InteCustomFieldByBusinessQuery { SiteId=_currentSite.SiteId??0,BusinessType= currentBusinessType });

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            List<InteCustomFieldEntity> inteCustomFieldEntities = new List<InteCustomFieldEntity>();
            List<InteCustomFieldInternationalizationEntity> inteCustomFieldInternationalizationEntities = new List<InteCustomFieldInternationalizationEntity>();
            foreach (var item in saveDtos) 
            {
                var inteCustomFieldEntityId = IdGenProvider.Instance.CreateId();

                inteCustomFieldEntities.Add(new InteCustomFieldEntity () 
                {
                    Id= inteCustomFieldEntityId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = _currentSite.SiteId ?? 0,

                    BusinessType=item.BusinessType,
                    Name= item.Name,
                    Remark= item.Remark,

                });

                if (item.Languages != null)
                {
                    foreach (var internationalizationDto in item.Languages)
                    {
                        inteCustomFieldInternationalizationEntities.Add(new InteCustomFieldInternationalizationEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn,
                            SiteId = _currentSite.SiteId ?? 0,

                            CustomFieldId = inteCustomFieldEntityId,
                            LanguageType = internationalizationDto.LanguageType,
                            TranslationValue = internationalizationDto.TranslationValue,

                        });
                    }
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (oldInteCustomFields != null && oldInteCustomFields.Any())
                {
                    //删除
                    await _inteCustomFieldRepository.DeletesTrueAsync(new Data.Repositories.Common.Command.DeleteCommand()
                    {
                        Ids = oldInteCustomFields.Select(x => x.Id).Distinct()
                    });

                    await _inteCustomFieldInternationalizationRepository.DeletedByCustomFieldIdsAsync(new InternationalizationDeleteByCustomFieldIdsCommand
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        CustomFieldIds = oldInteCustomFields.Select(x => x.Id).Distinct()
                    });
                }

                // 保存
                await _inteCustomFieldRepository.InsertRangeAsync(inteCustomFieldEntities);

                await _inteCustomFieldInternationalizationRepository.InsertRangeAsync(inteCustomFieldInternationalizationEntities);

                ts.Complete();
            }
        }

        /// <summary>
        /// 获取业务类型下的自定义字段信息（包含对应的语言设置）
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldDto>> GetDataByBusinessTypeAsync(InteCustomFieldBusinessTypeEnum businessType) 
        {
            if(!System.Enum.IsDefined(typeof(InteCustomFieldBusinessTypeEnum), businessType)) throw new CustomerValidationException(nameof(ErrorCode.MES15604));

            List<InteCustomFieldDto> result=new List<InteCustomFieldDto>();

            var inteCustomFields = await _inteCustomFieldRepository.GetEntitiesByBusinessTypeAsync(new InteCustomFieldByBusinessQuery { SiteId = _currentSite.SiteId ?? 0, BusinessType = businessType });

            if (inteCustomFields == null || !inteCustomFields.Any()) return result;

            //找到对应的语言
            var inteCustomFieldInternationalizations=  await _inteCustomFieldInternationalizationRepository.GetEntitiesByCustomFieldIdsAsync(new InteCustomFieldInternationalizationByCustomFieldIdsQuery {SiteId=_currentSite.SiteId??0, CustomFieldIds= inteCustomFields.Select(x=>x.Id).ToArray() });

            foreach (var item in inteCustomFields) 
            {
                var languages= inteCustomFieldInternationalizations?.Where(x => x.CustomFieldId == item.Id);

                result.Add(new InteCustomFieldDto
                {
                    BusinessType = businessType,
                    Name = item.Name,
                    Remark = item.Remark,

                    Languages = languages?.Select(x => new InteCustomFieldInternationalizationDto { LanguageType=x.LanguageType, TranslationValue=x.TranslationValue })
                }) ;
            }

            return result;
        }

        /// <summary>
        /// 保存各个业务ID的自定义字段数据
        /// </summary>
        /// <param name="saveBusinessDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task SaveBusinessDataAsync(IEnumerable<InteCustomFieldBusinessEffectuateDto> saveBusinessDtos)
        {
            if (saveBusinessDtos == null || !saveBusinessDtos.Any()) return;

            foreach (var item in saveBusinessDtos) 
            {
                await _businessEffectuateDtovalidationRules.ValidateAndThrowAsync(item);
            }

            //检测是否同一个businessID与BusinessTYpe
            if (saveBusinessDtos.GroupBy(x => x.BusinessType ).ToList().Count()>1) throw new CustomerValidationException(nameof(ErrorCode.MES15619));
            if (saveBusinessDtos.GroupBy(x => x.BusinessId ).ToList().Count() > 1) throw new CustomerValidationException(nameof(ErrorCode.MES15618));

            var currentBusinessId = saveBusinessDtos.First().BusinessId;

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //组装数据
            List<InteCustomFieldBusinessEffectuateEntity> list = new List<InteCustomFieldBusinessEffectuateEntity>();
            foreach (var item in saveBusinessDtos)
            {
                list.Add(new InteCustomFieldBusinessEffectuateEntity() 
                {
                    Id= IdGenProvider.Instance.CreateId(),
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = _currentSite.SiteId ?? 0,

                    BusinessId = item.BusinessId,
                    BusinessType = item.BusinessType,
                    CustomFieldName = item.CustomFieldName,
                    SetValue = item.SetValue,
                });
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _inteCustomFieldBusinessEffectuateRepository.DeleteTrueByBusinessIdAsync(currentBusinessId);

                await _inteCustomFieldBusinessEffectuateRepository.InsertRangeAsync(list);

                ts.Complete();
            }
        }

        /// <summary>
        /// 根据业务ID获取业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<IEnumerable<InteCustomFieldBusinessEffectuateDto>> GetBusinessEffectuatesAsync(long businessId) 
        {
            if(businessId==0) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            List<InteCustomFieldBusinessEffectuateDto> result = new List<InteCustomFieldBusinessEffectuateDto>();

            var businessEffectuates = await _inteCustomFieldBusinessEffectuateRepository.GetBusinessEffectuatesByBusinessIdAsync(businessId);

            foreach (var item in businessEffectuates)
            {
                result.Add(item.ToModel<InteCustomFieldBusinessEffectuateDto>());
            }

            return result;
        }

        /// <summary>
        /// 根据业务ID删除业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> DelBusinessEffectuatesAsync(long[] businessId)
        {
            if (businessId.Length == 0) 
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            return await _inteCustomFieldBusinessEffectuateRepository.DeleteTrueByBusinessIdsAsync(businessId);
        }
    }
}
