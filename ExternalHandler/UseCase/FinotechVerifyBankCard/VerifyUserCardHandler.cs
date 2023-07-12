using Microsoft.Extensions.Configuration;

using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Serialization;
using PefectMoney.Shared.Utility.ResultUtil;
using PefectMoney.Shared.Utility;
using Microsoft.Extensions.Options;
using MediatR;
using ExternalHandler.Settings;
using ExternalHandler.UseCase;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Authenticators.OAuth2;

namespace ExternalHandler.UseCase.FinotechVerifyBankCard
{
    public interface IVerifyUserCard
    {
        public Task<ResultOperation<VerifyUserCardResponseDto>> Handle(VerifyUserCardRequestDto request, CancellationToken cancellationToken);
    }
    public record VerifyUserCardRequestDto : IRequest<ResultOperation<VerifyUserCardResponseDto>>
    {
        [FromBody]
        public string token { get; set; }
        [FromBody]
        public string phoneNumber { get; set; }
        [FromBody]
        public string cartNumber { get; set; }
        [FromBody]
        public string trackId { get; set; }
    }
    public class VerifyUserCardHandler : IRequestHandler<VerifyUserCardRequestDto, ResultOperation<VerifyUserCardResponseDto>>, IVerifyUserCard
    {

        public VerifyUserCardHandler(IOptionsSnapshot<VerifyAccountSettings> configuration, IVerifyCardToken verifyCartToken, ILogger<VerifyUserCardHandler> logger)
        {
            VerifyAccountSettings = configuration.Value;
            VerifyCartToken = verifyCartToken;
            Logger = logger;
            RestClient = new RestClient();

        }
        RestClient RestClient { get; set; }
        VerifyAccountSettings VerifyAccountSettings { get; set; }

        public IVerifyCardToken VerifyCartToken { get; }
        public ILogger<VerifyUserCardHandler> Logger { get; }

        public async Task<ResultOperation<VerifyUserCardResponseDto>> Handle(VerifyUserCardRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                if (VerifyAccountSettings.Address == "https://sandboxapi.finnotech.ir")
                {

                    return new VerifyUserCardResponseDto()
                    {
                        Result = new VerifyUserCardResponseResultDto() { IsValid = true },
                    }.ToSuccessResult();
                }
                var token = await VerifyCartToken.GetToken();
                if (request.phoneNumber.Contains("+"))
                {
                    request.phoneNumber = request.phoneNumber.Replace("+98", "0");
                }
                var result = await GenerateVerifyRestRequestAsync(request.phoneNumber, request.cartNumber, request.trackId, token);
                Logger.LogInformation(result.Content);
                Logger.LogInformation("--------");
                Logger.LogInformation(result?.Data?.ToString());
                if (result.Data ==null)
                {
                    Logger.LogError("Data is Null");
                    return ResultOperation<VerifyUserCardResponseDto>.ToFailedResult();
                }
               
                return result.Data.ToSuccessResult();
           
           
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return ResultOperation<VerifyUserCardResponseDto>.ToFailedResult($"خطای سیستمی به وجود آمده به ادمین اطلاع دهید {e.Message} --- {e.InnerException?.Message}");
            }
        }

