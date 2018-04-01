using System.Net.Http;
using HttpSherpa;
using HttpSherpa.Guidances;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Guidances
{
    public class CookieMonsterTests
    {
        private readonly ITestOutputHelper output;
        private readonly IGuidance guidance = new CookieMonsterGuidance();

        public CookieMonsterTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Can_determine_good_result_with_no_cookies()
        {
            var scenario = new Scenario
            {
                Request = new HttpRequestMessage()
            };

            var result = guidance.Process(scenario);
            
            Assert.Equal(GuidanceOpinion.Good, result.Opinion);
            output.WriteLine(result.Output);
        }
        
        [Fact]
        public void Can_determine_good_result_with_one_cookie()
        {
            var scenario = new Scenario
            {
                Request = new HttpRequestMessage
                {
                    Headers =
                    {
                        {"Cookie", new[] {"one"}}
                    }
                }
            };

            var result = guidance.Process(scenario);
            
            Assert.Equal(GuidanceOpinion.Good, result.Opinion);
            output.WriteLine(result.Output);
        }
        
        [Fact]
        public void Can_determine_bad_result_based_on_count()
        {
            var scenario = new Scenario
            {
                Request = new HttpRequestMessage
                {
                    Headers =
                    {
                        {"Cookie", new[] {"one", "two", "three", "four"}}
                    }
                }
            };

            var result = guidance.Process(scenario);
            
            Assert.Equal(GuidanceOpinion.Bad, result.Opinion);
            output.WriteLine(result.Output);
        }
        
        [Fact]
        public void Can_determine_bad_result_based_on_size()
        {
            /* 4000 bytes */
            var text = new string(new char[4000]);
            
            var scenario = new Scenario
            {
                Request = new HttpRequestMessage
                {
                    Headers =
                    {
                        {"Cookie", new[] { text }}
                    }
                }
            };

            var result = guidance.Process(scenario);
            
            Assert.Equal(GuidanceOpinion.Bad, result.Opinion);
            output.WriteLine(result.Output);
        }
    }
}