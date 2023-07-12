using ExternalHandler.Services;
using ExternalHandler.Settings;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using ExternalHandler.UseCase.ZibalPayment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

namespace ExternalHandler.MinimalApi
{
    public record GetLinkPaymentRequest
    {
        [FromBody]
        public string token { get; set; }
        [FromBody]
        public string orderId { get; set; }
        [FromBody]
        public int amount { get; set; }
        //public string callbackUrl { get; set; }
        
        [FromBody]
        public string mobile { get; set; }
        [FromBody]
        public string description { get; set; }
        [FromBody]
        public string[] allowedCards { get; set; }

        public ZibalPaymentRequest MapToZibalPaymentRequest()
        {
            return new ZibalPaymentRequest
            {
                allowedCards = this.allowedCards,
                amount = this.amount,
                mobile = this.mobile,
                orderId = this.orderId,
                description = this.description,
                
            };
        }

    }
    public static class GetPaymentLink
    {
        
        public static WebApplication MapGetPaymentLink(this WebApplication app)
        {

            _ = app.MapPost("/GetPaymentLink", async (HttpContext http, GetLinkPaymentRequest request, [FromServices] IOptionsSnapshot<AppSettings> appSettings,
                [FromServices] IZiBalPaymentHandler ziBalPayment, CancellationToken cancellation) =>
            {
                if (request == null || request.token != appSettings.Value.Token)
                {
                    http.Response.StatusCode = 401;
                   
                  
                    return;
                };
                // Check Token 
                var result = await ziBalPayment.Handle(request.MapToZibalPaymentRequest(), cancellation);
                var responseToUser = ResultHandler<ZiBalPaymentResponse>.MapToResultHandler(result);
                await http.Response.WriteAsJsonAsync(responseToUser);
            });
            return app;
        }
    }
}
