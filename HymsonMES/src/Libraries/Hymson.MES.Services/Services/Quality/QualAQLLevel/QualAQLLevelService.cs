using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（AQL检验水平）
    /// </summary>
    public class QualAQLLevelService : IQualAQLLevelService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        /// <param name="localizationService"></param
        /// <param name="sysConfigRepository"></param>
        public QualAQLLevelService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IExcelService excelService,
            IMinioService minioService,
            ILocalizationService localizationService,
            ISysConfigRepository sysConfigRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _excelService = excelService;
            _minioService = minioService;
            _localizationService = localizationService;
            _sysConfigRepository = sysConfigRepository;
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<QualAQLLevelExcelDto>> QueryListAsync()
        {
            var entities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = SysConfigEnum.AQLLevel
                //Codes = new List<string> { AQLStandard.MIL }
            });
            if (entities == null || !entities.Any()) return Array.Empty<QualAQLLevelExcelDto>();

            // 转换为导出模型
            List<QualAQLLevelExcelDto> dtos = new();
            foreach (var item in entities)
            {
                var ranges = item.Value.ToDeserialize<IEnumerable<QualAQLLevelExcelDto>>();
                if (ranges == null) continue;

                dtos.AddRange(ranges);
            }
            if (dtos == null || !dtos.Any()) return Array.Empty<QualAQLLevelExcelDto>();

            return dtos;
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadImportTemplateAsync(Stream stream)
        {
            var worksheetName = "AQL检验水平";
            await _excelService.ExportAsync(Array.Empty<QualAQLLevelExcelDto>(), stream, worksheetName);
            return worksheetName;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<QualAQLLevelExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            // 分组标准
            var standardDict = dtos.ToLookup(x => x.Standard).ToDictionary(d => d.Key, d => d);

            var time = HymsonClock.Now();
            List<SysConfigEntity> entities = new();
            foreach (var item in standardDict)
            {
                var standard = new SysConfigEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    Type = SysConfigEnum.AQLLevel,
                    Code = $"{item.Key}",
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = time,
                    SiteId = _currentSite.SiteId ?? 0
                };

                // 不能有重复（批次最小数量）
                var minDict = item.Value.DistinctBy(x => x.Min);
                if (minDict.Count() < item.Value.Count()) throw new CustomerValidationException(nameof(ErrorCode.MES19401));

                // 不能有重复（批次最大数量）
                var maxDict = item.Value.DistinctBy(x => x.Max);
                if (maxDict.Count() < item.Value.Count()) throw new CustomerValidationException(nameof(ErrorCode.MES19402));

                // 正向排序（按批次最小数量）
                var ranges = item.Value.OrderBy(o => o.Min);
                foreach (var range in ranges)
                {
                    // 存在跟其他水平交叉（批次最小数量）
                    if (ranges.Any(a => a.Min < range.Min && range.Min <= a.Max))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19403)).WithData("Value", range.Min);
                    }

                    // 存在跟其他水平交叉（批次最大数量）
                    if (ranges.Any(a => a.Min <= range.Max && range.Max < a.Max))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19404)).WithData("Value", range.Max);
                    }
                }

                standard.Value = ranges.ToSerialize();
                entities.Add(standard);
            }

            await _sysConfigRepository.InsertRangeAsync(entities);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ExportResponseDto> ExprotAsync(QualAQLLevelExprotRequestDto dto)
        {
            var entities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = SysConfigEnum.AQLLevel,
                Codes = dto.Codes
            });
            if (entities == null || !entities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10134));

            // 转换为导出模型
            List<QualAQLLevelExcelDto> dtos = new();
            foreach (var item in entities)
            {
                var ranges = item.Value.ToDeserialize<IEnumerable<QualAQLLevelExcelDto>>();
                if (ranges == null) continue;

                dtos.AddRange(ranges);
            }
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10134));

            // 上传到文件服务器
            var fileName = _localizationService.GetResource("QualAQLLevel");
            var filePath = await _excelService.ExportAsync(dtos, fileName, fileName);
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResponseDto
            {
                FileName = fileName,
                Path = uploadResult.AbsoluteUrl,
            };
        }

    }
}
