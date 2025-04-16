using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Common.View;

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

    public async Task SaveImageUrlAsync(string fileName)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var query = "INSERT INTO Images (file_name, is_published, uploaded_at) VALUES (@FileName, 0, GETUTCDATE())";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@FileName", fileName);

        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SQL Error: {ex.Message}");
        }
    }

    public async Task<List<Image>> GetAllImagesAsync()
    {
        var images = new List<Image>();

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var query = "SELECT id, file_name, is_published, uploaded_at FROM Images ORDER BY uploaded_at DESC";
        using var cmd = new SqlCommand(query, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            images.Add(new Image
            {
                Id = reader.GetInt32(0),
                FileName = reader.GetString(1),
                IsPublished = reader.GetBoolean(2),
                UploadedAt = reader.GetDateTime(3)
            });
        }

        return images;
    }

    public async Task<List<Image>> GetPublishedImagesAsync()
    {
        var images = new List<Image>();

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var query = "SELECT id, file_name, is_published, uploaded_at FROM Images WHERE is_published = 1 ORDER BY uploaded_at DESC";
        using var cmd = new SqlCommand(query, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            images.Add(new Image
            {
                Id = reader.GetInt32(0),
                FileName = reader.GetString(1),
                IsPublished = reader.GetBoolean(2),
                UploadedAt = reader.GetDateTime(3)
            });
        }

        return images;
    }

    public async Task MarkImageAsPublishedAsync(string fileName)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var query = "UPDATE Images SET is_published = 0; UPDATE Images SET is_published = 1 WHERE file_name = @FileName";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@FileName", fileName);

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