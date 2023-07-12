using ExternalHandler.Settings;
using MediatR;
using Newtonsoft.Json;

using PefectMoney.Shared.Utility.ResultUtil;

using System.Net;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace ExternalHandler.UseCase.ZibalPayment
{
    public interface IZibalInquiryPaymentHandler
    {
         Task<ResultOperation<ZibalInquiryResponseDto>> Handle(ZibalInquiryPaymentRequest request, CancellationToken cancellationToken);
    }

    public record ZibalInquiryPaymentRequest : IRequest<ResultOperation<ZibalInquiryResponseDto>>
    {
        [FromBody]
        public string trackId { get; set; }
    }

    public class ZibalInquiryPaymentHandler : IRequestHandler<ZibalInquiryPaymentRequest, ResultOperation<ZibalInquiryResponseDto>> , IZibalInquiryPaymentHandler
    {
        public ZibalPaymentSettings PaymentSettings { get; }
        public ILogger<ZibalInquiryPaymentHandler> Logger { get; }

        public ZibalInquiryPaymentHandler(IOptionsSnapshot<ZibalPaymentSettings> paymentSettings,ILogger<ZibalInquiryPaymentHandler> logger)
        {
            PaymentSettings = paymentSettings.Value;
            Logger = logger;
        }

        public async Task<ResultOperation<ZibalInquiryResponseDto>> Handle(ZibalInquiryPaymentRequest request, CancellationToken cancellationToken)
        {

            try
            {
                string url = PaymentSettings.UrlVerifyPaymentRequest; // url
                Zibal.verifyRequest Request = new Zibal.verifyRequest(); // define Request
                Request.merchant = PaymentSettings.Merchant; // String
                Request.trackId = request.trackId; // String 
                var httpResponse = Zibal.HttpRequestToZibal(url, JsonConvert.SerializeObject(Request));  // get Response
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
                {
                    var responseText = streamReader.ReadToEnd(); // read Response
                    ZibalInquiryResponseDto result = JsonConvert.DeserializeObject<ZibalInquiryResponseDto>(responseText); // Deserilize as response class object
                                                                                                                   // you can access paidAt time with item.paidAt , result with item.result , message with item.message , status with item.status and amount with item.amount
                                                                                                                   // you can access paidAt time with item.paidAt , result with item.result , message with item.message , status with item.status and amount with item.amount
                    if (result == null)
                    {
                        Logger.LogError($"ZibalVerifyPaymentResponseDto result is Null For {request.trackId}"); // print exception error
                        return new ZibalInquiryResponseDto()
                        {
                            errorMessage = $"ZibalVerifyPaymentResponseDto result is Null For {request.trackId}",

                            HaveError = true
                        }.ToSuccessResult();
                    }
                    return result.ToSuccessResult();

                }
            }
            catch (WebException ex)
            {
                Logger.LogError(ex.Message, ex.InnerException?.Message); // print exception error
                return new ZibalInquiryResponseDto()
                {
                    errorMessage = ex.Message,
                    innerErrorMessage = ex.InnerException?.Message!,
                    HaveError = true
                }.ToSuccessResult();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex.InnerException?.Message); // print exception error
                return new ZibalInquiryResponseDto()
                {
                    errorMessage = ex.Message,
                    innerErrorMessage = ex.InnerException?.Message!,
                    HaveError = true
                }.ToSuccessResult();
            }
        }
    }

    public class ZibalInquiryResponseDto
    {
        public string paidAt { get; set; }
        public string createdAt { get; set; }
        public string cardNumber { get; set; }
        public string refNumber { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string wage { get; set; }
        public string amount { get; set; }
        public string result { get; set; }
        public string message { get; set; }
        public bool HaveError { get; set; }
        public string errorMessage { get; set; }
        public string innerErrorMessage { get; set; }
    }
}
