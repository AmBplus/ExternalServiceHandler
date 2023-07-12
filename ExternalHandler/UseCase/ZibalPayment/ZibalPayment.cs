using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json; // Solution Explorer->Right Click on Project Name -> Click on Manage Nuget Packages -> Search for newtonsoft -> Click on install button 
using System.Net;
using MediatR;
using PefectMoney.Shared.Utility.ResultUtil;
using Microsoft.Extensions.Options;

using PefectMoney.Shared.Utility;

using ExternalHandler.Settings;

namespace ExternalHandler.UseCase.ZibalPayment;

// Send A Link To User To Pay
// Get A Link To Verify Payment 


public record ZibalPaymentRequest : IRequest<ResultOperation<ZiBalPaymentResponse>>
{
    public string orderId { get; set; }
    public int amount { get; set; }
    //public string callbackUrl { get; set; }
    public string mobile { get; set; }
    public string description { get; set; }
    public string[] allowedCards { get; set; }

}
public interface IZiBalPaymentHandler
{
    Task<ResultOperation<ZiBalPaymentResponse>> Handle(ZibalPaymentRequest request, CancellationToken cancellationToken);
}
public record ZiBalPaymentResponse
{
    public Uri Uri { get; set; }
    public string trackId { get; set; }
    public string result { get; set; }
    public string message { get; set; }

}

public class ZiBalPaymentHandler : IRequestHandler<ZibalPaymentRequest, ResultOperation<ZiBalPaymentResponse>> , IZiBalPaymentHandler
{
    public ZiBalPaymentHandler(IOptionsSnapshot<ZibalPaymentSettings> zibalPaymentSettings,ILogger<ZiBalPaymentHandler> logger)
    {
        ZibalPaymentSettings = zibalPaymentSettings.Value;
        Logger = logger;
    }
   
    public ZibalPaymentSettings ZibalPaymentSettings { get; set; }
    public ILogger<ZiBalPaymentHandler> Logger { get; }

    //public string description { get; set; }
    public async Task<ResultOperation<ZiBalPaymentResponse>> Handle(ZibalPaymentRequest request, CancellationToken cancellationToken)
    {
        Uri paymentLink;
        try
        {
            // url
            Zibal.makeRequest Request = new Zibal.makeRequest()
            {
                merchant = ZibalPaymentSettings.Merchant,// String
                orderId = request.orderId, // String
                amount = request.amount, //Integer
                callbackUrl = ZibalPaymentSettings.BaseCallbackUrl,//String
                description = "Hello Zibal !",// String
                mobile = request.mobile,
                allowedCards = request.allowedCards,

            };

            var httpResponse = Zibal.HttpRequestToZibal(ZibalPaymentSettings.UrlPaymentRequest, JsonConvert.SerializeObject(Request));  // get Response
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
            {
                var responseText = streamReader.ReadToEnd(); // read Response
                Zibal.makeRequest_response response = JsonConvert.DeserializeObject<Zibal.makeRequest_response>(responseText); // Deserilize as response class object
              //  int responseCode = int.Parse(response.result);
                return new ZiBalPaymentResponse
                {
                    Uri = new Uri($"{ZibalPaymentSettings.UrlToPayZibal}{response?.trackId}"),
                    message= response?.message,
                    trackId = response?.trackId,
                    result = response?.result
                    

                }.ToSuccessResult();

            }
        }
        catch (WebException ex)
        {
            Logger.LogError(ex.Message); // print exception error
            return ResultOperation<ZiBalPaymentResponse>.ToFailedResult($"--{ex.Message} --- {ex.InnerException?.Message} --");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message); // print exception error
            return ResultOperation<ZiBalPaymentResponse>.ToFailedResult($"--{ex.Message} --- {ex.InnerException?.Message} --");
        }
    }
}
