using MediatR;
using Microsoft.Extensions.Options;

using ExternalHandler.Settings;

using ExternalHandler.UseCase._BotSettings;

namespace ExternalHandler.UseCase._BotSettings
{
    public class GetBotSettingsRequest : IRequest<AppSettings>
    {
    }
    public class GetBotSettingsHandler : IRequestHandler<GetBotSettingsRequest, AppSettings>
    {
        public GetBotSettingsHandler(IOptions<AppSettings> options)
        {
            BotSettings = options.Value;
        }

        public AppSettings BotSettings { get; }

        public async Task<AppSettings> Handle(GetBotSettingsRequest request, CancellationToken cancellationToken)
        {
            return BotSettings;
        }
    }
}
