

using System.Text.Json.Serialization;

namespace ProyectoFinal_BibliotecaPersonal.Models.Api
{

    public class BookSearchResult
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public List<BookVolume>? Items { get; set; }
    }

    public class BookVolume
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("volumeInfo")]
        public VolumeInfo? VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("authors")]
        public List<string>? Authors { get; set; }

        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("publishedDate")]
        public string? PublishedDate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("pageCount")]
        public int? PageCount { get; set; }

        [JsonPropertyName("categories")]
        public List<string>? Categories { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("industryIdentifiers")]
        public List<IndustryIdentifier>? IndustryIdentifiers { get; set; }

        [JsonPropertyName("imageLinks")]
        public ImageLinks? ImageLinks { get; set; }
    }

    public class IndustryIdentifier
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; } 

        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }
    }

    public class ImageLinks
    {
        [JsonPropertyName("smallThumbnail")]
        public string? SmallThumbnail { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }
    }
}