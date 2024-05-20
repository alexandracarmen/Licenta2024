
namespace Sentinel.Frontend.Services;

public class InMemoryExtensionService : IExtensionService
{
    private Dictionary<Guid, string> GuidTextDictionary { get; init; } = [];

    public string Load(Guid guid) => GuidTextDictionary[guid];

    public Response Save(string text)
    {
        var guid = Guid.NewGuid();
        GuidTextDictionary.Add(guid, text);
        return new Response(guid);
    }
}