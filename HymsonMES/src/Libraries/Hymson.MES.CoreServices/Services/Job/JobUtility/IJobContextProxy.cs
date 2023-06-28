namespace Hymson.MES.CoreServices.Services.Job.JobUtility
{
    public interface IJobContextProxy
    {
        void Dispose();
        ICollection<uint> GetKeys();
        TResult? GetValue<T, TResult>(Func<T, TResult> func, T parameter);
        Task<TResult?> GetValueAsync<T, TResult>(Func<T, Task<TResult>> func, T parameter);
        void InitDictionary();
    }
}