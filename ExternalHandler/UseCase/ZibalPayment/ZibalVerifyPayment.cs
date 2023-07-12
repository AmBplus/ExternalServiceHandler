using ExternalHandler.Settings;
using MediatR;
using Newtonsoft.Json;

using PefectMoney.Shared.Utility;
using PefectMoney.Shared.Utility.ResultUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExternalHandler.UseCase.ZibalPayment;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace ExternalHandler.UseCase.ZibalPayment
{

    public interface IZibalVerifyPaymentHandler
    {
        Task<ResultOperation<ZibalVerifyPaymentResponseDto>> Handle(ZibalVerifyPaymentRequestDto request, CancellationToken cancellationToken);
    }
    public record ZibalVerifyPaymentRequestDto : IRequest<ResultOperation<ZibalVerifyPaymentResponseDto>>
    {
        [FromQuery]     
        public string trackId { get; set; }
    }
    public class ZibalVerifyPaymentHandler : IRequestHandler<ZibalVerifyPaymentRequestDto, ResultOperation<ZibalVerifyPaymentResponseDto>> , IZibalVerifyPaymentHandler
    {
        public ZibalVerifyPaymentHandler(IOptionsSnapshot<ZibalPaymentSettings> paymentSettings,ILogger<ZibalVerifyPaymentHandler> logger )
        {
            PaymentSettings = paymentSettings.Value;
            Logger = logger;
        }

        public ZibalPaymentSettings PaymentSettings { get; }
        public ILogger<ZibalVerifyPaymentHandler> Logger { get; }

        public async Task<ResultOperation<ZibalVerifyPaymentResponseDto>> Handle(ZibalVerifyPaymentRequestDto request, CancellationToken cancellationToken)
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
                    ZibalVerifyPaymentResponseDto result = JsonConvert.DeserializeObject<ZibalVerifyPaymentResponseDto>(responseText); // Deserilize as response class object
                                                                                                                          // you can access paidAt time with item.paidAt , result with item.result , message with item.message , status with item.status and amount with item.amount
                    if(result == null)
                    {
                        Logger.LogError($"ZibalVerifyPaymentResponseDto result is Null For {request.trackId}"); // print exception error
                        return new ZibalVerifyPaymentResponseDto()
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
                Logger.LogError(ex.Message,ex.InnerException?.Message); // print exception error
                return new ZibalVerifyPaymentResponseDto()
                {
                    errorMessage= ex.Message,
                    innerErrorMessage = ex.InnerException?.Message!,
                    HaveError = true
                }.ToSuccessResult();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex.InnerException?.Message); // print exception error
                return new ZibalVerifyPaymentResponseDto()
                {
                    errorMessage = ex.Message,
                    innerErrorMessage = ex.InnerException?.Message!,
                    HaveError = true
                }.ToSuccessResult();
            }

        }



       
    }
    public class ZibalVerifyPaymentResponseDto
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
        public string orderId { get; set; }
        public string trackId { get; set; }

    }
}
