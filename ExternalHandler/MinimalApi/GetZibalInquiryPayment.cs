using ExternalHandler.Services;
using ExternalHandler.Settings;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using ExternalHandler.UseCase.ZibalPayment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExternalHandler.MinimalApi
{
    public static class GetZibalInquiryPayment
    {
        public static WebApplication MapZibalInquiryPayment(this WebApplication app)
        {

            app.MapPost("/zibalinquirypayment", async (HttpContext http, VerifyUserCardRequestDto request, [FromServices] IOptionsSnapshot<AppSettings> appSettings,
                [FromServices] IVerifyUserCard verifyUserCard, CancellationToken cancellation) =>
            {
                if (request == null || request.token != appSettings.Value.Token)
                {
                    http.Response.StatusCode = 401;
                    return;
                };
                // Check Token     
                request.trackId = Guid.NewGuid().ToString();
                var result = await verifyUserCard.Handle(request, cancellation);
                var responseToUser = ResultHandler<VerifyUserCardResponseDto>.MapToResultHandler(result);
                await http.Response.WriteAsJsonAsync(responseToUser);

            });
            return app;
        }
    }
}
