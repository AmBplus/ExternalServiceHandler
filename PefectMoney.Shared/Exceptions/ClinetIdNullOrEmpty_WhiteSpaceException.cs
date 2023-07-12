using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PefectMoney.Shared.Exceptions
{
    public class ClinetIdNullOrEmpty_WhiteSpaceException : ArgumentNullException
    {
        public ClinetIdNullOrEmpty_WhiteSpaceException() : base("کلاینت آیدی نال یا خالی یا پر از فضای خالی میباشد") { }
        public ClinetIdNullOrEmpty_WhiteSpaceException(string message) : base(message) { }
    }
}
