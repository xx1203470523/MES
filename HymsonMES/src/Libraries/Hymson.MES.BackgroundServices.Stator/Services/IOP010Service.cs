using Hymson.MES.BackgroundServices.Stator.Model;

namespace Hymson.MES.BackgroundServices.CoreServices
{
    /// <summary>
    /// Hello接口
    /// </summary>
    public interface IOP010Service : IBaseService<OP010Entity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string SayHello(string name);

    }
}
