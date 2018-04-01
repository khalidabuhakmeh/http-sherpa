using System.Net.Http;

namespace HttpSherpa
{
    public class Scenario
    {
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
    }

    public class Scenario<T> : Scenario
    {
        public T Context { get; set; }
    }
}