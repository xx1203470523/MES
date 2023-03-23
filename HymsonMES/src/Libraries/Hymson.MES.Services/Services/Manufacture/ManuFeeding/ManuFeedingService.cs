using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Sequences;
using Hymson.Utils;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Hymson.MES.Services.Services.Manufacture.ManuFeeding
{
    /// <summary>
    /// 服务（容器维护）
    /// </summary>
    public class ManuFeedingService : IManuFeedingService
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
        /// 容器维护 仓储
        /// </summary>
        //private readonly IInteContainerRepository _inteContainerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="inteContainerRepository"></param>
        public ManuFeedingService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            //_inteContainerRepository = inteContainerRepository;
        }


        /// <summary>
        /// 查询类型（物料加载）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<FeedingSourceEnum> GetFeedingSourceAsync(string code)
        {
            return await Task.FromResult(FeedingSourceEnum.Resource);
        }

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingListeAsync(ManuFeedingMaterialQueryDto queryDto)
        {
            List<ManuFeedingMaterialDto> list = new();

            // TODO
            for (int i = 1; i < 4; i++)
            {
                var item = new ManuFeedingMaterialDto
                {
                    MaterialId = i,
                    MaterialCode = $"MaterialCode-{i}",
                    MaterialName = $"MaterialName-{i}",
                    Version = $"v-{i}",
                    Children = new List<ManuFeedingMaterialItemDto>()
                };

                for (int j = 1; j < 4; j++)
                {
                    item.Children.Add(new ManuFeedingMaterialItemDto
                    {
                        MaterialId = i,
                        BarCode = $"BarCode-{j}",
                        InitQty = new Random().Next(50, 100),
                        Qty = new Random().Next(0, 20),
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }

                list.Add(item);
            }

            await Task.CompletedTask;
            return list;
        }

        /*
        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteContainerSaveDto createDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<InteContainerEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存实体
            return await _inteContainerRepository.InsertAsync(entity); ;
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteContainerSaveDto modifyDto)
        {
            var entity = modifyDto.ToEntity<InteContainerEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 更新实体
            return await _inteContainerRepository.UpdateAsync(entity);
        }
        */

    }
}
