using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteBusinessField.View;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text.RegularExpressions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（字段定义） 
    /// </summary>
    public class InteBusinessFieldService : IInteBusinessFieldService
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
        private readonly AbstractValidator<InteBusinessFieldSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（字段定义）
        /// </summary>
        private readonly IInteBusinessFieldRepository _inteBusinessFieldRepository;

        /// <summary>
        /// 仓储接口（字段定义列表数据）
        /// </summary>
        private readonly IInteBusinessFieldListRepository _inteBusinessFieldListRepository;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        private readonly IProcMaskCodeRepository _procMaskCodeRepository;


        private readonly IInteBusinessFieldDistributeDetailsRepository _inteBusinessFieldDistributeDetailsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteBusinessFieldRepository"></param>
        /// <param name="inteBusinessFieldListRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procMaskCodeRepository"></param>
        /// <param name="inteBusinessFieldDistributeDetailsRepository"></param>
        public InteBusinessFieldService(
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<InteBusinessFieldSaveDto> validationSaveRules,
            IInteBusinessFieldRepository inteBusinessFieldRepository,
            IInteBusinessFieldListRepository inteBusinessFieldListRepository,
            IMasterDataService masterDataService,
            IProcMaskCodeRepository procMaskCodeRepository,
            IInteBusinessFieldDistributeDetailsRepository inteBusinessFieldDistributeDetailsRepository
            )
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteBusinessFieldRepository = inteBusinessFieldRepository;
            _inteBusinessFieldListRepository = inteBusinessFieldListRepository;
            _masterDataService = masterDataService;
            _procMaskCodeRepository = procMaskCodeRepository;
            _inteBusinessFieldDistributeDetailsRepository = inteBusinessFieldDistributeDetailsRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(InteBusinessFieldSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var inteBusinessFieldEntity = saveDto.ToEntity<InteBusinessFieldEntity>();
            inteBusinessFieldEntity.Id = IdGenProvider.Instance.CreateId();
            inteBusinessFieldEntity.CreatedBy = updatedBy;
            inteBusinessFieldEntity.CreatedOn = updatedOn;
            inteBusinessFieldEntity.UpdatedBy = updatedBy;
            inteBusinessFieldEntity.UpdatedOn = updatedOn;
            inteBusinessFieldEntity.SiteId = _currentSite.SiteId ?? 0;


            // 验证是否编码唯一
            var entity = await _inteBusinessFieldRepository.GetByCodeAsync(new InteBusinessFieldQuery
            {
                Code = inteBusinessFieldEntity.Code.Trim(),
                SiteId = _currentSite.SiteId ?? 0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19441));
            }
            //if (saveDto.Type == FieldDefinitionTypeEnum.Number || saveDto.Type == FieldDefinitionTypeEnum.Text)
            //{
            //    // 物料未设置掩码
            //    if (!saveDto.MaskCodeId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19431)).WithData("code", inteBusinessFieldEntity.Code);

            //    // 未设置规则
            //    var maskCodeRules = await _masterDataService.GetMaskCodeRuleEntitiesByMaskCodeIdAsync(saveDto.MaskCodeId.Value);
            //    if (maskCodeRules == null || !maskCodeRules.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19431)).WithData("code", inteBusinessFieldEntity.Code);
            //    var isCodeRule = VerifyCode(inteBusinessFieldEntity.Code, maskCodeRules);
            //    if (!isCodeRule)
            //    {
            //        throw new CustomerValidationException(nameof(ErrorCode.MES19432)).WithData("code", inteBusinessFieldEntity.Code);
            //    }
            //}
            
            #region 处理 载具类型验证数据
            List<InteBusinessFieldListEntity> detailEntities = new();
            if (saveDto.inteBusinessFieldList != null && saveDto.inteBusinessFieldList.Any())
            {
                foreach (var item in saveDto.inteBusinessFieldList)
                {
                    //验证数据
                    var pattern = @"^[1-9]\d*$";
                    if (!Regex.IsMatch($"{item.Seq}", pattern)) throw new CustomerValidationException(nameof(ErrorCode.MES19429));
                    var ismianCount = saveDto.inteBusinessFieldList.Where(a => a.ISDefault).ToList().Count;
                    if (ismianCount > 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19424));
                    }

                    var isSeqCount = saveDto.inteBusinessFieldList.GroupBy(p => p.Seq)
                                         .Any(g => g.Count() > 1);
                    if (isSeqCount)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19430));
                    }

                    var patternFieldLabel = @"^[A-Z0-9_]+$";
                    if (!Regex.IsMatch($"{item.FieldLabel}", patternFieldLabel)) throw new CustomerValidationException(nameof(ErrorCode.MES19434));
                    detailEntities.Add(new InteBusinessFieldListEntity()
                    {
                        BusinessFieldId = inteBusinessFieldEntity.Id,
                        Seq = item.Seq,
                        FieldLabel = item.FieldLabel,
                        FieldValue = item.FieldValue,
                        iSDefault=item.ISDefault,
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
            await _inteBusinessFieldRepository.InsertAsync(inteBusinessFieldEntity);
            if (detailEntities.Any())
            {
                await _inteBusinessFieldListRepository.InsertRangeAsync(detailEntities);
            }
            ts.Complete();
            return inteBusinessFieldEntity.Id;
        }

        /// <summary>
        /// 验证数据字段掩码规则
        /// </summary>
        /// <param name="code"></param>
        /// <param name="maskCodeRules"></param>
        /// <returns></returns>
        public static bool VerifyCode(string code, IEnumerable<ProcMaskCodeRuleEntity> maskCodeRules)
        {
            // 对掩码规则进行校验
            foreach (var ruleEntity in maskCodeRules)
            {
                var rule = Regex.Replace(ruleEntity.Rule, "[?？]", ".");
                var pattern = $"{rule}";

                switch (ruleEntity.MatchWay)
                {
                    case MatchModeEnum.Start:
                        pattern = $"^{rule}.+";
                        break;
                    case MatchModeEnum.Middle:
                        pattern = $".+{rule}.+";
                        break;
                    case MatchModeEnum.End:
                        pattern = $".+{rule}$";
                        break;
                    case MatchModeEnum.Whole:
                        pattern = $"^{pattern}$";
                        break;
                    default:
                        break;
                }

                if (!Regex.IsMatch(code, pattern)) return false;
            }

            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task ModifyAsync(InteBusinessFieldSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<InteBusinessFieldEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            //if (saveDto.Type == FieldDefinitionTypeEnum.Number || saveDto.Type == FieldDefinitionTypeEnum.Text)
            //{
            //    // 物料未设置掩码
            //    if (!saveDto.MaskCodeId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19431)).WithData("code", entity.Code);

            //    // 未设置规则
            //    var maskCodeRules = await _masterDataService.GetMaskCodeRuleEntitiesByMaskCodeIdAsync(saveDto.MaskCodeId.Value);
            //    if (maskCodeRules == null || !maskCodeRules.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19431)).WithData("code", entity.Code);
            //    var isCodeRule = VerifyCode(entity.Code, maskCodeRules);
            //    if (!isCodeRule)
            //    {
            //        throw new CustomerValidationException(nameof(ErrorCode.MES19432)).WithData("code", entity.Code);
            //    }
            //}
            #region 处理 载具类型验证数据
            List<InteBusinessFieldListEntity> detailEntities = new();
            if (saveDto.inteBusinessFieldList != null && saveDto.inteBusinessFieldList.Any())
            {
                foreach (var item in saveDto.inteBusinessFieldList)
                {
                    //验证数据
                    var pattern = @"^[1-9]\d*$";
                    if (!Regex.IsMatch($"{item.Seq}", pattern)) throw new CustomerValidationException(nameof(ErrorCode.MES19429));
                    var ismianCount = saveDto.inteBusinessFieldList.Where(a => a.ISDefault).ToList().Count;
                    if (ismianCount > 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19424));
                    }
                    var patternFieldLabel = @"^[A-Z0-9_]+$";
                    if (!Regex.IsMatch($"{item.FieldLabel}", patternFieldLabel)) throw new CustomerValidationException(nameof(ErrorCode.MES19434));
                    // RuleFor(x => x.FieldLabel).Matches("^[A-Z0-9_]+$").WithErrorCode(nameof(ErrorCode.MES19434));

                    var isSeqCount = saveDto.inteBusinessFieldList.GroupBy(p => p.Seq)
                                        .Any(g => g.Count() > 1);
                    if (isSeqCount)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19430));
                    }
                    detailEntities.Add(new InteBusinessFieldListEntity()
                    {
                        BusinessFieldId = entity.Id,
                        Seq = item.Seq,
                        FieldLabel = item.FieldLabel,
                        FieldValue = item.FieldValue,
                        iSDefault = item.ISDefault,
                        Id = IdGenProvider.Instance.CreateId(),
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion

            using var ts = TransactionHelper.GetTransactionScope();
            await _inteBusinessFieldRepository.UpdateAsync(entity);
            await _inteBusinessFieldListRepository.DeleteAsync(entity.Id);
            if (detailEntities.Any())
            {
                await _inteBusinessFieldListRepository.InsertRangeAsync(detailEntities);
            }
            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {

            return await _inteBusinessFieldRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            // 校验数据 ：判断字段是否被分配
            var vehicleEntitys = await _inteBusinessFieldDistributeDetailsRepository.GetByBusinessFieldIdsAsync(new InteBusinessFieldDistributeDetailBusinessFieldIdIdsQuery { SiteId = _currentSite.SiteId ?? 0, BusinessFieldIds = ids });
            if (vehicleEntitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19433));
            }
            var rows = 0;
            using var ts = TransactionHelper.GetTransactionScope();
            rows += await _inteBusinessFieldListRepository.DeletesAsync(new DeleteCommand { Ids = ids });

            rows += await _inteBusinessFieldRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
            ts.Complete();
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaskInfoViewDto?> QueryByIdAsync(long id)
        {
            var inteBusinessFieldEntity = await _inteBusinessFieldRepository.GetByIdAsync(id);
            if (inteBusinessFieldEntity == null) return null;
            var maskInfoView = inteBusinessFieldEntity.ToModel<MaskInfoViewDto>();
            if (inteBusinessFieldEntity.MaskCodeId > 0)
            {
                var maskCodeEntity = await _procMaskCodeRepository.GetByIdAsync(inteBusinessFieldEntity.MaskCodeId.Value);
                maskInfoView.MaskCode = maskCodeEntity?.Code ?? "";
                maskInfoView.MaskName = maskCodeEntity?.Name ?? "";
            }
            return maskInfoView;
        }
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldListDto>> getBusinessFieldListByIdAsync(long id)
        {
            var inteBusinessFieldLists = new List<InteBusinessFieldListDto>();
            var inteBusinessFieldEntity = await _inteBusinessFieldListRepository.GetByIdAsync(id);
            //if (inteBusinessFieldEntity == null) return null;
            if (inteBusinessFieldEntity.Any())
            {
                inteBusinessFieldEntity= inteBusinessFieldEntity.OrderBy(x => x.Seq).ToList();
                foreach (var item in inteBusinessFieldEntity)
                {
                    var inteVehicleTypeVerifyDto = item.ToModel<InteBusinessFieldListDto>();

                    inteBusinessFieldLists.Add(inteVehicleTypeVerifyDto);
                }
            }
            return inteBusinessFieldLists;
        }
        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<MaskInfoViewDto>> GetPagedListAsync(InteBusinessFieldPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteBusinessFieldPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteBusinessFieldRepository.GetPagedListAsync(pagedQuery);

            //实体到DTO转换 装载数据
            List<MaskInfoViewDto> inteVehicleDtos = PrepareInteVehicleDtos(pagedInfo);
            return new PagedInfo<MaskInfoViewDto>(inteVehicleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<MaskInfoViewDto> PrepareInteVehicleDtos(PagedInfo<InteBusinessFieldView> pagedInfo)
        {
            var inteVehicleDtos = new List<MaskInfoViewDto>();
            foreach (var inteVehicleView in pagedInfo.Data)
            {
                var inteVehicleDto = inteVehicleView.ToModel<MaskInfoViewDto>();
                inteVehicleDtos.Add(inteVehicleDto);
            }

            return inteVehicleDtos;
        }
    }
}
