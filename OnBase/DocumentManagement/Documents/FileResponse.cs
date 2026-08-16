using HyRest.Utilities;
using Refit;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using HyRest.FileTypeMapping;

namespace HyRest.OnBase.Core;

public partial class FileResponse : IDisposable
{
    public FileResponse(IApiResponse<Stream> apiResponse) => _apiResponse = apiResponse;
    private IApiResponse<Stream> _apiResponse { get; set; }
    public bool IsPartial => StatusCode == HttpStatusCode.PartialContent;
    public Exception? Error => _apiResponse.Error;
    public bool IsSuccessful { get => _apiResponse.IsSuccessful; }
    public string? Extension
    {
        get
        {
            if (MimeType != null && FileTypeMap.TryGetExtension(MimeType, out string ext))
                return ext;
            return null;
        }
    }
    public string? MimeType
    {
        get
        {
            if (_apiResponse != null && _apiResponse.ContentHeaders != null)
            {
                var contentType = _apiResponse.ContentHeaders.FirstOrDefault(h => h.Key == "Content-Type");
                if (contentType.Value != null && contentType.Value.Count() > 0)
                {
                    var mimeType = contentType.Value.FirstOrDefault();
                    if (mimeType == null)
                        return null;
                    return mimeType;
                }
            }
            return null;
        }
    }
    public Stream? Content => _apiResponse.Content;
    [JsonIgnore]
    public HttpResponseHeaders? Headers => _apiResponse.Headers;
    [JsonIgnore]
    public HttpContentHeaders? ContentHeader => _apiResponse.ContentHeaders;
    [JsonIgnore]
    public HttpStatusCode? StatusCode => _apiResponse.StatusCode;

    /// <summary>
    /// Saves the stream to the specified folder path or to temp file path if no folder path is provided.
    /// <br/> The file will be given a temp file name with the extension provided by the content header mime type.
    /// </summary>
    /// <param name="folderPath">The folder path to save the file.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<string> SaveToFileAsync(string? folderPath = null, CancellationToken cancellationToken = default)
    {
        if (Content == null)
            throw new Exception("There is no content to save.");
        var fileName = Guid.NewGuid().ToString();

        if (string.IsNullOrWhiteSpace(folderPath))
            folderPath = Path.GetTempPath();
        var filePath = Path.Combine(folderPath, $"{fileName}{Extension ?? "undefined"}");

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 81920, useAsync: true);
        await Content.CopyToAsync(fileStream, cancellationToken);
        await fileStream.FlushAsync(cancellationToken);
        return filePath;
    }

    /// <summary>
    /// Saves the stream to the specified file path synchronously.
    /// </summary>
    /// <param name="folderPath">The full path where the file should be saved.</param>
    /// /// <param name="fileName">The full path where the file should be saved. The extensio</param>
    public string SaveToFile(string folderPath, string? fileName = null)
    {
        if (Content == null)
            throw new Exception("There is no content to save.");

        string? extension = null;
        if (fileName == null)
            fileName = $"{Guid.NewGuid().ToString()}.{Extension}";
        else
            extension = Path.GetExtension(fileName);

        if (extension == null)
            extension = Extension;
        else if (extension != Extension)
            fileName = Path.ChangeExtension(fileName, extension);

            var filePath = Path.Combine(folderPath, fileName);

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        Content.CopyTo(fileStream);
        fileStream.Flush();
        return filePath;
    }

    public void Dispose()
    {
        Content?.Dispose();
    }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}