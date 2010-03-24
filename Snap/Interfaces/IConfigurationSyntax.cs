
namespace Snap
{
    /// <summary>
    /// Extends syntax for aspect configuration.  This is where the fluent configuration
    /// really begins.
    /// </summary>
    public interface IConfigurationSyntax
    {
        void To<TAttribute>();
    }
}
