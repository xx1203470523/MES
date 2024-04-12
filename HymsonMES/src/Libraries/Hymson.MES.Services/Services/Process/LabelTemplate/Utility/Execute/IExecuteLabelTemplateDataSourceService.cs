using Hymson.MES.BackgroundServices.Dtos.Manufacture.LabelTemplate;

namespace Hymson.MES.BackgroundServices.Manufacture.PrintTemplate.Utility.Execute
{
    public interface IExecuteLabelTemplateDataSourceService
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task Execute(LabelTemplateSourceDto param);
    }
}
