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
        /// 设置当前测试设备信息
        /// </summary>
        /// <param name="routeInOutBoundDto"></param>
        public static void SetEquInfoAsync(ProcessRouteInOutBoundDto routeInOutBoundDto)
        {
            //所以必须先设置站点Id
            var siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            Dictionary<string, object> equDic = new()
            {
                { "Id", routeInOutBoundDto.Id },
                { "FactoryId", routeInOutBoundDto.FactoryId },
                { "Code", routeInOutBoundDto.Code },
                { "Name", routeInOutBoundDto.Name }
            };
            CurrentEquipmentInfo.AddUpdate(equDic);
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
                var procProcedure = procProcedureEntities.Where(c => c.Id == processRoute.ProcedureId).FirstOrDefault();
                //当前资源类型
                var procResourceType = procResourceTypeEntities.Where(c => c.Id == procProcedure.ResourceTypeId).FirstOrDefault();
                //当前资源
                var procResource = procResourceEntities.Where(c => c.ResTypeId == procResourceType.Id).FirstOrDefault();
                //当前资源绑定信息
                var equipmentBindEntities = procResourceEquipmentBinds.Where(c => c.ResourceId == procResource.Id);
                if (!equipmentBindEntities.Any())
                {
                    throw new Exception($"当前资源{procResource?.Id}没有绑定任何设备");
                }
                //如果有主设置就取主设备否则取第一个
                ProcResourceEquipmentBindEntity? equipmentBindEntity = (equipmentBindEntities.Where(c => c.IsMain).FirstOrDefault()
                    ?? equipmentBindEntities.FirstOrDefault()) ?? throw new Exception("未找到关联设备信息");
                //当前模拟设备信息
                var equEquipment = equEquipmentEntities.Where(c => c.Id == equipmentBindEntity.EquipmentId).FirstOrDefault();
                //返回当前需要模拟的所有设备信息
                routeInOutBoundDtos.Add(new ProcessRouteInOutBoundDto
                {
                    SiteId = siteId,
                    Code = equEquipment.EquipmentCode,
                    Id = equEquipment.Id,
                    FactoryId = equEquipment.WorkCenterFactoryId,
                    Name = equEquipment.EquipmentName,
                    ResourceCode = procResource.ResCode,
                    Sort = processRoute.ManualSortNumber
                });
            }
            return routeInOutBoundDtos;
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
            string prefix = "AAA230804" + DateTime.Now.ToString("HHmm");//生成条码前缀
            int sfcCount = 1;//需要进出站多少条码数量
            int sfcSuffixLength = 3;//条码追加后缀0长度
            string[] sfcs = new string[sfcCount];
            for (int i = 1; i <= sfcCount; i++)
            {
                var sfcSuffix = i.ToString().PadLeft(sfcSuffixLength, '0');
                var sfc = prefix + sfcSuffix;
                sfcs[i - 1] = sfc;
            }
            var processRouteInOutBounds = await GetTestProcessInfoAsnync(processRouteCode);
            processRouteInOutBounds = processRouteInOutBounds.OrderBy(x => x.Sort).ToList();
            foreach (var item in processRouteInOutBounds)
            {
                var resourceCode = item.ResourceCode;
                //设置当前模拟设备名称
                SetEquInfoAsync(item);
                //进站
                await _inBoundService.InBoundMoreAsync(new InBoundMoreDto
                {
                    LocalTime = HymsonClock.Now().AddMilliseconds(-1),
                    ResourceCode = resourceCode,
                    SFCs = sfcs.ToArray()
                });
                //出站
                List<OutBoundSFCDto> sfcList = new();
                foreach (var sfc in sfcs)
                {
                    sfcList.Add(new OutBoundSFCDto
                    {
                        SFC = sfc,
                        Passed = 1//都TM合格
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
