using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


public class ImageService
{
    private readonly string _connectionString;


    public ImageService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DataContext");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
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
}