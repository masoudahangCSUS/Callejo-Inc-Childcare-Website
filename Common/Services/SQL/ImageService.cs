using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;


public class ImageService
{
    private readonly string _connectionString;
    private readonly HttpClient _httpClient;
    private const string GoogleDriveApiKey = "AIzaSyBT0xpKBYukH7xxF3Hwo67_Sj5yTVZbVB0";
    private const string FolderId = "15hKk_wnES5hlqe5t1AQbmuhMhFQUBbOm";


    public ImageService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _connectionString = configuration.GetConnectionString("DataContext");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }
    }

    public async Task<List<string>> GetGoogleDriveImagesAsync()
    {
        string apiUrl = $"https://www.googleapis.com/drive/v3/files?q=%27{FolderId}%27+in+parents+and+mimeType+contains+%27image/%27&key={GoogleDriveApiKey}&fields=files(id,name,mimeType)";

        try
        {
            Console.WriteLine($"DEBUG: Making API request to {apiUrl}");

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"DEBUG API RESPONSE: {responseString}");

            var googleDriveResponse = JsonSerializer.Deserialize<GoogleDriveResponse>(responseString, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            });

            if (googleDriveResponse == null || googleDriveResponse.Files == null || googleDriveResponse.Files.Count == 0)
            {
                Console.WriteLine("DEBUG: Google Drive API returned null or empty response.");
                return new List<string>();
            }


            Console.WriteLine($"DEBUG: Found {googleDriveResponse.Files.Count} files in Google Drive.");

            var imageUrls = googleDriveResponse.Files
                .Where(file => !string.IsNullOrEmpty(file.Id))
                .Select(file => $"https://lh3.googleusercontent.com/d/{file.Id}")
                .ToList();



            foreach (var url in imageUrls)
            {
                Console.WriteLine($"DEBUG: WebContentLink URL - {url}");
            }



            Console.WriteLine($"DEBUG: Returning {imageUrls.Count} image URLs.");
            return imageUrls;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"ERROR: API request failed - {ex.Message}");
            return new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Unexpected error - {ex.Message}");
            return new List<string>();
        }
    }




    public async Task SaveImageUrlAsync(string imageUrl)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var query = "INSERT INTO Images (image_url, is_published, uploaded_at) VALUES (@ImageUrl, 0, GETUTCDATE())";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }

    public async Task<string> GetPublishedImageUrlAsync()
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var query = "SELECT TOP 1 image_url FROM Images WHERE is_published = 1 ORDER BY id DESC";
            using var cmd = new SqlCommand(query, conn);

            try
            {
                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return null;
            }
        }
    }

    public async Task MarkImageAsPublishedAsync(string imageUrl)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var query = "UPDATE Images SET is_published = 0; UPDATE Images SET is_published = 1 WHERE image_url = @ImageUrl";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
        }
    }

    public async Task<string> GetLatestImageUrlAsync()
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var query = "SELECT TOP 1 image_url FROM Images ORDER BY uploaded_at DESC";
            using var cmd = new SqlCommand(query, conn);

            try
            {
                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return null;
            }
        }
    }
    private class GoogleDriveResponse
    {
        [JsonPropertyName("files")]
        public List<GoogleDriveFile> Files { get; set; } = new();
    }

    private class GoogleDriveFile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; }
    }

}