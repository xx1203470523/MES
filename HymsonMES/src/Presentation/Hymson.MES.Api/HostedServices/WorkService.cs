using Hymson.ClearCache;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Enums;
using Hymson.Localization.Domain;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.Snowflake;
using Hymson.Utils;
using OfficeOpenXml.Attributes;
using System.Reflection;

namespace Hymson.MES.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkService : BackgroundService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<WorkService> _logger;
        private readonly IResourceService _resourceService;
        private readonly IClearCacheService _clearCacheService;
        private readonly IResourceRepository _resourceRepository;
        private readonly ILanguageRepository _languageRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceService"></param>
        /// <param name="logger"></param>
        /// <param name="clearCacheService"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="languageRepository"></param>
        public WorkService(ILogger<WorkService> logger,
            IResourceService resourceService,
            IClearCacheService clearCacheService,
            IResourceRepository resourceRepository,
            ILanguageRepository languageRepository)
        {
            _logger = logger;
            _resourceService = resourceService;
            _clearCacheService = clearCacheService;
            _resourceRepository = resourceRepository;
            _languageRepository = languageRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] {
                 ServiceTypeEnum.User,
                 ServiceTypeEnum.MES
                }, stoppingToken);
                //await _resourceService.InitEnumAsync();
                //await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));
                //await InitExcelDtoAsync();
                //await _resourceService.HotLoadingAsync();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "初始化失败");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task InitExcelDtoAsync()
        {
            var types = GetExcelDtoTypes();
            var languageEntities = await _languageRepository.GetAllLanguagesAsync();
            foreach (var type in types)
            {
                var propertyInfos = type.GetProperties();
                foreach (var propertyInfo in propertyInfos)
                {
                    var epplusTableColumnAttribute = propertyInfo.GetCustomAttribute<EpplusTableColumnAttribute>();
                    if (epplusTableColumnAttribute == null) { continue; }
                    var resourceName = $"{type.Namespace}.{type.Name}.{propertyInfo.Name}";
                    var resourceValue = epplusTableColumnAttribute.Header;
                    foreach (var languageEntity in languageEntities.Select(x => x.Id))
                    {
                        var localeStringResourceEntity = new LocaleStringResourceEntity
                        {
                            CreatedBy = "System",
                            CreatedOn = HymsonClock.Now(),
                            IsDeleted = 0,
                            Id = IdGenProvider.Instance.CreateId(),
                            LanguageId = languageEntity,
                            ResourceName = resourceName,
                            ResourceValue = resourceValue,
                            ServiceType = Infrastructure.Enums.ServiceTypeEnum.MES,
                            UpdatedBy = "System",
                            UpdatedOn = HymsonClock.Now(),
                        };
                        if (languageEntity == 1)
                        {
                            await _resourceRepository.InsertOrUpdateAsync(localeStringResourceEntity);
                        }
                        else
                        {
                            await _resourceRepository.InsertIgnoreAsync(localeStringResourceEntity);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetExcelDtoTypes()
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var assemblies = typeFinder.GetAssemblies();
            var allExcelDtoTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(x => x.BaseType == typeof(BaseExcelDto));
                allExcelDtoTypes.AddRange(types);
            }
            return allExcelDtoTypes;
        }


    }
}