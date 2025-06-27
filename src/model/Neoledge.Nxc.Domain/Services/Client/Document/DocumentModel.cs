using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neoledge.Nxc.Domain.Services.Client.Document
{
    public class DocumentModel
    {
        //public required IList<DocumentAttachment> Attachments { get; init; }

        public string? Description { get; init; }

        public required string MailId { get; init; }

        public required string Reference { get; init; }

        public required string Subject { get; init; }

        // Custom properties in array format
        public List<DocumentCustomProperty> CustomProperties { get; set; } = new();

        // Serialization options
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // Helper methods
        public void AddCustomProperty(string key, string value)
        {
            CustomProperties.Add(new DocumentCustomProperty { Key = key, Value = value });
        }

        public string GetCustomProperty(string key)
        {
            return CustomProperties.FirstOrDefault(f => f.Key == key)?.Value ?? string.Empty;
        }

        // Serialization
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, _jsonOptions);
        }

        public static DocumentModel? FromJson(string json)
        {
            return JsonSerializer.Deserialize<DocumentModel>(json);
        }

        // Convert document to stream
        public async Task ToStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            await JsonSerializer.SerializeAsync(stream, this, _jsonOptions, cancellationToken).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin); // Reset for reading
        }

        // Create document from stream
        public static async Task<DocumentModel?> FromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            //stream.Seek(0, SeekOrigin.Begin); // Ensure we start reading from the beginning
            return await JsonSerializer.DeserializeAsync<DocumentModel>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}