using System.Reflection;

namespace ImageAPI.ResourceLoaders;

public abstract class ResourceLoader<TResource>
{
    private readonly string resourceName;
    public abstract TResource Resource { get; }

    protected ResourceLoader(string resourceName)
    {
        this.resourceName = resourceName;
    }

    public abstract TResource Load();

    protected Stream ResourceStream() =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException(ErrorMessage());

    private string ErrorMessage() =>
        $"Could not find the embedded resource '{resourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.";
}