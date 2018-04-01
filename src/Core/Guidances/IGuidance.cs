namespace HttpSherpa.Guidances
{
    public interface IGuidance
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        GuidanceResult Process(Scenario scenario);
    }

    public interface IGuidance<T> : IGuidance
    {
         GuidanceResult<T> Process(Scenario<T> scenario);
    }
}