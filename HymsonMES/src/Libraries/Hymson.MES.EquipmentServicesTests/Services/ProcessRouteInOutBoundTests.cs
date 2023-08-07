using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServicesTests.Dtos;
using Hymson.MES.EquipmentServicesTests.Extensions;
using Hymson.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServicesTests.Services
{
    [TestClass()]
    public class ProcessRouteInOutBoundTests : BaseTest
    {
        private readonly IInBoundService _inBoundService;
        private readonly IOutBoundService _outBoundService;
        private readonly IEquEquipmentRepository _equEquipmentRepository;//设备
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;//工艺路线
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;//工艺路线明细
        private readonly IProcProcedureRepository _procProcedureRepository;//工序
        private readonly IProcResourceTypeRepository _procResourceTypeRepository;//资源类型
        private readonly IProcResourceRepository _procResourceRepository;//资源
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;//资源对应设备绑定
        public ProcessRouteInOutBoundTests()
        {
            _inBoundService = ServiceProvider.GetRequiredService<IInBoundService>();
            _outBoundService = ServiceProvider.GetRequiredService<IOutBoundService>();
            _equEquipmentRepository = ServiceProvider.GetRequiredService<IEquEquipmentRepository>();
            _procProcessRouteRepository = ServiceProvider.GetRequiredService<IProcProcessRouteRepository>();
            _procProcessRouteDetailNodeRepository = ServiceProvider.GetRequiredService<IProcProcessRouteDetailNodeRepository>();
            _procProcedureRepository = ServiceProvider.GetRequiredService<IProcProcedureRepository>();
            _procResourceTypeRepository = ServiceProvider.GetRequiredService<IProcResourceTypeRepository>();
            _procResourceRepository = ServiceProvider.GetRequiredService<IProcResourceRepository>();
            _procResourceEquipmentBindRepository = ServiceProvider.GetRequiredService<IProcResourceEquipmentBindRepository>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //设置测试Site信息
            Dictionary<string, object> siteInfo = new()
            {
                { "SiteId", 123456 },
                {"SiteName","单元测试站点"}
            };
            CurrentEquipmentInfo.AddUpdate(siteInfo);
        }

        /// <summary>
        /// 获取工艺路线对应工序，设备，资源相关信息
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ProcessRouteInOutBoundDto>> GetTestProcessInfoAsnync(string processRouteCode)
        {
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var procProcessRouteEntities = await _procProcessRouteRepository.GetProcProcessRouteEntitiesAsync(new ProcProcessRouteQuery
            {
                Code = processRouteCode,
                SiteId = siteId,
            });
            if (!procProcessRouteEntities.Any()) throw new Exception("工艺路线不存在");
            var procProcessRouteEntity = procProcessRouteEntities.First();
            //获取工艺路线明细
            var procProcessRouteDetailNodeEntities = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(procProcessRouteEntity.Id);
            if (!procProcessRouteDetailNodeEntities.Any()) throw new Exception("未获取到工艺路线节点信息");
            //过滤结束标识工艺路线并排序
            var procProcessRouteDetailNodes = procProcessRouteDetailNodeEntities.Where(it => it.ProcedureId != ProcessRoute.LastProcedureId).OrderBy(x => x.ManualSortNumber).Distinct();
            var procedureIds = procProcessRouteDetailNodes.Select(c => c.ProcedureId).ToArray();
            //工艺路线的所有工序信息
            var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);
            if (!procProcedureEntities.Any()) throw new Exception("工艺路线不包含任何工序");
            //查询工序对应资源类型
            var resourceTypeIds = procProcedureEntities.Select(c => c.ResourceTypeId ?? 0).ToArray();
            var procResourceTypeEntities = await _procResourceTypeRepository.GetByIdsAsync(resourceTypeIds);
            //根据资源类型查询所有关联资源
            var procResourceEntities = await _procResourceRepository.GetByResTypeIdsAsync(new ProcResourceQuery { IdsArr = resourceTypeIds, SiteId = siteId });
            if (!procResourceEntities.Any()) throw new Exception("根据资源类型未找到任何资源信息");
            //根据资源Id查询绑定设备信息
            var resourceIds = procResourceEntities.Select(c => c.Id).ToArray();
            var procResourceEquipmentBinds = await _procResourceEquipmentBindRepository.GetByResourceIdsAsync(resourceIds);
            if (!procResourceEquipmentBinds.Any()) throw new Exception("根据资源信息未找到任何绑定设备信息");
            //查询所有设备信息
            var equipmentIds = procResourceEquipmentBinds.Select(c => c.EquipmentId).ToArray();
            var equEquipmentEntities = await _equEquipmentRepository.GetByIdsAsync(equipmentIds);
            //组装测试设备数据
            List<ProcessRouteInOutBoundDto> routeInOutBoundDtos = new List<ProcessRouteInOutBoundDto>();
            foreach (var processRoute in procProcessRouteDetailNodes)
            {
                //当前工序
                var procProcedure = procProcedureEntities.Where(c => c.Id == processRoute.ProcedureId).First();
                //当前资源类型
                var procResourceType = procResourceTypeEntities.Where(c => c.Id == procProcedure.ResourceTypeId).First();
                //当前资源
                var procResource = procResourceEntities.Where(c => c.ResTypeId == procResourceType.Id).First();
                //当前资源绑定信息
                var equipmentBindEntities = procResourceEquipmentBinds.Where(c => c.ResourceId == procResource.Id);
                if (!equipmentBindEntities.Any())
                {
                    throw new Exception($"当前资源{procResource?.Id}没有绑定任何设备");
                }
                //如果有主设置就取主设备否则取第一个
                ProcResourceEquipmentBindEntity equipmentBindEntity = (equipmentBindEntities.Where(c => c.IsMain).FirstOrDefault()
                    ?? equipmentBindEntities.First()) ?? throw new Exception("未找到关联设备信息");
                //当前模拟设备信息
                var equEquipment = equEquipmentEntities.Where(c => c.Id == equipmentBindEntity.EquipmentId).First();
                //返回当前需要模拟的所有设备信息
                routeInOutBoundDtos.Add(new ProcessRouteInOutBoundDto
                {
                    SiteId = siteId,
                    ProcedureId = procProcedure.Id,
                    Id = equEquipment.Id,
                    Code = equEquipment.EquipmentCode,
                    FactoryId = equEquipment.WorkCenterFactoryId,
                    Name = equEquipment.EquipmentName,
                    ResourceCode = procResource.ResCode,
                    Sort = processRoute.ManualSortNumber
                });
            }
            return routeInOutBoundDtos;
        }

        /// <summary>
        /// 获取Ng候选列表
        /// </summary>
        /// <returns></returns>
        private static List<OutBoundNG> GetOutBoundNGs()
        {
            return new List<OutBoundNG> {
                new OutBoundNG {NGCode="C2108"},//负极穿透熔深超下限
                new OutBoundNG {NGCode="C2107"},//负极穿透熔深超上限
                new OutBoundNG {NGCode="C2104"},//二维码内容错误
                new OutBoundNG {NGCode="C2103"},//二维码位置不良
                new OutBoundNG {NGCode="C2102"},//二维码尺寸不良
                new OutBoundNG {NGCode="C2101"},//二维码尺寸不良
            };
        }

        /// <summary>
        /// 获取参数候选列表
        /// 对于出站参数，编码不存在会新增
        /// </summary>
        /// <returns></returns>
        private static List<OutBoundParam> GetOutBoundParameters()
        {
            List<OutBoundParam> outParams = new();
            Random random = new Random();
            for (int i = 1; i <= 50; i++)
            {
                outParams.Add(new OutBoundParam
                {
                    ParamCode = "单元测试出站参数" + i,
                    ParamValue = random.Next(1, 100).ToString()
                });
            }
            return outParams;
        }

        /// <summary>
        /// 按工艺路线进出站测试简单的进出站直至工艺路线结束（用于造测试数据）
        /// 工序必须维护对应资源类型
        /// 资源必须维护对应资源类型
        /// 资源对应设备如果多个会取主设备模拟，否则取第一个设备模拟
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ProcessRouteInOutBoundTestAsnync()
        {
            string processRouteCode = "QAPR001";//执行工艺路线
            string prefix = "AAA230807" + DateTime.Now.ToString("HHmm");//生成条码前缀
            int sfcCount = 30;//需要进出站多少条码数量
            int sfcSuffixLength = 3;//条码追加后缀0长度
            //或许工艺路线设备工序等信息
            var processRouteInOutBounds = await GetTestProcessInfoAsnync(processRouteCode);
            processRouteInOutBounds = processRouteInOutBounds.OrderBy(x => x.Sort).ToList();
            //生成模拟条码
            ProcessRouteSfcDto[] routeSfcDtos = new ProcessRouteSfcDto[sfcCount];
            for (int i = 1; i <= sfcCount; i++)
            {
                Random random = new Random();
                var sfcSuffix = i.ToString().PadLeft(sfcSuffixLength, '0');
                var sfc = prefix + sfcSuffix;
                //随机Ng数量
                var ngCount = random.Next(0, 6);
                var ngList = GetOutBoundNGs().GetListRandomElements(ngCount);
                //随机取工序为NG发生工序
                var ngProcessRoute = processRouteInOutBounds.ToList().GetListRandomElements(1).First();
                //出站参数
                var parameterCount = random.Next(0, 50);
                var outParameters = GetOutBoundParameters().GetListRandomElements(parameterCount);
                routeSfcDtos[i - 1] = new ProcessRouteSfcDto
                {
                    IsNg = false,
                    OutBoundParam = outParameters,
                    NgList = ngList,
                    NgProcProcedure = i % 3 == 0 ? ngProcessRoute.ProcedureId : null,//偶数条码NG设定
                    SFC = sfc
                };
            }
            //按工艺路线做简单循环进出站
            foreach (var item in processRouteInOutBounds)
            {
                var resourceCode = item.ResourceCode;
                //获取需要进站的数据
                var routeSfcs = routeSfcDtos.Where(c => c.IsNg != true);
                var sfcs = routeSfcs.Select(c => c.SFC).ToArray();
                if (!sfcs.Any()) { break; }
                //设置当前模拟设备名称
                SetEquInfoAsync(new EquipmentInfoDto { Id = item.Id, FactoryId = item.FactoryId, Code = item.Code, Name = item.Name });
                //进站
                await _inBoundService.InBoundMoreAsync(new InBoundMoreDto
                {
                    LocalTime = HymsonClock.Now().AddMilliseconds(-1),
                    ResourceCode = resourceCode,
                    SFCs = sfcs
                });
                //出站
                List<OutBoundSFCDto> sfcList = new();
                foreach (var sfcItem in routeSfcs)
                {
                    var passed = true;
                    //当前条码当前工序是否需要Ng
                    if (sfcItem.NgProcProcedure.HasValue && item.ProcedureId == sfcItem.NgProcProcedure)
                    {
                        passed = false;
                        sfcItem.IsNg = true;//标记为已经NG出站
                    }
                    sfcList.Add(new OutBoundSFCDto
                    {
                        SFC = sfcItem.SFC,
                        Passed = passed ? 1 : 0,
                        NG = !passed ? sfcItem.NgList?.ToArray() : null,
                        ParamList = sfcItem.OutBoundParam?.ToArray()
                    });
                }
                await _outBoundService.OutBoundMoreAsync(new OutBoundMoreDto
                {
                    ResourceCode = resourceCode,
                    LocalTime = HymsonClock.Now(),
                    SFCs = sfcList.ToArray()
                });
            }
            Assert.IsTrue(true);
        }

    }
}
