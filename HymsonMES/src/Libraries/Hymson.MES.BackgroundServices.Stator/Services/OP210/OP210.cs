namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 
    /// </summary>
    public class OP210 : BaseOPEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string PeakPressure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndPressure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WaitTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PressTemp { get; set; }

    }
}
