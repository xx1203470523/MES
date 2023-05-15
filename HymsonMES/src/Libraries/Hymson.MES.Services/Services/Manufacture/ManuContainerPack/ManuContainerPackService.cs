/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 服务
    /// </summary>
    public class ManuContainerPackService : IManuContainerPackService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器装载表（物理删除） 仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<ManuContainerPackCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerPackModifyDto> _validationModifyRules;
        private readonly IManuContainerPackRecordService _manuContainerPackRecordService;
        /// <summary>
        /// 接口（操作面板按钮）
        /// </summary>
        private readonly IManuFacePlateButtonService _manuFacePlateButtonService;

        public ManuContainerPackService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuContainerPackRepository manuContainerPackRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuContainerPackRecordService manuContainerPackRecordService,
            AbstractValidator<ManuContainerPackCreateDto> validationCreateRules,
            AbstractValidator<ManuContainerPackModifyDto> validationModifyRules,
            IProcMaterialRepository procMaterialRepository,
            IManuFacePlateButtonService manuFacePlateButtonService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procMaterialRepository = procMaterialRepository;
            _manuFacePlateButtonService = manuFacePlateButtonService;
            _manuContainerPackRecordService = manuContainerPackRecordService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuContainerPackCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuContainerPackAsync(ManuContainerPackCreateDto manuContainerPackCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuContainerPackCreateDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackCreateDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.Id = IdGenProvider.Instance.CreateId();
            manuContainerPackEntity.CreatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.CreatedOn = HymsonClock.Now();
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();
            manuContainerPackEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuContainerPackRepository.InsertAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerPackAsync(long id)
        {
            await _manuContainerPackRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuContainerPackAsync(long[] ids)
        {
            return await _manuContainerPackRepository.DeleteTrueAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        public async Task DeleteAllByContainerBarCodeIdAsync(long containerBarCodeId)
        {
            //生成删除记录
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(containerBarCodeId, _currentSite.SiteId ?? 0).ContinueWith(async t =>
                {
                    var packs = t.Result.Select(m =>
                    {
                        return new ManuContainerPackRecordCreateDto()
                        {
                            ResourceId = m.ResourceId,
                            ProcedureId = m.ProcedureId,
                            ContainerBarCodeId = m.ContainerBarCodeId,
                            LadeBarCode = m.LadeBarCode,
                            OperateType = (int)ManuContainerBarcodeOperateTypeEnum.Unload
                        };
                    });
                    await _manuContainerPackRecordService.CreateManuContainerPackRecordsAsync(packs.ToList());

                });
                await _manuContainerPackRepository.DeleteAllAsync(containerBarCodeId);
                ts.Complete();
            }
            //物理删除


        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackDto>> GetPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto)
        {
            var manuContainerPackPagedQuery = manuContainerPackPagedQueryDto.ToQuery<ManuContainerPackPagedQuery>();
            manuContainerPackPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuContainerPackRepository.GetPagedInfoAsync(manuContainerPackPagedQuery);


            //实体到DTO转换 装载数据
            List<ManuContainerPackDto> manuContainerPackDtos = await PrepareManuContainerPackDtos(pagedInfo);
            return new PagedInfo<ManuContainerPackDto>(manuContainerPackDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private async Task<List<ManuContainerPackDto>> PrepareManuContainerPackDtos(PagedInfo<ManuContainerPackView> pagedInfo)
        {
            var manuContainerPackDtos = new List<ManuContainerPackDto>();
            //工单信息
            IEnumerable<PlanWorkOrderEntity> planWorkOrderList = new List<PlanWorkOrderEntity>();
            //批量查询物料信息
            IEnumerable<ProcMaterialEntity> procMaterialList = new List<ProcMaterialEntity>();
            if (pagedInfo.Data.Any())
            {
                var productIds = pagedInfo.Data.Select(c => c.ProductId).ToArray();
                if (productIds.Any())
                {
                    procMaterialList = await _procMaterialRepository.GetByIdsAsync(productIds);
                }

                //批量查询工单信息
                var workOrderIds = pagedInfo.Data.Select(c => c.WorkOrderId).ToArray();
                if (workOrderIds.Any())
                {
                    planWorkOrderList = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
                }
            }

            //转换Dto
            foreach (var manuContainerPackView in pagedInfo.Data)
            {
                var manuContainerPackDto = manuContainerPackView.ToModel<ManuContainerPackDto>();
                //转换物料编码
                var procMater = procMaterialList.Where(c => c.Id == manuContainerPackView.ProductId)?.FirstOrDefault();
                if (procMater != null)
                {
                    manuContainerPackDto.MaterialCode = procMater.MaterialCode;
                }
                //转换工单编码
                var planWorkOrder = planWorkOrderList.Where(c => c.Id == manuContainerPackView.WorkOrderId)?.FirstOrDefault();
                if (planWorkOrder != null)
                {
                    manuContainerPackDto.WorkOrderCode = planWorkOrder.OrderCode;
                }

                manuContainerPackDtos.Add(manuContainerPackDto);
            }

            return manuContainerPackDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerPackAsync(ManuContainerPackModifyDto manuContainerPackModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerPackModifyDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackModifyDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerPackRepository.UpdateAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerPackDto> QueryManuContainerPackByIdAsync(long id)
        {
            var manuContainerPackEntity = await _manuContainerPackRepository.GetByIdAsync(id);
            if (manuContainerPackEntity != null)
            {
                return manuContainerPackEntity.ToModel<ManuContainerPackDto>();
            }
            return null;
        }


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateContainerPackExJobDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<Dictionary<string, JobResponseDto>> ExecuteJobAsync(ManuFacePlateContainerPackExJobDto manuFacePlateContainerPackExJobDto)
        {
            #region  验证数据
            if (string.IsNullOrWhiteSpace(manuFacePlateContainerPackExJobDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16708));
            }
            #endregion

            #region 调用作业
            manuFacePlateContainerPackExJobDto.SFC = manuFacePlateContainerPackExJobDto.SFC.Trim();
            var jobDto = new ButtonRequestDto
            {
                FacePlateId = manuFacePlateContainerPackExJobDto.FacePlateId,
                FacePlateButtonId = manuFacePlateContainerPackExJobDto.FacePlateButtonId,
                Param = new Dictionary<string, string>()
            };
            jobDto.Param?.Add("SFC", manuFacePlateContainerPackExJobDto.SFC);
            jobDto.Param?.Add("ProcedureId", $"{manuFacePlateContainerPackExJobDto.ProcedureId}");
            jobDto.Param?.Add("ResourceId", $"{manuFacePlateContainerPackExJobDto.ResourceId}");
            jobDto.Param?.Add("ContainerId", $"{manuFacePlateContainerPackExJobDto.ContainerId}");

            // 调用作业
            var resJob = await _manuFacePlateButtonService.ClickAsync(jobDto);
            if (resJob == null || resJob.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES16709));
            return resJob;
            #endregion
        }
    }
}
