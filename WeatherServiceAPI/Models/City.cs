using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace WeatherServiceAPI.Models
{
    public class City
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("cityid")]
        [JsonPropertyName("id")]
        public int CityId { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [BsonElement("searchedbyname")]
        public string SearchedByName { get; set; } = String.Empty;

        [BsonElement("latitude")]
        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [BsonElement("longitude")]
        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }

        [BsonElement("timezone")]
        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
    }
}
