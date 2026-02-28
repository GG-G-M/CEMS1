namespace CEMS.Services
{
    public interface IS3StorageService
    {
        /// <summary>Whether S3 storage is configured and available.</summary>
        bool IsEnabled { get; }

        /// <summary>Upload a file and return the S3 object key.</summary>
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);

        /// <summary>Generate a pre-signed URL for secure, time-limited access.</summary>
        string? GetPreSignedUrl(string key, int expirationMinutes = 15);

        /// <summary>Delete a file from S3.</summary>
        Task DeleteFileAsync(string key);
    }
}
