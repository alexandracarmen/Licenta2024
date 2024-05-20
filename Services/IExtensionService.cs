namespace Sentinel.Frontend.Services;


public record Response(Guid Id);

public interface IExtensionService {
    Response Save(string Text);
    string Load(Guid guid);
}