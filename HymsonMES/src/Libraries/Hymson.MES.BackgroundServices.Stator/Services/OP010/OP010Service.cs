﻿using Hymson.MES.Core.Domain.Process;
using Hymson.MES.CoreServices.Extension;
using Hymson.Snowflake;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP010Service : IOP010Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP010Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP010> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IBaseService _baseService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="baseService"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="procParameterRepository"></param>
        public OP010Service(ILogger<OP010Service> logger,
            IOPRepository<OP010> opRepository,
            IBaseService baseService,
            IWaterMarkService waterMarkService,
            IProcParameterRepository procParameterRepository)
        {
            _logger = logger;
            _opRepository = opRepository;
            _baseService = baseService;
            _waterMarkService = waterMarkService;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP010).Name}";
            var buzKey = $"{StatorConst.BUZ_KEY_PREFIX}-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var entities = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (entities == null || !entities.Any())
            {
                _logger.LogDebug($"【{producreCode}】没有要拉取的数据！");
                return 0;
            }

            // 先定位条码位置
            var barCodes = entities.Select(s => s.wire1_barcode);

            // 获取转换数据（基础数据）
            var summaryBo = await _baseService.ConvertDataAsync(entities, barCodes);

            // 读取标准参数
            var parameterEntities = await _baseService.GetParameterEntitiesWithInitAsync(_parameterCodes, summaryBo.StatorBo);

            // 填充参数
            foreach (var step in summaryBo.ManuSfcStepEntities)
            {
                // 当前记录
                var entity = entities.FirstOrDefault(f => $"{f.index}" == step.Remark);
                if (entity == null) continue;

                // 遍历参数
                foreach (var param in parameterEntities)
                {
                    // 指定对象获取值
                    var paramValue = entity.GetType().GetProperty(param.ParameterCode)?.GetValue(entity);

                    summaryBo.ManuProductParameterEntities.Add(new Core.Domain.Parameter.ManuProductParameterEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = step.ProcedureId ?? 0,
                        SfcstepId = step.Id,
                        SFC = step.SFC,

                        ParameterId = param.Id,
                        ParameterValue = $"{paramValue}",
                        ParameterGroupId = entity.Result == "OK" ? 1 : 0,
                        CollectionTime = entity.RDate ?? step.CreatedOn,

                        SiteId = step.SiteId,
                        CreatedBy = step.CreatedBy,
                        CreatedOn = step.CreatedOn,
                        UpdatedBy = step.UpdatedBy,
                        UpdatedOn = step.UpdatedOn
                    });
                }

            }

            // 保存数据
            return await _baseService.SaveBaseDataWithCommitAsync(buzKey, entities.Max(m => m.index), summaryBo);
        }

    }

    /// <summary>
    /// 服务
    /// </summary>
    public partial class OP010Service
    {
        /// <summary>
        /// 参数编码集合
        /// </summary>
        private static readonly List<string> _parameterCodes = new()
        {
            "FormingLHZDistance",
            "FormingRHZDistance",
            "FormingUpperLHXDistance",
            "FormingUpperRHXDistance",
            "FormingLowerLHXDistance",
            "FormingLowerRHXDistance",
            "FormingLHZSpeed",
            "FormingRHZSpeed",
            "FormingUpperLHXSpeed",
            "FormingUpperRHXSpeed",
            "FormingLowerLHXSpeed",
            "FormingLowerRHXSpeed"
        };
    }
}
