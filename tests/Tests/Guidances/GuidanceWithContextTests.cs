using HttpSherpa;
using HttpSherpa.Guidances;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Guidances
{
    public class GuidanceWithContextTests
    {
        private readonly ITestOutputHelper output;
        private readonly IGuidance guidance = new TestGuidance();

        public GuidanceWithContextTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Can_call_process_with_scenario()
        {
            var result = guidance.Process(new Scenario());        
            Assert.Contains("Default", result.Output);
            output.WriteLine(result.Output);
        }
        
        [Fact]
        public void Can_call_process_with_scenario_of_T()
        {
            var result = guidance.Process(new Scenario<Options>
            { 
                Context = new Options { Name = "Bob" }
            });
            
            Assert.Contains("Bob", result.Output);
            output.WriteLine(result.Output);
        }

        [Fact]
        public void Can_get_scenario_of_T_from_result()
        {
            var result = guidance.Process(new Scenario<Options>
            { 
                Context = new Options { Name = "Ross" }
            });
            
            Assert.True(result is GuidanceResult<Options>);
            output.WriteLine(result.Output);
        }
    }
    
    public class TestGuidance : IGuidance<Options> 
    {
        public string Id { get; } = "test-context";
        public string Name { get; } = "Test";
        public string Description { get; } = "Testing Generic Context";
        
        public GuidanceResult<Options> Process(Scenario<Options> scenario)
        {
            return Process((Scenario) scenario) as GuidanceResult<Options>;
        }
        
        public GuidanceResult Process(Scenario scenario)
        {
            // A Guidance should have a default Context
            // in the chance that a non-generic scenario
            // is passed in.
            var scenarioOfOptions = 
                scenario is Scenario<Options> strong 
                ? strong 
                : new Scenario<Options> { Request = scenario.Request, Response = scenario.Response, Context = new Options() };

            var result = new GuidanceResult<Options>()
            {
                GuidanceId = Id,
                Scenario = scenarioOfOptions,
                Opinion = GuidanceOpinion.Unknown,
                Output = $"¯\\_(ツ)_/¯ ({scenarioOfOptions.Context.Name})"
            };

            return result;
        }

    }

    public class Options
    {
        public string Name { get; set; } = "Default";
    }
}