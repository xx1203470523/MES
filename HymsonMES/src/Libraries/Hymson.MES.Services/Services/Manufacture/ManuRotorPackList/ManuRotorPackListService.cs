using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Mavel.Rotor.PackList;
using Hymson.MES.Data.Repositories.QualFqcInspectionMaval;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（转子装箱记录表） 
    /// </summary>
    public class ManuRotorPackListService : IManuRotorPackListService
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
        private readonly AbstractValidator<ManuRotorPackListSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（转子装箱记录表）
        /// </summary>
        private readonly IManuRotorPackListRepository _manuRotorPackListRepository;

        private readonly ISequenceService _sequenceService;

        private readonly IQualFqcInspectionMavalRepository _qualFqcInspectionMavalRepository;

        private readonly IOptions<WMSOptions> _wmsOptions;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuRotorPackListService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuRotorPackListSaveDto> validationSaveRules,
            IManuRotorPackListRepository manuRotorPackListRepository, ISequenceService sequenceService,
            IQualFqcInspectionMavalRepository qualFqcInspectionMavalRepository,
            IOptions<WMSOptions> wmsOptions,
            IInteWorkCenterRepository inteWorkCenterRepository
            )
        {
            _sequenceService = sequenceService;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuRotorPackListRepository = manuRotorPackListRepository;
            _qualFqcInspectionMavalRepository = qualFqcInspectionMavalRepository;
            _wmsOptions = wmsOptions;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuRotorPackListSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuRotorPackListEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuRotorPackListRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuRotorPackListSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuRotorPackListEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuRotorPackListRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuRotorPackListRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuRotorPackListRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuRotorPackListDto?> QueryByIdAsync(long id)
        {
            var manuRotorPackListEntity = await _manuRotorPackListRepository.GetByIdAsync(id);
            if (manuRotorPackListEntity == null) return null;

            return manuRotorPackListEntity.ToModel<ManuRotorPackListDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuRotorPackListDto>> GetPagedListAsync(ManuRotorPackListPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuRotorPackListPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuRotorPackListRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuRotorPackListDto>());
            return new PagedInfo<ManuRotorPackListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRotorPackViewDto>> QueryByIdAsync(ManuRotorPackListQuery query)
        {
            List<ManuRotorPackViewDto> manuRotors = new();
            var manuRotorPackEntities = await _manuRotorPackListRepository.GetEntitiesAsync(new ManuRotorPackListQuery { BoxCode = query.BoxCode, Sfc = query.Sfc, SiteId = _currentSite.SiteId ?? 0 });
            if (manuRotorPackEntities.Any() == false) return manuRotors;

            var sfcs = manuRotorPackEntities.Select(x => x.ProductCode).ToList();
            var qualFqcs = await _qualFqcInspectionMavalRepository.GetQualFqcInspectionMavalEntitiesAsync(new QualFqcInspectionMavalQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = sfcs,
            });


            // 获取工作中心编码
            EntityByCodeQuery workQuery = new EntityByCodeQuery();
            workQuery.Site = _currentSite.SiteId;
            workQuery.Code = query.WorkCenterCode;
            var workCenterModel = await _inteWorkCenterRepository.GetByCodeAsync(workQuery);
            if (workQuery == null || string.IsNullOrEmpty(workCenterModel.LineCoding) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10541)).WithData("Code", query.WorkCenterCode);
            }
            string lineBrevityCode = workCenterModel.LineCoding!;

            string inspectionOrder = string.Empty;
            string dateStr = HymsonClock.Now().ToString("yyyyMMdd");
            string serialNumKey = $"{dateStr}{query.OrderCode}FAI";
            int curKeyNum = await _sequenceService.GetCurrentValueAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, serialNumKey);
            if (curKeyNum == 0)
            {
                var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, serialNumKey);
                inspectionOrder = $"{lineBrevityCode}{dateStr}{sequence.ToString().PadLeft(3, '0')}";
            }
            else
            {
                inspectionOrder = $"{lineBrevityCode}{dateStr}{curKeyNum.ToString().PadLeft(3, '0')}";
            }

            foreach (var item in manuRotorPackEntities)
            {
                //var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "FAI");
                //var InspectionOrder = $"{query.WorkCenterCode?.Substring(0, 2)}{DateTime.UtcNow.ToString("yyyyMMdd")}{sequence.ToString().PadLeft(3, '0')}";
                var qualFqc = qualFqcs.FirstOrDefault(x => x.SFC == item.ProductCode);
                var type = ProductReceiptQualifiedStatusEnum.ToBeBnspected;
                var WarehouseCode = "待检验仓";
                var whCode = _wmsOptions.Value.ProductReceipt.PendInspection;
                // type = ProductReceiptQualifiedStatusEnum.Qualified;
                if (qualFqc != null)
                {
                    if (qualFqc.JudgmentResults == FqcJudgmentResultsEnum.Unqualified)
                    {
                        WarehouseCode = "不良品仓";
                        type = ProductReceiptQualifiedStatusEnum.Unqualified;
                        whCode = _wmsOptions.Value.ProductReceipt.NgWarehouseCode;
                    }
                    if (qualFqc.JudgmentResults == FqcJudgmentResultsEnum.Qualified)
                    {
                        WarehouseCode = "成品仓";
                        type = ProductReceiptQualifiedStatusEnum.Qualified;
                        whCode = _wmsOptions.Value.ProductReceipt.FinishWarehouseCode;
                    }
                }
                var manuRotorPackView = new ManuRotorPackViewDto
                {
                    Sfc = item.ProductCode,
                    BoxCode = item.BoxCode,
                    Type = type,
                    Unit = "个",
                    Qty = 1,
                    WarehouseCode = WarehouseCode,
                    Batch = inspectionOrder,
                    WhCode = whCode,
                };
                manuRotors.Add(manuRotorPackView);
            };

            manuRotors.OrderBy(o => o.Sfc);
            return manuRotors;
        }
    }
}
