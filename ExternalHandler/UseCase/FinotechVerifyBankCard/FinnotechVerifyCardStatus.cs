using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PefectMoney.Shared.Utility;
using ExternalHandler.UseCase.FinotechVerifyBankCard;

namespace ExternalHandler.UseCase.FinotechVerifyBankCard
{
    public class FinnotechVerifyCardStatus : Enumeration<int, string>
    {

        public static FinnotechVerifyCardStatus DONE = new FinnotechVerifyCardStatus(1, "DONE");
        public static FinnotechVerifyCardStatus FAILED = new FinnotechVerifyCardStatus(2, "FAILED");
        public FinnotechVerifyCardStatus(int id, string name) : base(id, name)
        {
        }
    }
    public class FinnotechVerifyCardErrors : Enumeration<string, string>
    {
        public int StatusCode { get; set; }
        public static FinnotechVerifyCardErrors UNAUTHORIZED = new FinnotechVerifyCardErrors("UNAUTHORIZED", "DONE", 403);
        public static FinnotechVerifyCardErrors VALIDATION_ERROR = new FinnotechVerifyCardErrors("VALIDATION_ERROR", "عدم دسترسی به اسکوپ آدرس ارسالی", 400);
        public FinnotechVerifyCardErrors(string id, string name, int statusCode) : base(id, name)
        {
            StatusCode = statusCode;
        }


    }
}
