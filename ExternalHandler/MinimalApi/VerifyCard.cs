using ExternalHandler.Services;
using ExternalHandler.Settings;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExternalHandler.MinimalApi
{
    public static class VerifyCard
    {
        public static WebApplication MapVerifyUserCard(this WebApplication app)
        {

            app.MapPost("/verifycard", async ( HttpContext http,VerifyUserCardRequestDto request, [FromServices] IOptionsSnapshot<AppSettings> appSettings,
                [FromServices] IVerifyUserCard verifyUserCard, CancellationToken cancellation ) =>
            {
                if (request == null || request.token != appSettings.Value.Token)
                {
                    http.Response.StatusCode = 401;
                    return;
                };
                // Check Token 
                if(app.Environment.IsDevelopment())
                {
                    Console.WriteLine("Create A New Guid For VerifyCard Request Track ID");
                    request.trackId = Guid.NewGuid().ToString();
                }
                
                var result = await verifyUserCard.Handle(request, cancellation);

                var responseToUser = ResultHandler<VerifyUserCardResponseDto>.MapToResultHandler(result);   

                await http.Response.WriteAsJsonAsync(responseToUser);
              
            });
            return app;
        }
    }
}
