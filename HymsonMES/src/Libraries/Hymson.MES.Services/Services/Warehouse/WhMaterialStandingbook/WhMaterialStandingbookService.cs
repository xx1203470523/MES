/*
 *creator: Karl
 *
 *describe: 物料台账    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */
using Elastic.Clients.Elasticsearch.Core.Reindex;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Reactive;
using System.Transactions;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料台账 服务
    /// </summary>
    public class WhMaterialStandingbookService : IWhMaterialStandingbookService
    {
        private readonly ICurrentUser _currentUser;

        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 物料台账 仓储
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        private readonly AbstractValidator<WhMaterialStandingbookCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialStandingbookModifyDto> _validationModifyRules;

        private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        public WhMaterialStandingbookService(ICurrentUser currentUser, ICurrentSite currentSite,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            AbstractValidator<WhMaterialStandingbookCreateDto> validationCreateRules,
            AbstractValidator<WhMaterialStandingbookModifyDto> validationModifyRules,
            IWhSupplierRepository whSupplierRepository,
            ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService)
        {
            _currentUser = currentUser;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
            _whSupplierRepository = whSupplierRepository;
            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whMaterialStandingbookCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhMaterialStandingbookAsync(WhMaterialStandingbookCreateDto whMaterialStandingbookCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whMaterialStandingbookCreateDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookCreateDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _whMaterialStandingbookRepository.InsertAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhMaterialStandingbookAsync(long id)
        {
            await _whMaterialStandingbookRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhMaterialStandingbookAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _whMaterialStandingbookRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialStandingbookDto>> GetPageListAsync(WhMaterialStandingbookPagedQueryDto whMaterialStandingbookPagedQueryDto)
        {
            var whMaterialStandingbookPagedQuery = whMaterialStandingbookPagedQueryDto.ToQuery<WhMaterialStandingbookPagedQuery>();
            whMaterialStandingbookPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whMaterialStandingbookRepository.GetPagedInfoAsync(whMaterialStandingbookPagedQuery);

            //查询供应商
            var suppliers = await _whSupplierRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.SupplierId).ToArray());

            //实体到DTO转换 装载数据
            List<WhMaterialStandingbookDto> whMaterialStandingbookDtos = PrepareWhMaterialStandingbookDtos(pagedInfo);

            foreach (var item in whMaterialStandingbookDtos)
            {
                if (item.SupplierId > 0)
                {
                    item.SupplierCode = suppliers.FirstOrDefault(x => x.Id == item.SupplierId)?.Code ?? "";
                }
            }

            return new PagedInfo<WhMaterialStandingbookDto>(whMaterialStandingbookDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhMaterialStandingbookDto> PrepareWhMaterialStandingbookDtos(PagedInfo<WhMaterialStandingbookEntity> pagedInfo)
        {
            var whMaterialStandingbookDtos = new List<WhMaterialStandingbookDto>();
            foreach (var whMaterialStandingbookEntity in pagedInfo.Data)
            {
                var whMaterialStandingbookDto = whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
                whMaterialStandingbookDtos.Add(whMaterialStandingbookDto);
            }

            return whMaterialStandingbookDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialStandingbookModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyWhMaterialStandingbookAsync(WhMaterialStandingbookModifyDto whMaterialStandingbookModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whMaterialStandingbookModifyDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookModifyDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            await _whMaterialStandingbookRepository.UpdateAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialStandingbookDto> QueryWhMaterialStandingbookByIdAsync(long id)
        {
            var whMaterialStandingbookEntity = await _whMaterialStandingbookRepository.GetByIdAsync(id);
            if (whMaterialStandingbookEntity != null)
            {
                return whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
            }
            return new WhMaterialStandingbookDto();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<WhMaterialStandingbookExportResultDto> ExprotListAsync(WhMaterialStandingbookPagedQueryDto whMaterialStandingbookPagedQueryDto)
        {
            var whMaterialStandingbookPagedQuery = whMaterialStandingbookPagedQueryDto.ToQuery<WhMaterialStandingbookPagedQuery>();
            whMaterialStandingbookPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            whMaterialStandingbookPagedQuery.PageSize = 10000;
            var pagedInfo = await _whMaterialStandingbookRepository.GetPagedInfoAsync(whMaterialStandingbookPagedQuery);

            List<WhMaterialStandingbookExportDto> listDto = new List<WhMaterialStandingbookExportDto>();
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WhMaterialStandingbook"), _localizationService.GetResource("WhMaterialStandingbook"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new WhMaterialStandingbookExportResultDto
                {
                    FileName = _localizationService.GetResource("WhMaterialStandingbook"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            //查询供应商
            var suppliers = await _whSupplierRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.SupplierId).ToArray());

            //实体到DTO转换 装载数据
            List<WhMaterialStandingbookDto> whMaterialStandingbookDtos = PrepareWhMaterialStandingbookDtos(pagedInfo);

            foreach (var item in whMaterialStandingbookDtos)
            {
                if (item.SupplierId > 0)
                {
                    item.SupplierCode = suppliers.FirstOrDefault(x => x.Id == item.SupplierId)?.Code ?? "";
                }
                listDto.Add(new WhMaterialStandingbookExportDto
                {
                    MaterialBarCode=item.MaterialBarCode,
                    Batch=item.Batch,
                    Quantity=item.Quantity,
                    Unit = item.Unit,
                    MaterialName = item.MaterialName,
                    MaterialCode = item.MaterialCode,
                    MaterialVersion = item.MaterialVersion,
                    Type = item.Type,
                    Source=item.Source,
                    SupplierCode= suppliers.FirstOrDefault(x => x.Id == item.SupplierId)?.Code ?? "",
                    CreatedBy=item.CreatedBy,
                    CreatedOn=item.CreatedOn
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WhMaterialStandingbook"), _localizationService.GetResource("WhMaterialStandingbook"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new WhMaterialStandingbookExportResultDto
            {
                FileName = _localizationService.GetResource("WhMaterialStandingbook"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
