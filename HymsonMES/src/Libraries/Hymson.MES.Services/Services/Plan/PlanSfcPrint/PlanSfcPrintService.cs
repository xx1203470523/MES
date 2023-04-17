using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 服务
    /// </summary>
    public class PlanSfcPrintService : IPlanSfcPrintService
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
        private readonly AbstractValidator<PlanSfcPrintCreateDto> _validationCreateRules;

        /// <summary>
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="manuSfcRepository"></param>
        public PlanSfcPrintService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanSfcPrintCreateDto> validationCreateRules,
            IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _manuSfcRepository = manuSfcRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(PlanSfcPrintCreateDto createDto)
        {
            #region 验证与数据组装
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);

            //验证条码与工单




            //生成条码




            //条码打印




            #endregion

            #region 入库
            #endregion
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var sfcEntities = await _manuSfcRepository.GetByIdsAsync(idsArr);
            if (sfcEntities.Any(it => it.IsUsed > YesOrNoEnum.Yes) == true) throw new BusinessException(nameof(ErrorCode.MES16116));

            return await _manuSfcRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcPassDownPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => new PlanSfcPrintDto
            {
                Id = s.Id,
                SFC = s.SFC,
                IsUsed = s.IsUsed,
                UpdatedOn = s.UpdatedOn,
                OrderCode = s.OrderCode,
                MaterialCode = s.MaterialCode,
                MaterialName = s.MaterialName
            });
            return new PagedInfo<PlanSfcPrintDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


    }
}
