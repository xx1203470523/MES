using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.StatorBox
{
    /// <summary>
    /// 定子装箱数据源
    /// </summary>
    public class StatorBoxService : IStatorBoxService
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（定子装箱记录表）
        /// </summary>
        private readonly IManuStatorPackListRepository _manuStatorPackListRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatorBoxService(ISysConfigRepository sysConfigRepository,
            IManuStatorPackListRepository manuStatorPackListRepository)
        {
            _sysConfigRepository = sysConfigRepository;
            _manuStatorPackListRepository = manuStatorPackListRepository;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatorBoxDto>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param)
        {
            List<StatorBoxDto> resultList = new List<StatorBoxDto>();

            string boxCode = param.BarCodes.ElementAt(0).BarCode;
            //根据配置取数据
            SysConfigQuery configQuery = new SysConfigQuery();
            configQuery.Type = SysConfigEnum.NioBaseConfig;
            configQuery.Codes = new List<string>() { "NioStatorConfig" };
            var baseConfigList = await _sysConfigRepository.GetEntitiesAsync(configQuery);
            if (baseConfigList == null || !baseConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "NioStatorConfig");
            }
            string configValue = baseConfigList.ElementAt(0).Value;
            NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(configValue);

            //查询箱体码装了多少定子
            int boxNum = 0;
            var dbList = await _manuStatorPackListRepository.GetByBoxcodeAsync(boxCode);
            if(dbList != null && dbList.Count() > 0)
            {
                boxNum = dbList.Count();
            }

            StatorBoxDto model = new StatorBoxDto();
            model.ProductionDate = HymsonClock.Now().ToString("yyyyMMdd");
            model.MaterialUnit = "PCS";
            model.Num = boxNum;
            model.MaterialName = curConfig.VendorProductName;
            model.MaterialCode = curConfig.VendorProductCode;
            model.BoxCode = boxCode;

            resultList.Add(model);
            return resultList;
        }
    }
}
