namespace HttpSherpa
{
    public class GuidanceResult
    {
        public string GuidanceId { get; set; }
        /// <summary>
        /// The original scenario that the guidance is refering to
        /// </summary>
        public Scenario Scenario { get; set; }
        /// <summary>
        /// Output is in markdown and can be post-processed
        /// for varying mediums (web, console, logs..)
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// The opinion of the guidance
        /// </summary>
        public GuidanceOpinion Opinion { get; set; } = GuidanceOpinion.Unknown;
    }

    public class GuidanceResult<T> : GuidanceResult
    {
        public new Scenario<T> Scenario { get; set; }
    }
}