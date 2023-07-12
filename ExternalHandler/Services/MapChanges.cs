using Microsoft.Extensions.Options;
using ExternalHandler.Services;
using ExternalHandler.Settings;

namespace ExternalHandler.Services
{
    public class MapChanges : IMapChanges
    {
        public MapChanges(IOptionsSnapshot<AppSettings> optionsSnapshot)
        {

        }
    }
    public interface IMapChanges
    {

    }
}
