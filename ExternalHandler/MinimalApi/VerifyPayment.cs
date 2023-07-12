using ExternalHandler.Services;
using ExternalHandler.Settings;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using ExternalHandler.UseCase.ZibalPayment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PefectMoney.Shared.Utility.ResultUtil;
using RestSharp;

namespace ExternalHandler.MinimalApi
{
    public static class VerifyPayment
    {
        public static WebApplication MapVerifyPayment(this WebApplication app)
        {

            app.MapGet("/verifypayment", async (HttpContext http, veriPaymentRequest request,
                [FromServices] IZibalVerifyPaymentHandler verifyPayment, [FromServices] IOptionsSnapshot<AppSettings> appSettings,
                [FromServices] IOptionsSnapshot <ZibalPaymentSettings> zibalSettings ,ILogger<WebApplication> logger,
                 CancellationToken cancellation) =>
            {
                // Check TrackId 
                if(string.IsNullOrWhiteSpace(request.trackId))
                {
                    http.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return ;
                }
                 //is Valid Basket From telegram server
                 //Code


                    try
                {
                    RestSharp.RestClient restClient = new RestSharp.RestClient(appSettings.Value.UrlValidBasketWithTrackId);
                    RestSharp.RestRequest restRequest = new RestSharp.RestRequest();
                    restRequest.AddJsonBody(new { trackId = request.trackId });
                    var response = await restClient.GetAsync<ResultHandler>(restRequest);
                    if (response == null)
                    {
                        Console.WriteLine($"{"message"}:{"have problem from telegram bot"}");

                        return;
                    }
                    if (response.IsSuccess == false)
                    {
                        string message = string.Empty;
                        foreach (var item in response.Message)
                        {
                            message = item;
                            break;
                        }
                        logger.LogError($"{"message"}:{message}");

                        return;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"{e.Message}---{e.InnerException?.Message}");
                }



                // if result is true Process Verify Basket 

                var result = await verifyPayment.Handle(new ZibalVerifyPaymentRequestDto { trackId = request.trackId}, cancellation);
                var responseToUser = ResultHandler<ZibalVerifyPaymentResponseDto>.MapToResultHandler(result);
                logger.LogInformation(responseToUser.ToString());
                try
                {
                    RestSharp.RestClient restClient = new RestSharp.RestClient(appSettings.Value.BaseTelegramUrl);
                    RestSharp.RestRequest restRequest = new RestSharp.RestRequest(zibalSettings.Value.UrlVerifyPaymentRequest);
                    restRequest.AddJsonBody(result);
                    await restClient.PostAsync(restRequest);
                }
                catch (Exception e)
                {

                    logger.LogError($"{e.Message}---{e.InnerException?.Message}");
                }
                return;
            });
            return app;
        }
    }

    public record veriPaymentRequest
    {
        [FromBody]
        public string success { get; set; }
        [FromBody]
        public string status { get; set; }
        [FromBody]
        public string trackId { get; set; }
        [FromBody]
        public string orderId { get; set; }

    }
}