        private async Task<RestResponse<VerifyUserCardResponseDto>> GenerateVerifyRestRequestAsync(string phoneNumber, string cartNumber, string trackId, string token)
        {

            var options = new RestClientOptions($"{VerifyAccountSettings.Address}")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/kyc/v2/clients/{VerifyAccountSettings.ClientId}/mobileCardVerification?trackId={trackId}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var authenticator = new JwtAuthenticator(token);
            request.Authenticator = authenticator;
       
            request.AddBody(new
            {
                card = cartNumber,
                mobile = phoneNumber,
            });
        
            var response = await client.ExecuteAsync<VerifyUserCardResponseDto>(request);
            return response;
          
            

        }


    }
    public record veriRequest
    {

        public string mobile { get; set; } 
        public string cart { get; set; }
    }
    public class VerifyUserCardResponseErrorCode : Enumeration<string, string>
    {
        public TypeErrorCodeStatus Status { get; set; }
        public static VerifyUserCardResponseErrorCode UnValidNationalCode = new("FN-KCFH-40000530009", "کد ملی معتبر نیست", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode UnValidNationalCode1 = new("FN-KCFH-40000130009", "کد ملی معتبر نیست", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode UnValidNationalCode2 = new("FN-KCFH-40000630009", "کد ملی معتبر نیست", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static VerifyUserCardResponseErrorCode UnValidNationalCodeAndMobileNumber = new("FN-KCFH-40000130058", "شماره موبایل یا کد ملی نا معتبر است", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static VerifyUserCardResponseErrorCode NationalCodeIsNecessary = new("FN-KCFH-400006300127", "پارامتر کد ملی الزامی است", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode NationalCodeIsNecessary2 = new("FN-KCFH-40001030060", "کد ملی الزامی است ", TypeErrorCodeStatus.ApplicationError);
        public static VerifyUserCardResponseErrorCode NationalCodeIsNecessary3 = new("FN-KCFH-40000230060", "کد ملی الزامی است", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode MobileNumberIsNecessary = new("FN-KCFH-40000230041", "موبایل اجباری است", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode UnValidMobileNumber = new("FN-KCFH-400005300578", "شماره موبایل  نا معتبر است", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode UnValidMobileNumber1 = new("FN-KCFH-40000530057", "شماره موبایل  نا معتبر است", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode UnValidMobileNumber2 = new("FN-KCFH-40000130057", "شماره موبایل  نا معتبر است", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode NationalForDeathPerson = new("FN-KCFH-40000830046", "کد ملی ارسال شده برای شخص فوت شده است", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode NetworkError = new("FN-KCAA-50000860000", "خطای داخل شبکه", TypeErrorCodeStatus.DestinationSystemError);
        public static VerifyUserCardResponseErrorCode NetworkError1 = new("FN-KCAA-50000960000", "خطای داخل شبکه", TypeErrorCodeStatus.DestinationSystemError);
        public static VerifyUserCardResponseErrorCode NetworkError2 = new("FN-KCHR-50001060000", "خطای داخل شبکه", TypeErrorCodeStatus.DestinationSystemError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode AuthenticationRequestNotFound = new("FN-KCFH-40400030038", "درخواست احراز هویت یافت نشد", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode AuthenticationRequestNotFound2 = new("FN-KCFH-40400730038", "درخواست احراز هویت یافت نشد", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------


        public static VerifyUserCardResponseErrorCode UserInBankBlackList = new("FN-KCKZ-40000430037", "کاربر در لیست سیاه بانک است ", TypeErrorCodeStatus.UserError);
        public static VerifyUserCardResponseErrorCode UserInBankBlackList2 = new("FN-KCKZ-20000530037", "کاربر در لیست سیاه بانک است ", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode ValidationError = new("FN-KCFH-40000530039", "Validation Error", TypeErrorCodeStatus.ApplicationError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static VerifyUserCardResponseErrorCode NationalCodeAndMobileNumberDoNotMatch = new("FN-KCFH-200001000005", "کد ملی و شماره موبایل منطبق نیست ", TypeErrorCodeStatus.UserError);
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public VerifyUserCardResponseErrorCode(string id, string messsage, TypeErrorCodeStatus status) : base(id, messsage)
        {
            Status = status;
        }


        public static VerifyUserCardResponseErrorCode? VerifyMatchToError(string errorId)
        {
            foreach (var item in GetAll<VerifyUserCardResponseErrorCode>())
            {
                if (item.Id == errorId)
                {
                    return item;
                }
            }
            return null;

        }
    }


    public class VerifyUserCardResponseDto
    {
        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("trackId")]
        public string TrackId { get; set; }

        [JsonPropertyName("result")]
        public VerifyUserCardResponseResultDto Result { get; set; }

        [JsonPropertyName("error")]
        public VerifyUserCardResponseErrorDto Error { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

    }
    public class VerifyUserCardResponseResultDto
    {
        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }
    }
    public class VerifyUserCardResponseErrorDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("message")]
        public string message { get; set; }
    }

  
}
