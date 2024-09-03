using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Core.NIO;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushCollection.View;
using Hymson.MES.Data.Repositories.NioPushCollection;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hymson.MES.Services.Services.NioPushCollection
{
    /// <summary>
    /// 服务（NIO推送参数） 
    /// </summary>
    public class NioPushCollectionService : INioPushCollectionService
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
        private readonly AbstractValidator<NioPushCollectionSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（NIO推送参数）
        /// </summary>
        private readonly INioPushCollectionRepository _nioPushCollectionRepository;

        /// <summary>
        /// 参数
        /// </summary>
        private readonly IProcProductParameterGroupRepository _procProductParameterGroupRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 
        /// </summary>
        public NioPushCollectionService(ICurrentUser currentUser, 
            ICurrentSite currentSite, 
            AbstractValidator<NioPushCollectionSaveDto> validationSaveRules, 
            INioPushCollectionRepository nioPushCollectionRepository,
            IProcProductParameterGroupRepository procProductParameterGroupRepository,
            INioPushRepository nioPushRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _nioPushCollectionRepository = nioPushCollectionRepository;
            _procProductParameterGroupRepository = procProductParameterGroupRepository;
            _nioPushRepository = nioPushRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(NioPushCollectionSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushCollectionEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _nioPushCollectionRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(NioPushCollectionSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            //await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var dbModel = await _nioPushRepository.GetByIdAsync(saveDto.NioPushId);
            if (dbModel.Status == Core.Enums.Plan.PushStatusEnum.Success)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17773));
            }

            Core.Enums.TrueOrFalseEnum isOk = Core.Enums.TrueOrFalseEnum.Yes;
            //判断参数是否合格
            if (saveDto.DecimalValue != null)
            {
                if(saveDto.UpperLimit != null && saveDto.LowerLimit != null)
                {
                    if (saveDto.DecimalValue > saveDto.UpperLimit || saveDto.DecimalValue < saveDto.LowerLimit)
                    {
                        isOk = Core.Enums.TrueOrFalseEnum.No;
                    }
                }
                else if(saveDto.UpperLimit != null)
                {
                    if (saveDto.DecimalValue > saveDto.UpperLimit)
                    {
                        isOk = Core.Enums.TrueOrFalseEnum.No;
                    }
                }
                else if(saveDto.LowerLimit != null)
                {
                    if (saveDto.DecimalValue < saveDto.LowerLimit)
                    {
                        isOk = Core.Enums.TrueOrFalseEnum.No;
                    }
                }
                //saveDto.DecimalValue = Math.Round((decimal)saveDto.DecimalValue, 4);
            }

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushCollectionEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.IsOk = isOk;

            long nioPushId = entity.NioPushId;

            using var trans = TransactionHelper.GetTransactionScope();

            int result = await _nioPushCollectionRepository.UpdateAsync(entity);
            //同步修改NIO_PUSH
            var pushItemList = await _nioPushCollectionRepository.GetByPushIdAsync(nioPushId);
            foreach(var item in pushItemList)
            {
                if(item.DecimalValue != null)
                {
                    item.DecimalValue = Math.Round((decimal)item.DecimalValue, 4);
                }
            }
            string tmpPushContext = JsonConvert.SerializeObject(pushItemList);
            List<NioCollectionDto> pushList = JsonConvert.DeserializeObject<List<NioCollectionDto>>(tmpPushContext);
            pushList.ForEach(m => m.UpdateTime = GetTimestamp(HymsonClock.Now()));
            NioCollectionSchDto nioSch = new NioCollectionSchDto() { List = pushList };

            NioPushEntity nioPushModel = await _nioPushRepository.GetByIdAsync(nioPushId);
            if(nioPushModel != null)
            {
                nioSch.SchemaCode = nioPushModel.SchemaCode;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string pushContext = JsonConvert.SerializeObject(nioSch, settings);

            NioPushEntity updateEntity = new NioPushEntity();
            updateEntity.Id = nioPushId;
            updateEntity.UpdatedBy = _currentUser.UserName;
            updateEntity.UpdatedOn = HymsonClock.Now();
            updateEntity.Content = pushContext;
            result += await _nioPushRepository.UpdateContentAsync(updateEntity);

            trans.Complete();

            return result;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private long GetTimestamp(DateTime date)
        {
            DateTime utcDateTime = ((DateTime)date).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = utcDateTime - epoch;
            return (long)timeSpan.TotalSeconds;

            //date = date.AddHours(8);
            //return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _nioPushCollectionRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _nioPushCollectionRepository.DeletesAsync(new DeleteCommand
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
        public async Task<NioPushCollectionDto?> QueryByIdAsync(long id) 
        {
           var nioPushCollectionEntity = await _nioPushCollectionRepository.GetByIdAsync(id);
           if (nioPushCollectionEntity == null) return null;
           
           return nioPushCollectionEntity.ToModel<NioPushCollectionDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushCollectionDto>> GetPagedListAsync(NioPushCollectionPagedQueryDto pagedQueryDto)
        {
            if(pagedQueryDto.CreatedOn == null)
            {
                string nowStr = HymsonClock.Now().ToString("yyyy-MM-dd 00:00:00");
                DateTime now = Convert.ToDateTime(nowStr);
                DateTime[] dateArr = new DateTime[] { now, now.AddDays(1) };
                pagedQueryDto.CreatedOn = dateArr;
            }
            var pagedQuery = pagedQueryDto.ToQuery<NioPushCollectionPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _nioPushCollectionRepository.GetPagedListAsync(pagedQuery);
            var dtos = await CheckParamStatusAsync(pagedInfo);

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());
            return new PagedInfo<NioPushCollectionDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 校验参数状态
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private async Task<IEnumerable<NioPushCollectionDto>?> CheckParamStatusAsync(PagedInfo<NioPushCollectionStatusView> pagedInfo)
        {
            //判断参数是否合格
            ProcedureParamQuery query = new ProcedureParamQuery();
            query.SiteId = _currentSite.SiteId ?? 0;
            var paramList = await _procProductParameterGroupRepository.GetParamListAsync(query);
            foreach (var item in pagedInfo.Data)
            {
                var curParam = paramList.Where(m => m.ParameterCode == item.VendorFieldCode).FirstOrDefault();
                if (curParam == null)
                {
                    continue;
                }
                item.LowerLimit = curParam.LowerLimit;
                item.UpperLimit = curParam.UpperLimit;
                item.CenterValue = curParam.CenterValue;
                item.ParameterName = curParam.ParameterName;
                item.ProcedureName = curParam.ProcedureName;
                ////不是数值则不校验
                //if (curParam.DataType != Core.Enums.DataTypeEnum.Numeric)
                //{
                //    continue;
                //}
                ////源数据没有数值不校验
                //if (item.DecimalValue == null)
                //{
                //    continue;
                //}
                //decimal curValue = (decimal)item.DecimalValue;
                ////上下限都存在
                //if (curParam.UpperLimit != null && curParam.LowerLimit != null)
                //{
                //    if (curValue > curParam.UpperLimit || curValue < curParam.LowerLimit)
                //    {
                //        item.IsOk = Core.Enums.TrueOrFalseEnum.No;
                //        continue;
                //    }
                //}
                ////中心值存在
                //if (curParam.CenterValue != null)
                //{
                //    if (curValue != curParam.CenterValue)
                //    {
                //        item.IsOk = Core.Enums.TrueOrFalseEnum.No;
                //        continue;
                //    }
                //}
                ////只存在上限值
                //if(curParam.UpperLimit != null)
                //{
                //    if (curValue > curParam.UpperLimit)
                //    {
                //        item.IsOk = Core.Enums.TrueOrFalseEnum.No;
                //        continue;
                //    }
                //}
                ////只存在下限值
                //if (curParam.LowerLimit != null)
                //{
                //    if (curValue < curParam.LowerLimit)
                //    {
                //        item.IsOk = Core.Enums.TrueOrFalseEnum.No;
                //        continue;
                //    }
                //}
                //item.IsOk = Core.Enums.TrueOrFalseEnum.Yes;
            }

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());

            return dtos;
        }

    }
}
