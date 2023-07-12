

using Microsoft.Extensions.Options;

namespace ExternalHandler.Settings
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }

}
