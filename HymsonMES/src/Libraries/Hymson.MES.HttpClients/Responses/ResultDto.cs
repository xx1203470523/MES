namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string Msg { get; set; } = "Success";

        /// <summary>
        /// 
        /// </summary>
        public object? Data { get; set; }

    }
}
