using CloudinaryDotNet;
using HappyWedding.Api.Models.Domain;
using Microsoft.Extensions.Options;

namespace HappyWedding.Api.Services;

public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg" };

    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    public CloudinaryService(IOptions<CloudinarySettings> settings, ILogger<CloudinaryService> logger)
    {
        _logger = logger;

        var cfg = settings.Value;
        var account = new Account(cfg.CloudName, cfg.ApiKey, cfg.ApiSecret);
        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    }

    // -------------------------------------------------------------------------
    // Upload single image
    // -------------------------------------------------------------------------
    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string? folder = null)
    {
        ValidateFile(file);

        await using var stream = file.OpenReadStream();

        var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder ?? "uploads",
            // Auto-generate a unique public ID
            Overwrite = true,   
            UniqueFilename = false,
            UseFilename = true,
            // Strip metadata, auto-quality, auto-format
            // Transformation = new Transformation()
            //     .Quality("auto")
            //     .FetchFormat("auto")
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error is not null)
        {
            _logger.LogError("Cloudinary upload failed: {Error}", uploadResult.Error.Message);
            throw new InvalidOperationException($"Image upload failed: {uploadResult.Error.Message}");
        }

        _logger.LogInformation("Uploaded image {PublicId}", uploadResult.PublicId);

        return MapToResult(uploadResult, file.FileName);
    }

    // -------------------------------------------------------------------------
    // Upload multiple images
    // -------------------------------------------------------------------------
    public async Task<IEnumerable<ImageUploadResult>> UploadMultipleImagesAsync(
        IList<IFormFile> files, string? folder = null)
    {
        if (files is null || files.Count == 0)
            throw new ArgumentException("No files provided.");

        if (files.Count > 10)
            throw new ArgumentException("Maximum 10 files per request.");

        var tasks = files.Select(f => UploadImageAsync(f, folder));
        return await Task.WhenAll(tasks);
    }

    // -------------------------------------------------------------------------
    // Delete image by publicId
    // -------------------------------------------------------------------------
    public async Task<bool> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            throw new ArgumentException("Public ID must not be empty.", nameof(publicId));

        var deleteParams = new CloudinaryDotNet.Actions.DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error is not null)
        {
            _logger.LogError("Cloudinary delete failed for {PublicId}: {Error}", publicId, result.Error.Message);
            throw new InvalidOperationException($"Image deletion failed: {result.Error.Message}");
        }

        var deleted = result.Result == "ok";
        _logger.LogInformation("Delete {PublicId}: {Result}", publicId, result.Result);
        return deleted;
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------
    private static void ValidateFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("File is empty.");

        if (file.Length > MaxFileSizeBytes)
            throw new ArgumentException($"File exceeds maximum size of {MaxFileSizeBytes / 1024 / 1024} MB.");

        var ext = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(ext))
            throw new ArgumentException($"File type '{ext}' is not allowed. Allowed: {string.Join(", ", AllowedExtensions)}");
    }

    private ImageUploadResult MapToResult(CloudinaryDotNet.Actions.ImageUploadResult src, string originalFileName) =>
        new()
        {
            PublicId = src.PublicId,
            Url = src.Url?.ToString() ?? string.Empty,
            SecureUrl = BuildDeliveryUrl(src.PublicId, _cloudinary.Api.Account.Cloud),
            Format = src.Format,
            Width = src.Width,
            Height = src.Height,
            Bytes = src.Bytes,
            OriginalFileName = originalFileName,
            CreatedAt = src.CreatedAt
        };


    // When returning the URL, build a transformed delivery URL instead:
    private static string BuildDeliveryUrl(string publicId, string cloudName)
    {
        return $"https://res.cloudinary.com/{cloudName}/image/upload/f_auto,q_auto/{publicId}";
    }
}