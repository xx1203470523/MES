using Hymson.MES.BackgroundServices.Stator.Model;
using SqlSugar.IOC;

namespace Hymson.MES.BackgroundServices.CoreServices
{
    /// <summary>
    /// 
    /// </summary>
    [AppService(ServiceType = typeof(IOP010Service), ServiceLifetime = LifeTime.Transient)]
    public class OP010Service : BaseService<OP010Entity>, IOP010Service, IDynamicApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public OP010Service() { }

        /// <summary>
        /// 数据库使用案例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string SayHello(string name)
        {
            var result = DbScoped.SugarScope.Queryable<OP010Entity>().First();
            Console.WriteLine(JsonConvert.SerializeObject(result));

            return "Hello:" + name;
        }

    }
}
