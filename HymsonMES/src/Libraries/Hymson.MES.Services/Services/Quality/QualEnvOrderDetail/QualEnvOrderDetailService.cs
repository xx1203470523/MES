/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using AutoMapper.Configuration.Annotations;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.QualEnvParameterGroupDetailSnapshoot;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.QualEnvOrderDetail;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;
using System.Transactions;

namespace Hymson.MES.Services.Services.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细 服务
    /// </summary>
    public class QualEnvOrderDetailService : IQualEnvOrderDetailService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 环境检验单检验明细 仓储
        /// </summary>
        private readonly IQualEnvOrderDetailRepository _qualEnvOrderDetailRepository;
        private readonly AbstractValidator<QualEnvOrderDetailCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualEnvOrderDetailModifyDto> _validationModifyRules;
        private readonly AbstractValidator<List<QualEnvOrderDetailModifyDto>> _validationModifysRules;

        public QualEnvOrderDetailService(ICurrentUser currentUser, ICurrentSite currentSite, IQualEnvOrderDetailRepository qualEnvOrderDetailRepository, AbstractValidator<QualEnvOrderDetailCreateDto> validationCreateRules, AbstractValidator<QualEnvOrderDetailModifyDto> validationModifyRules, AbstractValidator<List<QualEnvOrderDetailModifyDto>> validationModifysRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualEnvOrderDetailRepository = qualEnvOrderDetailRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationModifysRules = validationModifysRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="qualEnvOrderDetailCreateDto"></param>
        /// <returns></returns>
        public async Task CreateQualEnvOrderDetailAsync(QualEnvOrderDetailCreateDto qualEnvOrderDetailCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(qualEnvOrderDetailCreateDto);

            //DTO转换实体
            var qualEnvOrderDetailEntity = qualEnvOrderDetailCreateDto.ToEntity<QualEnvOrderDetailEntity>();
            qualEnvOrderDetailEntity.Id = IdGenProvider.Instance.CreateId();
            qualEnvOrderDetailEntity.CreatedBy = _currentUser.UserName;
            qualEnvOrderDetailEntity.UpdatedBy = _currentUser.UserName;
            qualEnvOrderDetailEntity.CreatedOn = HymsonClock.Now();
            qualEnvOrderDetailEntity.UpdatedOn = HymsonClock.Now();
            qualEnvOrderDetailEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _qualEnvOrderDetailRepository.InsertAsync(qualEnvOrderDetailEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQualEnvOrderDetailAsync(long id)
        {
            await _qualEnvOrderDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualEnvOrderDetailAsync(long[] ids)
        {
            return await _qualEnvOrderDetailRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="qualEnvOrderDetailPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvOrderDetailDto>> GetPagedListAsync(QualEnvOrderDetailPagedQueryDto qualEnvOrderDetailPagedQueryDto)
        {
            var qualEnvOrderDetailPagedQuery = qualEnvOrderDetailPagedQueryDto.ToQuery<QualEnvOrderDetailPagedQuery>();
            var pagedInfo = await _qualEnvOrderDetailRepository.GetPagedInfoAsync(qualEnvOrderDetailPagedQuery);


            //实体到DTO转换 装载数据
            List<QualEnvOrderDetailDto> qualEnvOrderDetailDtos = PrepareQualEnvOrderDetailDtos(pagedInfo);
            return new PagedInfo<QualEnvOrderDetailDto>(qualEnvOrderDetailDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualEnvOrderDetailDto> PrepareQualEnvOrderDetailDtos(PagedInfo<QualEnvOrderDetailEntity> pagedInfo)
        {
            var qualEnvOrderDetailDtos = new List<QualEnvOrderDetailDto>();
            foreach (var qualEnvOrderDetailEntity in pagedInfo.Data)
            {
                var qualEnvOrderDetailDto = qualEnvOrderDetailEntity.ToModel<QualEnvOrderDetailDto>();
                qualEnvOrderDetailDtos.Add(qualEnvOrderDetailDto);
            }

            return qualEnvOrderDetailDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualEnvOrderDetailDto"></param>
        /// <returns></returns>
        public async Task ModifyQualEnvOrderDetailAsync(QualEnvOrderDetailModifyDto qualEnvOrderDetailModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(qualEnvOrderDetailModifyDto);

            //DTO转换实体
            var qualEnvOrderDetailEntity = qualEnvOrderDetailModifyDto.ToEntity<QualEnvOrderDetailEntity>();
            qualEnvOrderDetailEntity.UpdatedBy = _currentUser.UserName;
            qualEnvOrderDetailEntity.UpdatedOn = HymsonClock.Now();

            await _qualEnvOrderDetailRepository.UpdateAsync(qualEnvOrderDetailEntity);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param> 
        /// <returns></returns>
        public async Task<int> ModifyQualEnvOrderDetailsAsync(List<QualEnvOrderDetailModifyDto> qualEnvOrderDetailEntitys)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证参数
            await _validationModifysRules.ValidateAndThrowAsync(qualEnvOrderDetailEntitys);

            var datas = new List<QualEnvOrderDetailEntity>();
            foreach (var item in qualEnvOrderDetailEntitys)
            {
                var data = new QualEnvOrderDetailEntity()
                {
                    Id = item.Id,
                    RealTime = HymsonClock.Now().ToString($"yyyy-MM-dd {item.RealTime}:ss").ParseToDateTime(),
                    InspectionValue = item.InspectionValue,
                    IsQualified = item.IsQualified,
                    Remark = item.Remark,
                };

                data.UpdatedBy = _currentUser.UserName;
                data.UpdatedOn = HymsonClock.Now();
                datas.Add(data);
            }

            return await _qualEnvOrderDetailRepository.UpdatesExecAsync(datas);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualEnvOrderDetailDto> QueryQualEnvOrderDetailByIdAsync(long id)
        {
            var qualEnvOrderDetailEntity = await _qualEnvOrderDetailRepository.GetByIdAsync(id);
            if (qualEnvOrderDetailEntity != null)
            {
                return qualEnvOrderDetailEntity.ToModel<QualEnvOrderDetailDto>();
            }
            return null;
        }


        #region 


        /// <summary>
        /// 根据检验单ID获取数据
        /// </summary>
        /// <param name="envOrderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderDetailExtendDto>> GetQualEnvOrderDetailByEnvOrderIdAsync(long envOrderId)
        {
            var qualEnvOrderDetails = await _qualEnvOrderDetailRepository.GetByEnvOrderIdAsync(envOrderId);
            if (qualEnvOrderDetails == null || !qualEnvOrderDetails.Any())
            {
                return null;
            }
            //获取快照
            var groupDetailSnapshootIds = qualEnvOrderDetails.Select(x => x.GroupDetailSnapshootId).ToArray();
            var GroupDetailSnapshoots = await _qualEnvOrderDetailRepository.GetGroupDetailSnapshootByIdsAsync(groupDetailSnapshootIds);
            var qualEnvOrderDetailDtos = new List<QualEnvOrderDetailExtendDto>();
            foreach (var item in qualEnvOrderDetails)
            {
                QualEnvOrderDetailExtendDto qualEnvOrderDetailDto;
                try
                {
                    qualEnvOrderDetailDto = item.ToModel<QualEnvOrderDetailExtendDto>();

                }
                catch (Exception ex)
                {

                    throw;
                }
                var groupDetailSnapshoot = GroupDetailSnapshoots.Where(x => x.Id == item.GroupDetailSnapshootId).FirstOrDefault();
                if (groupDetailSnapshoot == null)
                {
                    continue;
                }
                qualEnvOrderDetailDto.ParameterCode = groupDetailSnapshoot.ParameterCode;
                qualEnvOrderDetailDto.ParameterName = groupDetailSnapshoot.ParameterName;
                qualEnvOrderDetailDto.ParameterUnit = groupDetailSnapshoot.ParameterUnit;
                qualEnvOrderDetailDto.ParameterDataType = groupDetailSnapshoot.ParameterDataType;
                qualEnvOrderDetailDto.UpperLimit = groupDetailSnapshoot.UpperLimit;
                qualEnvOrderDetailDto.ReferenceValue = groupDetailSnapshoot.ReferenceValue;
                qualEnvOrderDetailDto.LowerLimit = groupDetailSnapshoot.LowerLimit;
                qualEnvOrderDetailDto.StartTime = item.StartTime.ToString("HH:mm");
                qualEnvOrderDetailDto.EndTime = item.StartTime.ToString("HH:mm");
                qualEnvOrderDetailDto.RealTime = item.RealTime == null ? HymsonClock.Now().ToString("HH:mm") : item.StartTime.ToString("HH:mm");
                qualEnvOrderDetailDtos.Add(qualEnvOrderDetailDto);
            }

            return qualEnvOrderDetailDtos;
        }




        #endregion
    }
}
