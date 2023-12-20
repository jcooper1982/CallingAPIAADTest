using System.Text.Json.Serialization;

namespace CallingAPIAADTest
{
    public class CalledAPIResponse
    {
        [JsonPropertyName("apI2Response")]
        public String API2Response { get; set; }
    }
}
