using System.Net.Http.Json;
using System.Text.Json;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Models.Api;

namespace ProyectoFinal_BibliotecaPersonal.Services
{
    public class BookApiService
    {
        private readonly HttpClient _httpClient;
        private readonly DatabaseService _database; 

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BookApiService(HttpClient httpClient, DatabaseService database)
        {
            _httpClient = httpClient;
            _database = database;
        }

        public async Task<List<Book>> SearchBooksAsync(
            string query,
            int maxResults = 20,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Book>();

            maxResults = Math.Clamp(maxResults, 1, 40);

            var encodedQuery = Uri.EscapeDataString(query);
            var requestUrl = $"volumes?q={encodedQuery}&maxResults={maxResults}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl, ct);
                response.EnsureSuccessStatusCode();

                var searchResult = await response.Content
                    .ReadFromJsonAsync<BookSearchResult>(_jsonOptions, ct);

                if (searchResult?.Items is null || searchResult.Items.Count == 0)
                    return new List<Book>();

                // mapear al modelo Book de Xavier
                return searchResult.Items
                    .Select(MapToBook)
                    .ToList();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BookApiService] HTTP error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BookApiService] JSON error: {ex.Message}");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(string googleBooksId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(googleBooksId))
                return null;

            var response = await _httpClient.GetAsync($"volumes/{googleBooksId}", ct);
            if (!response.IsSuccessStatusCode)
                return null;

            var volume = await response.Content.ReadFromJsonAsync<BookVolume>(_jsonOptions, ct);
            return volume is null ? null : MapToBook(volume);
        }


        public async Task<bool> AddBookToLibraryAsync(Book book)
        {
            if (book is null)
                return false;

            if (!string.IsNullOrWhiteSpace(book.ISBN))
            {
                var existing = await _database.GetBooksAsync();
                bool yaExiste = existing.Any(b =>
                    !string.IsNullOrWhiteSpace(b.ISBN) &&
                    string.Equals(b.ISBN, book.ISBN, StringComparison.OrdinalIgnoreCase));

                if (yaExiste)
                    return false;
            }

            book.Id = 0;
            await _database.SaveBookAsync(book);
            return true;
        }

        private static Book MapToBook(BookVolume volume)
        {
            var info = volume.VolumeInfo;

            return new Book
            {

                Title = info?.Title ?? string.Empty,

                Author = info?.Authors is { Count: > 0 }
                    ? string.Join(", ", info.Authors)
                    : string.Empty,

                ISBN = ExtractIsbn(info?.IndustryIdentifiers) ?? string.Empty,

                Year = ParseYear(info?.PublishedDate),

                Genre = info?.Categories is { Count: > 0 }
                    ? Truncate(info.Categories[0], 50)
                    : string.Empty,

                Pages = info?.PageCount ?? 0,

                CoverUrl = NormalizeThumbnail(info?.ImageLinks?.Thumbnail
                    ?? info?.ImageLinks?.SmallThumbnail)
                    ?? string.Empty,

                IsRead = false,

                Rating = 0, 

                Notes = string.Empty,

                DateAdded = DateTime.Now
            };
        }

        // --- Helpers de mapeo ---

        private static string? ExtractIsbn(List<IndustryIdentifier>? identifiers)
        {
            if (identifiers is null || identifiers.Count == 0)
                return null;

            return identifiers.FirstOrDefault(i => i.Type == "ISBN_13")?.Identifier
                ?? identifiers.FirstOrDefault(i => i.Type == "ISBN_10")?.Identifier;
        }

        private static int ParseYear(string? publishedDate)
        {
            if (string.IsNullOrWhiteSpace(publishedDate))
                return 0;

            var yearPart = publishedDate.Length >= 4 ? publishedDate.Substring(0, 4) : publishedDate;
            return int.TryParse(yearPart, out var year) ? year : 0;
        }

        private static string? NormalizeThumbnail(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            return url.StartsWith("http://")
                ? "https://" + url.Substring("http://".Length)
                : url;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}

