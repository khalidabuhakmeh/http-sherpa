using System;
using System.Linq;
using System.Text;

namespace HttpSherpa.Guidances
{
    /// <summary>
    /// Inspects cookies on a request and determines the
    /// count and the size.
    ///                  _/0\/ \_
    ///         .-.   .-` \_/\0/ '-.
    ///        /:::\ / ,_________,  \
    ///       /\:::/ \  '. (:::/  `'-;
    ///       \ `-'`\ '._ `"'"'\__    \
    ///        `'-.  \   `)-=-=(  `,   |
    ///            \  `-"`      `"-`   /
    /// </summary>
    public class CookieMonsterGuidance : IGuidance
    {
        const string CookieKey = "Cookie";
        const int MaxCookieCount = 3;
        const int MaxCookieSizeInBytes = 3000 /* bytes */;
        
        public string Id { get; } = "cookie-monster";
        public string Name { get; } = "Cookie Monster";

        public string Description { get; } =
            "Determines if the number and size of the cookies on a request will lead to potential perfomance issues.";
        
        public GuidanceResult Process(Scenario scenario)
        {
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));
            if (scenario.Request == null) throw new ArgumentNullException(nameof(scenario.Request));
            
            var result = new GuidanceResult
            {
                GuidanceId = Id,
                Output = string.Format(Good, 0 /* cookies */, 0 /* bytes */),
                Scenario = scenario,
                Opinion = GuidanceOpinion.Good
            };

            if (!scenario.Request.Headers.TryGetValues(CookieKey, out var results)) 
                return result;
           
            var all = results.ToList();
                
            var cookies = new
            {
                count = all.Count,
                size = all.Sum(value => Encoding.UTF8.GetByteCount(value))
            };

            var outcome = cookies.count <= MaxCookieCount && cookies.size <= MaxCookieSizeInBytes; 
                
            result.Output = string.Format(outcome ? Good : Bad, cookies.count, cookies.size);
            result.Opinion = outcome ? GuidanceOpinion.Good : GuidanceOpinion.Bad;

            return result;
        }

        private const string Good =
            "The request had **{0} ({1} bytes)** cookies present, which is within the acceptable number of cookies.";

        private const string Bad =
            "The request had **{0} ({1} bytes)** cookies present, which could potentially " +
            "lead to slower requests and server processing times. Please consider reducing " +
            "the amount of cookies utilized in your request.";
    }
}