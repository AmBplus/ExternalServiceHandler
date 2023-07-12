using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PefectMoney.Shared.Exceptions
{
 
    public class ClientSecrectNullOrEmpty_WhiteSpace_Exception : ArgumentNullException
    {
        public ClientSecrectNullOrEmpty_WhiteSpace_Exception() : base("کلاینت سیکرت نال یا خالی یا پر از فضای خالی میباشد") { }
        public ClientSecrectNullOrEmpty_WhiteSpace_Exception(string message) : base(message) { }
    }
}
