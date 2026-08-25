using System.Text.Json.Serialization;

namespace SystemBridge.Api.Models;

public class PackagingType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<ProductPackaging> Packagings { get; set; } = new List<ProductPackaging>();
}